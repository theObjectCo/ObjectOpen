using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.Models
{
    public static class BMICalculatorStateless
    {
        public const double CM_TO_M_MULTIPLIER = 0.01;
        public const double IMPERIAL_MULTIPLIER = 703;
        public const double MIN_BMI = 0;
        public const double MAX_BMI = 1000;

        public static double Calculate(Units units, double weight, double height)
        {
            double bmi;

            if (units == Units.Metric)
                bmi = ComputeMetricBMI(weight, height);
            else //its imperial
                bmi = ComputeImperialBMI(weight, height);

            //can now safely return 
            return bmi;
        }

        public static double ComputeMetricBMI(double weight, double height)
        {
            height *= CM_TO_M_MULTIPLIER;
            double height2 = height * height;
            double result = weight / height2; //this will throw if height2 is 0 (and it's 0 if height == 0)

            //because height2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new InvalidOperationException($"{nameof(result)} is infinite.");
            if (double.IsNaN(result)) throw new InvalidOperationException($"{nameof(result)} is NaN.");
            if (result < MIN_BMI) throw new InvalidOperationException($"{nameof(result)} is smaller than {nameof(MIN_BMI)}");
            if (result > MAX_BMI) throw new InvalidOperationException($"{nameof(result)} is smaller than {nameof(MAX_BMI)}");

            //we know result is not null, not infinity and not nan, we can safely return it
            return result;
        }

        public static double ComputeImperialBMI(double weight, double height)
        {
            double height2 = height * height;
            double result = weight / height2 * IMPERIAL_MULTIPLIER;

            //because height2 can be very small, we need to check for infinity in result 
            if (double.IsInfinity(result)) throw new InvalidOperationException($"{nameof(result)} is infinite.");
            if (double.IsNaN(result)) throw new InvalidOperationException($"{nameof(result)} is NaN.");
            if (result < MIN_BMI) throw new InvalidOperationException($"{nameof(result)} is smaller than {nameof(MIN_BMI)}");
            if (result > MAX_BMI) throw new InvalidOperationException($"{nameof(result)} is smaller than {nameof(MAX_BMI)}");

            //we know result is not null, not infinity and not nan, we can safely return it
            return result;
        }
    }
}