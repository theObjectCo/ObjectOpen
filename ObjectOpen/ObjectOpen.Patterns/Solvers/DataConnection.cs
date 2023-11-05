using System;

namespace ObjectOpen.Patterns.Solvers
{
    public class DataConnection : IEquatable<DataConnection>
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

        public bool Equals(DataConnection other)
        {
            if (!ReferenceEquals(this.SourceSolver, other.SourceSolver)) return false;
            if (!ReferenceEquals(this.TargetSolver, other.TargetSolver)) return false;
            if (!(this.SourcePropertyName == other.SourcePropertyName)) return false;
            if (!(this.TargetPropertyName == other.TargetPropertyName)) return false;
            return true;
        }
    }
}