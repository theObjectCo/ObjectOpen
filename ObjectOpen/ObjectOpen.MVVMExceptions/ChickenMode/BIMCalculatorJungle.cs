using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.ChickenMode
{
    public class BMICalculatorJungle
    {
        private const double CM_TO_M_MULTIPLIER = 0.01;
        private const double IMPERIAL_MULTIPLIER = 703;

        private Range _weightRange = new Range(0, 1000);
        private Range _heightRange = new Range(0, 1000);

        private readonly Units _units;
        public BMICalculatorJungle(Units units)
        {
            if (units == Units.None) throw new ArgumentException($"{nameof(units)}: can't be {Units.None}");
            _units = units;
        }

        public double Calculate(double weight, double height)
        {
            if (!_weightRange.IsWithinInclusive(weight))
                throw new ArgumentException($"{nameof(weight)} has to fit {_weightRange}");
            if (!_heightRange.IsWithinInclusive(height))
                throw new ArgumentException($"{nameof(height)} has to fit {_heightRange}");

            switch (_units)
            {
                case Units.Metric:
                    return ComputeMetricBMI(weight, height);

                case Units.Imperial:
                    return ComputeImperialBMI(weight, height);

                // Since Units does not originate from the current context, chances are that it might be changed without this being aware
                default:
                    throw new NotImplementedException($"There is no implementation for {_units}");
            }
        }

        private static double ComputeMetricBMI(double weight, double height)
        {
            height *= CM_TO_M_MULTIPLIER;
            double heightSq = height * height;

            return weight / heightSq;
        }

        private static double ComputeImperialBMI(double weight, double height)
        {
            double heightSq = height * height;

            return weight / heightSq * IMPERIAL_MULTIPLIER;
        }

        private struct Range : IEquatable<Range>
        {
            public double Max { get; }
            public double Min { get; }
            public Range(double min, double max)
            {
                if (double.IsNaN(min) || double.IsNaN(max))
                    throw new ArgumentException($"Input arguments can't be {double.NaN}");

                Min = min;
                Max = max;
            }

            public bool IsWithinInclusive(double value)
            {
                if (double.IsNaN(value))
                    throw new ArgumentException($"{nameof(value)} can't be {double.NaN}");

                return value >= Min & value <= Max;
            }

            public override string ToString() => $"Range: from {Min} to {Max}";


            #region IEquatable implementation along with GetHashCode override

            public readonly override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (obj.GetType() != typeof(Range)) return false;

                return Equals((Range)obj);
            }

            public readonly bool Equals(Range other) => Min == other.Min && Max == other.Max;

            public static bool operator ==(Range left, Range right) => left.Equals(right);

            public static bool operator !=(Range left, Range right) => !left.Equals(right);

            public readonly override int GetHashCode()
            {
                // As the example from Josh Blochs' Effective Java
                unchecked
                {
                    int hash = 17;

                    hash = hash * 23 + Min.GetHashCode();
                    hash = hash * 23 + Max.GetHashCode();

                    return hash;
                }
            }

            #endregion
        }
    }
}
