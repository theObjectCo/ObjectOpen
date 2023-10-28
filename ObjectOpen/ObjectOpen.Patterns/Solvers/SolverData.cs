namespace ObjectOpen.Patterns.Solvers
{
    public abstract class SolverData
    {
    }
    public abstract class SolverInputs : SolverData
    {
    }
    public abstract class SolverSettings : SolverData
    {
    }
    public abstract class SolverOutputs : SolverData
    {
    }
    public class EmptySettings : SolverSettings
    {
    }
}
