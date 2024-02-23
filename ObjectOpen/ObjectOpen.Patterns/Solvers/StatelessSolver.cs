using System;
using System.Collections.Generic;

namespace ObjectOpen.Patterns.Solvers
{
    public abstract class StatelessSolver
    {
        public string Name { get; set; }

        public StatelessSolver() { }

        protected StatelessSolver(string name)
        {
            Name = name;
        }

        public abstract Type GetInputsType();
        
        public abstract Type GetOutputsType();

        public virtual string ToJSON() { return ""; }
    }

    public abstract class StatelessSolver<TInputs, TOutputs> : StatelessSolver
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