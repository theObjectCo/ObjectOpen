using System.Linq;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.Solvers
{
    public class AdditionDirector : Director<DirectorInputs, DirectorSettings, DirectorOutputs>
    {

        private AdditionSolver SolverA { get; set; } = new AdditionSolver("SolverA");
        private AdditionSolver SolverB { get; set; } = new AdditionSolver("SolverB");
        private AdditionSolver SolverC { get; set; } = new AdditionSolver("SolverC");

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

            //demonstrating the topology-based solver sorting, these will get sorted automatically
            this.Solvers.Add(SolverB);
            this.Solvers.Add(SolverC);
            this.Solvers.Add(SolverA);
            return new Result();
        }

        public override void UpdateConnections(out Result result)
        {
            result = new Result(); 

            //director input to solver a
            result.Combine(this.TryAddConnection(new DataConnection(this, SolverA, nameof(this.Inputs.Value), nameof(SolverA.Inputs.ValueA))));
            result.Combine(this.TryAddConnection(new DataConnection(this, SolverA, nameof(this.Inputs.Value), nameof(SolverA.Inputs.ValueB))));

            //director input to solver b
            result.Combine(this.TryAddConnection(new DataConnection(this, SolverB, nameof(this.Inputs.Value), nameof(SolverB.Inputs.ValueA))));
            result.Combine(this.TryAddConnection(new DataConnection(this, SolverB, nameof(this.Inputs.Value), nameof(SolverB.Inputs.ValueB))));

            //solver a and b to solver c
            result.Combine(this.TryAddConnection(new DataConnection(SolverA, SolverC, nameof(SolverA.Outputs.Value), nameof(SolverC.Inputs.ValueA))));
            result.Combine(this.TryAddConnection(new DataConnection(SolverB, SolverC, nameof(SolverB.Outputs.Value), nameof(SolverC.Inputs.ValueB))));

            //solver c to director output
            result.Combine(this.TryAddConnection(new DataConnection(SolverC, this, nameof(SolverC.Outputs.Value), nameof(this.Outputs.Value))));
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
