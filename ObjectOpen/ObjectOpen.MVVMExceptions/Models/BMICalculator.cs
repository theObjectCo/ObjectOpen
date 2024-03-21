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
            //constructor changes the class state, hence the need for validation
            if (units == Units.None) throw new ArgumentException($"Invalid {nameof(units)}: {Units.None}");
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
            height *= CM_TO_M_MULTIPLIER;
            double height2 = height * height;
            double result = weight / height2; //this will throw if height2 is 0 (and it's 0 if height == 0)

            //because height2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new InvalidOperationException($"{nameof(result)} is infinite.");
            if (double.IsNaN(result)) throw new InvalidOperationException($"{nameof(result)} is NaN.");

            //we know result is not null, not infinity and not nan, we can safely return it
            return result;
        }

        private double ComputeImperialBMI(double weight, double height)
        {
            double height2 = height * height;
            double result = weight / height2 * IMPERIAL_MULTIPLIER;

            //because height2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new InvalidOperationException($"{nameof(result)} is infinite.");
            if (double.IsNaN(result)) throw new InvalidOperationException($"{nameof(result)} is NaN.");

            //we know result is not null, not infinity and not nan, we can safely return it
            return result;
        }
    }
}