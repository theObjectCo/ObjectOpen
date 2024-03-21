using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.ChickenMode
{
    public class BMICalculator
    {
        private Units _units;
        public BMICalculator(Units units)
        {
            Units = units;
        }

        public Units Units
        {
            get => _units;
            set
            {
                if (value == Units.None) throw new ArgumentException($"Invalid {nameof(value)}: {Units.None}");
                _units = value;
            }
        }

        public double Calculate(double weight, double height)
        {
            if (Units == Units.Metric)
                return BMICalculatorStateless.ComputeMetricBMI(weight, height);
            else //its imperial
                return BMICalculatorStateless.ComputeImperialBMI(weight, height);
        }
    }
}