using System;
using System.Collections.Generic;

namespace ObjectOpen.Patterns.Solvers
{
    public abstract class Solver
    {
        public Solver() { }
        public abstract Type GetInputType();
        public abstract Type GetOutputType();
        public abstract Type GetSettingType();

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
    public abstract class Solver<TInput, TSetting, TOutput> : Solver
        where TInput : SolverInputs
        where TSetting : SolverSettings
        where TOutput : SolverOutputs
    {
        public Solver() : base() { }
        public Solver(TInput inputs, TSetting settings)
        {
            Inputs = inputs;
            Settings = settings;
        }
        public TInput Inputs { get; set; }
        public TSetting Settings { get; set; }
        public TOutput Outputs { get; set; }

        public override Type GetInputType()
        {
            return typeof(TInput);
        }
        public override Type GetOutputType()
        {
            return typeof(TOutput);
        }
        public override Type GetSettingType()
        {
            return typeof(TSetting);
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