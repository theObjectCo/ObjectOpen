using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.Models
{
    public static class BMICalculatorStateless
    {
        private const double CM_TO_M_MULTIPLIER = 0.01;
        private const double IMPERIAL_MULTIPLIER = 703;

        public static double Calculate(Units units, double weight, double height)
        {
            double bmi;

            if (units == Units.Metric)
                bmi = ComputeMetricBMI(weight, height);
            else //its imperial
                bmi = ComputeImperialBMI(weight, height);

            if (double.IsInfinity(bmi)) throw new ArgumentException($"{nameof(bmi)} is infinite.");
            if (double.IsNaN(bmi)) throw new ArgumentException($"{nameof(bmi)} is NaN.");

            //can now safely return 
            return bmi;
        }

        private static double ComputeMetricBMI(double weight, double height)
        {
            height *= CM_TO_M_MULTIPLIER;
            double height2 = height * height;
            double result = weight / height2; //this will throw if height2 is 0 (and it's 0 if height == 0)
            return result;
        }

        private static double ComputeImperialBMI(double weight, double height)
        {
            double height2 = height * height;
            double result = weight / height2 * IMPERIAL_MULTIPLIER;
            return result;
        }
    }
}