using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.StateDriven
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

            return bmi;
        }

        public static double ComputeMetricBMI(double weight, double height)
        {
            height *= CM_TO_M_MULTIPLIER;
            double height2 = height * height;
            double result = weight / height2;
            return result;
        }

        public static double ComputeImperialBMI(double weight, double height)
        {
            double height2 = height * height;
            double result = weight / height2 * IMPERIAL_MULTIPLIER;
            return result;
        }
    }
}