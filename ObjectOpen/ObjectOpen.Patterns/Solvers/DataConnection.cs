namespace ObjectOpen.Patterns.Solvers
{
    public class DataConnection
    {
        public DataConnection() { }
        public DataConnection(Solver sourceSolver, Solver targetSolver, string sourcePropertyName, string targetPropertyName)
        {
            SourceSolver = sourceSolver;
            TargetSolver = targetSolver;
            SourcePropertyName = sourcePropertyName;
            TargetPropertyName = targetPropertyName;
        }

        public Solver SourceSolver { get; set; }
        public Solver TargetSolver { get; set; }
        public string SourcePropertyName { get; set; }
        public string TargetPropertyName { get; set; }

    }
}