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
        public abstract Type GetSettingsType();

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
    public abstract class Solver<TInputs, TSettings, TOutputs> : Solver
        where TInputs : SolverInputs
        where TSettings : SolverSettings
        where TOutputs : SolverOutputs
    {
        public Solver() : base() { }
        public Solver(TInputs inputs, TSettings settings, string name = "")
        {
            Inputs = inputs;
            Settings = settings;
            Name = name;
        }
        public Solver(string name) : base(name) { }
        public TInputs Inputs { get; set; }
        public TSettings Settings { get; set; }
        public TOutputs Outputs { get; set; }

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

        public override string ToJSON()
        {
            SortedList<string, object> data = new SortedList<string, object>();
            data.Add("SolverType", GetType().FullName);
            data.Add("Assembly", GetType().Assembly.FullName);
            if (Inputs != null) data.Add("Inputs", Inputs);
            if (Settings != null) data.Add("Settings", Settings);
            if (Outputs != null) data.Add("Outputs", Outputs);

            var sets = new Newtonsoft.Json.JsonSerializerSettings();
            sets.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(data, sets);
        }
    }
}