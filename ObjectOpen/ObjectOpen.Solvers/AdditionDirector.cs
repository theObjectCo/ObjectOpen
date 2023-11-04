using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.Solvers
{
    public class AdditionDirector : Director<DirectorInputs, DirectorSettings, DirectorOutputs>
    {

        private AdditionSolver SolverA { get; set; } = new AdditionSolver();
        private AdditionSolver SolverB { get; set; } = new AdditionSolver();
        private AdditionSolver SolverC { get; set; } = new AdditionSolver();

        public override Result CreateInputsOutputs()
        {
            this.Inputs = new DirectorInputs();
            this.Outputs = new DirectorOutputs();
            return new Result();
        }

        public override Result InitSolvers()
        {
            SolverA.Inputs = new AdditionInputs(); 
            SolverB.Inputs = new AdditionInputs(); 
            SolverC.Inputs = new AdditionInputs(); 

            this.Solvers.Add(SolverA);
            this.Solvers.Add(SolverB);
            this.Solvers.Add(SolverC);
            return new Result();
        }

        public override Result UpdateConnections()
        {
            //director input to solver a
            this.DataConnections.Add(new DataConnection(this, SolverA, nameof(this.Inputs.Value), nameof(SolverA.Inputs.ValueA)));
            this.DataConnections.Add(new DataConnection(this, SolverA, nameof(this.Inputs.Value), nameof(SolverA.Inputs.ValueB)));

            //director input to solver b
            this.DataConnections.Add(new DataConnection(this, SolverB, nameof(this.Inputs.Value), nameof(SolverB.Inputs.ValueA)));
            this.DataConnections.Add(new DataConnection(this, SolverB, nameof(this.Inputs.Value), nameof(SolverB.Inputs.ValueB)));

            //solver a and b to solver c
            this.DataConnections.Add(new DataConnection(SolverA, SolverC, nameof(SolverA.Outputs.Value), nameof(SolverC.Inputs.ValueA)));
            this.DataConnections.Add(new DataConnection(SolverB, SolverC, nameof(SolverB.Outputs.Value), nameof(SolverC.Inputs.ValueB)));

            //solver c to director output
            this.DataConnections.Add(new DataConnection(SolverC, this, nameof(SolverC.Outputs.Value), nameof(this.Outputs.Value)));

            return new Result();
        }
    }

    public class DirectorInputs : SolverInputs
    {
        public double Value { get; set; }
    }
    public class DirectorSettings : SolverSettings { }
    public class DirectorOutputs : SolverOutputs
    {
        public double Value { get; set; }

    }
}
