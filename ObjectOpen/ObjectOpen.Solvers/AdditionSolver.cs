using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.Solvers
{
    /// <summary>
    /// The solver base class requires to spec the 3 types, that is input, settings and output data.
    /// If the settings are not necessary, EmptySettings class can be used. 
    /// </summary>
    public class AdditionSolver : Solver<AdditionInputs, EmptySettings, AdditionOutputs>
    {
        public AdditionSolver() : base() { }
        public AdditionSolver(string name) : base(name) { }

        public override Result SolveInternal()
        {
            this.Outputs = new AdditionOutputs() { Value = Inputs.ValueA + Inputs.ValueB };
            return new Result(Flag.OK);
        }
    }
    public class AdditionInputs : SolverInputs
    {
        public double ValueA { get; set; }
        public double ValueB { get; set; }
    }
    public class AdditionOutputs : SolverOutputs
    {
        public double Value { get; set; }
    }
}