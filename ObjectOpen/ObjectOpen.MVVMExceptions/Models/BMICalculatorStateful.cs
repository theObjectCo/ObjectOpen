using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.Models
{
    public class BMICalculatorStateful
    {
        private const double CM_TO_M_MULTIPLIER = 0.01;
        private const double IMPERIAL_MULTIPLIER = 703;

        private Units _units;
        private double _weight;
        private double _height;

        private double _bmi;
        public double Bmi { get => _bmi; set => _bmi = value; }

        public BMICalculatorStateful(Units units, double weight, double height)
        {
            //constructor changes the class state, hence the need for validation
            if (units == Units.None) throw new Exception($"Invalid {nameof(units)}: {Units.None}");

            //we use external class for validation, it's logic could be as well a part of this class
            if (!(Validation.HeightValidator.Validate(height, units).Flag == Flag.OK)) throw new Exception($"Invalid {nameof(height)}");
            if (!(Validation.WeightValidator.Validate(weight, units).Flag == Flag.OK)) throw new Exception($"Invalid {nameof(weight)}");

            //we can now safely change the object state
            _units = units;
            _weight = weight;
            _height = height;
        }

        public void Calculate()
        {
            double bmi;

            if (_units == Units.Metric)
                bmi = ComputeMetricBMI(_weight, _height);
            else //its imperial
                bmi = ComputeImperialBMI(_weight, _height);

            if (double.IsInfinity(bmi)) throw new ArgumentException($"{nameof(bmi)} is infinite.");
            if (double.IsNaN(bmi)) throw new ArgumentException($"{nameof(bmi)} is NaN.");

            //can now safely change the state
            Bmi = bmi; 
        }

        private double ComputeMetricBMI(double weight, double height)
        {
            height *= CM_TO_M_MULTIPLIER;
            double height2 = height * height;
            double result = weight / height2; //this will throw if height2 is 0 (and it's 0 if height == 0)
            return result;
        }

        private double ComputeImperialBMI(double weight, double height)
        {
            double height2 = height * height;
            double result = weight / height2 * IMPERIAL_MULTIPLIER;
            return result;
        }
    }
}