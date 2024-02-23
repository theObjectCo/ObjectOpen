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
    public abstract class Director<TInputs, TOutputs> : Solver
        where TInputs : SolverInputs
        where TOutputs : SolverOutputs
    {
        public Director()
        {
            this.CreateInputsOutputs();
            this.InitSolvers();
        }

        public List<Solver> Solvers { get; set; } = new List<Solver>();
        private List<Solver> SortedSolvers { get; set; } = new List<Solver>();
        private List<DataConnection> DataConnections { get; set; } = new List<DataConnection>();
        public TInputs Inputs { get; set; }
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
        public abstract void UpdateConnections(out Result result);

        public Result TryAddConnection(DataConnection connection)
        {
            if (ReferenceEquals(connection.SourceSolver, connection.TargetSolver)) return new Result(Flag.Error, "Source and Target solver is the same instance");

            var propertyCheck = CheckProperties(connection);
            if (propertyCheck.Flag == Flag.Error) return propertyCheck;

            foreach (var item in DataConnections)
            {
                if (item.Equals(connection)) return new Result(Flag.Error, "Connection already exists.");

                if (ReferenceEquals(item.TargetSolver, connection.TargetSolver))
                    if (item.TargetPropertyName == connection.TargetPropertyName) return new Result(Flag.Error, $"Target property already used. (Property {item.TargetPropertyName} in solver {item.TargetSolver.GetType().Name})");
            }

            DataConnections.Add(connection);

            return new Result();
        }

        private Result CheckProperties(DataConnection connection)
        {
            Type sourceType = connection.SourceSolver == this ? connection.SourceSolver.GetInputsType() : connection.SourceSolver.GetOutputsType();
            Type targetType = connection.TargetSolver == this ? connection.TargetSolver.GetOutputsType() : connection.TargetSolver.GetInputsType();

            PropertyInfo sourceProperty = sourceType.GetProperties()?.Where((x) => x.Name == connection.SourcePropertyName).FirstOrDefault();
            PropertyInfo targetProperty = targetType.GetProperties()?.Where((x) => x.Name == connection.TargetPropertyName).FirstOrDefault();

            if (sourceProperty == null) return new Result(Flag.Error, $"Property {sourceProperty} not found in the source solver");
            if (targetProperty == null) return new Result(Flag.Error, $"Property {targetProperty} not found in the target solver");

            if (targetProperty.GetType() != sourceProperty.GetType()) return new Result(Flag.Error, $"Property type mismatch");

            return new Result();
        }

        /// <summary>
        /// This method is called once for each Solve, before updating the connections.
        /// It's a perfect spot to modify the Solvers list dynamically. 
        /// </summary>
        /// <returns></returns>
        public virtual void UpdateSolvers(out Result result) { result = new Result(); }

        /// <summary>
        /// Sorts the solvers in the execution order based on the connections.
        /// </summary>
        /// <returns></returns>
        private Result SortSolvers()
        {
            SortedSolvers.Clear();

            List<Tuple<Solver, int>> sorted = new List<Tuple<Solver, int>>();

            sorted.Add(new(this, 0));
            foreach (Solver solver in Solvers)
                sorted.Add(new(solver, -1));

            bool anythingChanged = true;

            for (int i = 0; i < Solvers.Count + 3; i++) //should not be possible to have longer paths
            {
                anythingChanged &= AssignValues(sorted, DataConnections);
                if (!anythingChanged) break;
            }

            sorted.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            sorted = sorted.Where((x) => x.Item2 != 0).ToList();

            SortedSolvers.AddRange(sorted.Select((x) => x.Item1));

            return anythingChanged ? new Result(Flag.Error, "Couldn't sort the execution graph. This may indicate cyclical dependencies.") : new Result();
        }

        private bool AssignValues(List<Tuple<Solver, int>> solvers, List<DataConnection> connections)
        {
            bool anythingChanged = false;

            foreach (var item in connections)
            {
                var sourceTuple = solvers.Where((x) => x.Item1 == item.SourceSolver).FirstOrDefault();
                if (sourceTuple.Item2 == -1) continue;

                var targetTuple = solvers.Where((x) => x.Item1 == item.TargetSolver).FirstOrDefault();
                if (targetTuple.Item1 == this) continue;
                if (sourceTuple.Item2 < targetTuple.Item2) continue;

                var index = solvers.IndexOf(targetTuple);
                solvers[index] = new(targetTuple.Item1, targetTuple.Item2 + 1);
                anythingChanged = true;
            }

            return anythingChanged;
        }

        /// <summary>
        /// This method is called before each Solver.Solve, will update the data in the connections where the current solver is the target. 
        /// </summary>
        /// <param name="solver">Solver that is pulling the data.</param>
        private Result PullData(Solver solver)
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

        private SolverInputs GetSolverInputs(Solver solver)
        {
            foreach (var item in solver.GetType().GetProperties())
                if (item.PropertyType.IsSubclassOf(typeof(SolverInputs)))
                    return (SolverInputs)item.GetValue(solver);

            return default;
        }

        private SolverOutputs GetSolverOutputs(Solver solver)
        {
            foreach (var item in solver.GetType().GetProperties())
                if (item.PropertyType.IsSubclassOf(typeof(SolverOutputs)))
                    return (SolverOutputs)item.GetValue(solver);

            return default;
        }

        public override Result SolveInternal()
        {
            UpdateSolvers(out Result updateSolvers);
            if (updateSolvers.Flag == Flag.Error) return updateSolvers;

            this.DataConnections.Clear(); //purge the connection list before update
            UpdateConnections(out Result updateConnections);
            if (updateConnections.Flag == Flag.Error) return updateConnections;

            this.SortedSolvers.Clear(); //purge the solver list before update
            var sortSolvers = SortSolvers();
            if (sortSolvers.Flag == Flag.Error) return sortSolvers;

            foreach (var item in SortedSolvers)
            {
                var pullResult = PullData(item);
                if (pullResult.Flag == Flag.Error) return pullResult;

                var result = item.Solve();
                if (result.Flag == Flag.Error) return result;
            }

            PullData(this);

            return new Result();
        }

        public override string ToJSON()
        {
            SortedList<string, object> data = new SortedList<string, object>
            {
                { "SolverType", GetType().FullName },
                { "Assembly", GetType().Assembly.FullName }
            };

            if (Inputs != null) data.Add("Inputs", Inputs);
            if (Outputs != null) data.Add("Outputs", Outputs);

            var sets = new Newtonsoft.Json.JsonSerializerSettings();
            sets.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(data, sets);
        }
    }
}