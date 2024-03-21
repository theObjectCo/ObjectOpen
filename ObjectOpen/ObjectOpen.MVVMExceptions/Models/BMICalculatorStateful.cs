using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.Models
{
    public class BMICalculatorStateful
    {
        private Units _units;
        private double _weight;
        private double _height;

        private double _bmi;

        public BMICalculatorStateful(Units units, double weight, double height)
        {
            Units = units;
            Weight = weight;
            Height = height;
        }

        public double BMI
        {
            get => _bmi;
            set
            {
                if (double.IsInfinity(value)) throw new ArgumentException($"{nameof(value)} is infinite.");
                if (double.IsNaN(value)) throw new ArgumentException($"{nameof(value)} is NaN.");
                if (value < BMICalculatorStateless.MIN_BMI) throw new ArgumentException($"{nameof(value)} is smaller than {nameof(BMICalculatorStateless.MIN_BMI)}");
                if (value > BMICalculatorStateless.MAX_BMI) throw new ArgumentException($"{nameof(value)} is smaller than {nameof(BMICalculatorStateless.MAX_BMI)}");

                _bmi = value;
            }
        }

        public Units Units
        {
            get => _units;
            private set
            {
                if (value == Units.None) throw new ArgumentException($"Invalid {nameof(value)}: {Units.None}");
                _units = value;
            }
        }
        public double Weight
        {
            get => _weight;
            private set
            {
                if (!(Validation.HeightValidator.Validate(value, Units).Flag == Flag.OK)) throw new ArgumentException($"Invalid {nameof(value)}");
                _weight = value;
            }
        }
        public double Height
        {
            get => _height;
            private set
            {
                if (!(Validation.WeightValidator.Validate(value, Units).Flag == Flag.OK)) throw new ArgumentException($"Invalid {nameof(value)}");
                _height = value;
            }
        }

        public void Calculate()
        {
            double bmi;

            if (Units == Units.Metric)
                bmi = BMICalculatorStateless.ComputeMetricBMI(Weight, Height);
            else //its imperial
                bmi = BMICalculatorStateless.ComputeImperialBMI(Weight, Height);

            //setter will throw if invalid
            BMI = bmi;
        }
    }
}