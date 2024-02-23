using System;
using System.Collections.Generic;

namespace ObjectOpen.Patterns.Solvers
{

    public abstract class Solver
    {
        public Solver() { }

        public string Name { get; set; }
        protected Solver(string name)
        {
            Name = name;
        }

        public abstract Type GetInputsType();
        public abstract Type GetOutputsType();

        public virtual Result Solve()
        {
            Result res;

            try
            {
                res = SolveInternal();
            }
            catch (Exception ex)
            {
                res = new Result(Flag.Error, ex.Message);
            }

            return res;
        }

        public virtual string ToJSON() { return ""; }
        public abstract Result SolveInternal();
    }
    public abstract class Solver<TInputs, TOutputs> : Solver
        where TInputs : SolverInputs
        where TOutputs : SolverOutputs
    {
        public Solver() : base() { }
        public Solver(TInputs inputs, string name = "")
        {
            Inputs = inputs;
            Name = name;
        }
        public Solver(string name) : base(name) { }
        public TInputs Inputs { get; set; }
        public TOutputs Outputs { get; set; }

        public override Type GetInputsType()
        {
            return typeof(TInputs);
        }
        public override Type GetOutputsType()
        {
            return typeof(TOutputs);
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

    public abstract class StatelessSolver<TInputs, TOutputs> : Solver
      where TInputs : SolverInputs
      where TOutputs : SolverOutputs
    {
        public StatelessSolver() : base() { }
        public StatelessSolver(string name = "") : base(name) { }
        public abstract Result<TOutputs> SolveInternal(TInputs inputs);
        public virtual Result<TOutputs> Solve(TInputs inputs)
        {
            Result<TOutputs> res;

            try
            {
                res = SolveInternal(inputs);
            }
            catch (Exception ex)
            {
                res = new Result<TOutputs>(Flag.Error, ex.Message);
            }

            return res;
        }
        public override Type GetInputsType()
        {
            return typeof(TInputs);
        }
        public override Type GetOutputsType()
        {
            return typeof(TOutputs);
        }
        public override string ToJSON()
        {
            SortedList<string, object> data = new SortedList<string, object>
            {
                { "SolverType", GetType().FullName },
                { "Assembly", GetType().Assembly.FullName }
            };

            var sets = new Newtonsoft.Json.JsonSerializerSettings();
            sets.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(data, sets);
        }
    }
}