using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.Models
{
    public class BMICalculator
    {
        private const double CM_TO_M_MULTIPLIER = 0.01;
        private const double IMPERIAL_MULTIPLIER = 703;

        private Units _units;
        public BMICalculator(Units units)
        {
            if (units == Units.None) throw new Exception($"Invalid {nameof(units)}: {Units.None}");
            _units = units;
        }

        public double Calculate(double weight, double height)
        {
            if (_units == Units.Metric)
                return ComputeMetricBMI(weight, height);
            else //its imperial
                return ComputeImperialBMI(weight, height);
        }

        private double ComputeMetricBMI(double weight, double height)
        {
            if (double.IsNaN(weight)) throw new ArgumentException($"{nameof(weight)} is not a number.");
            if (double.IsNaN(height)) throw new ArgumentException($"{nameof(height)} is not a number.");

            if (double.IsInfinity(weight)) throw new ArgumentException($"{nameof(weight)} is infinite.");
            if (double.IsInfinity(height)) throw new ArgumentException($"{nameof(height)} is infinite.");

            if (height == 0) throw new ArgumentException($"{nameof(height)} cannot be 0.");

            height *= CM_TO_M_MULTIPLIER;

            double height2 = height * height;

            //check for double overflow 
            if (double.IsInfinity(height2)) throw new ArgumentException($"{nameof(height2)} is infinite.");

            double result = weight / height2;

            //because heigth2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new ArgumentException($"{nameof(result)} is infinite.");

            return result;
        }

        private double ComputeImperialBMI(double weight, double height)
        {
            if (double.IsNaN(weight)) throw new ArgumentException($"{nameof(weight)} is not a number.");
            if (double.IsNaN(height)) throw new ArgumentException($"{nameof(height)} is not a number.");

            if (double.IsInfinity(weight)) throw new ArgumentException($"{nameof(weight)} is infinite.");
            if (double.IsInfinity(height)) throw new ArgumentException($"{nameof(height)} is infinite.");

            if (height == 0) throw new ArgumentException($"{nameof(height)} cannot be 0.");

            double height2 = height * height;

            //check for double overflow 
            if (double.IsInfinity(height2)) throw new ArgumentException($"{nameof(height2)} is infinite.");

            double result = (weight / height2) * IMPERIAL_MULTIPLIER;

            //because heigth2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new ArgumentException($"{nameof(result)} is infinite.");

            return result;
        }

    }
}