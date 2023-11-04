using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectOpen.Patterns.Solvers
{

    /// <summary>
    /// Director calls the solvers sequentally, passing the resulting data forward using connections.
    /// </summary>
    /// <typeparam name="TInputs"></typeparam>
    /// <typeparam name="TSettings"></typeparam>
    /// <typeparam name="TOutputs"></typeparam>
    public abstract class Director<TInputs, TSettings, TOutputs> : Solver
        where TInputs : SolverInputs
        where TSettings : SolverSettings
        where TOutputs : SolverOutputs
    {
        public Director()
        {
            this.CreateInputsOutputs();
            this.InitSolvers();
        }

        public List<Solver> Solvers { get; set; } = new List<Solver>();
        public List<DataConnection> DataConnections { get; set; } = new List<DataConnection>();
        public TInputs Inputs { get; set; } 
        public TSettings Settings { get; set; }
        public TOutputs Outputs { get; set; }
        
        /// <summary>
        /// Ensure the director has inputs and outputs initialized before the first run.
        /// </summary>
        /// <returns></returns>
        public abstract Result CreateInputsOutputs();

        public override Type GetInputsType()
        {
            return typeof(TInputs);
        }

        public override Type GetOutputsType()
        {
            return typeof(TOutputs);
        }

        public override Type GetSettingsType()
        {
            return typeof(TSettings);
        }

        /// <summary>
        /// This method is called once in the constructor. 
        /// If you want to modify the Solver list dynamically, override the UpdateSolvers() method.
        /// </summary>
        /// <returns></returns>
        public abstract Result InitSolvers();

        /// <summary>
        /// Every time before calling the solvers, the Director will Update the connection list.
        /// The connection update will be performed in the order specified here.
        /// </summary>
        public abstract Result UpdateConnections();

        /// <summary>
        /// This method is called once for each Solve, before updating the connections.
        /// It's a perfect spot to modify the Solvers list dynamically. 
        /// </summary>
        /// <returns></returns>
        public virtual Result UpdateSolvers() { return new Result(); }

        /// <summary>
        /// This method is called after each Solver.Solve, will update the data in the connections where the current solver is the source. 
        /// </summary>
        /// <param name="solver">Solver that is pushing the data.</param>
        public Result PushData(Solver solver)
        {
            var connectionsToUpdate = DataConnections.Where((x) => x.SourceSolver == solver).ToList();
            return PassData(connectionsToUpdate);
        }

        /// <summary>
        /// This method is called before each Solver.Solve, will update the data in the connections where the current solver is the target. 
        /// </summary>
        /// <param name="solver">Solver that is pulling the data.</param>
        public Result PullData(Solver solver)
        {
            var connectionsToUpdate = DataConnections.Where((x) => x.TargetSolver == solver).ToList();
            return PassData(connectionsToUpdate);
        }

        private Result PassData(IEnumerable<DataConnection> connections)
        {
            foreach (var connection in connections)
            {
                PropertyInfo sourceProperty;
                PropertyInfo targetProperty;
                object sourceInstance;
                object targetInstance;

                sourceInstance = connection.SourceSolver == this ? GetSolverInputs(connection.SourceSolver) : GetSolverOutputs(connection.SourceSolver);
                targetInstance = connection.TargetSolver == this ? GetSolverOutputs(connection.TargetSolver) : GetSolverInputs(connection.TargetSolver);

                Type sourceType = sourceInstance.GetType();
                Type targetType = targetInstance.GetType();

                sourceProperty = sourceType.GetProperties()?.Where((x) => x.Name == connection.SourcePropertyName).FirstOrDefault();
                targetProperty = targetType.GetProperties()?.Where((x) => x.Name == connection.TargetPropertyName).FirstOrDefault();

                if (sourceProperty == null) return new Result(Flag.Error, $"Property {sourceProperty} not found in the source solver");
                if (targetProperty == null) return new Result(Flag.Error, $"Property {targetProperty} not found in the target solver");

                if (targetProperty.GetType() != sourceProperty.GetType()) return new Result(Flag.Error, $"Property type mismatch");

                targetProperty.SetValue(targetInstance, sourceProperty.GetValue(sourceInstance));
            }

            return new Result();
        }

        public SolverInputs GetSolverInputs(Solver solver)
        {
            foreach (var item in solver.GetType().GetProperties())
                if (item.PropertyType.IsSubclassOf(typeof(SolverInputs)))
                    return (SolverInputs)item.GetValue(solver);

            return default;
        }

        public SolverOutputs GetSolverOutputs(Solver solver)
        {
            foreach (var item in solver.GetType().GetProperties())
                if (item.PropertyType.IsSubclassOf(typeof(SolverOutputs)))
                    return (SolverOutputs)item.GetValue(solver);

            return default;
        }

        public override Result SolveInternal()
        {
            var updateSolvers = UpdateSolvers();
            if (updateSolvers.Flag == Flag.Error) return updateSolvers;

            this.DataConnections.Clear(); //purge the connection list before update
            var updateResult = UpdateConnections();
            if (updateResult.Flag == Flag.Error) return updateResult;

            foreach (var item in Solvers)
            {
                var pullResult = PullData(item);
                if (pullResult.Flag == Flag.Error) return pullResult;

                var result = item.Solve();
                if (result.Flag == Flag.Error) return result;

                var pushResult = PushData(item);
                if (pushResult.Flag == Flag.Error) return pushResult;
            }

            return new Result();
        }
    }
}