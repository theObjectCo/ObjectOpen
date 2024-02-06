using System;
using System.Collections.Generic;

namespace ObjectOpen.Patterns.Solvers
{
    public abstract class Solverino
    {
        protected Solverino() { }
        protected Solverino(string name) { Name = name; }
        public string Name { get; set; }
        public abstract Type GetInputsType();
        public abstract Type GetOutputsType();
        public virtual string ToJSON() { return ""; }
    }

    public abstract class Solverino<TInputs, TOutputs> : Solverino
    where TInputs : SolverInputs
    where TOutputs : SolverOutputs
    {
        protected Solverino() : base() { }
        public Solverino(string name = "") : base(name) { }
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

        public TInputs Inputs { get; set; }
        public TOutputs Outputs { get; set; }
        public abstract Result SolveInternal();
    }

    public abstract class StatelessSolverino<TInputs, TOutputs> : Solverino
        where TInputs : SolverInputs
        where TOutputs : SolverOutputs
    {
        public StatelessSolverino() : base() { }
        public StatelessSolverino(string name = "") : base(name) { }
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
