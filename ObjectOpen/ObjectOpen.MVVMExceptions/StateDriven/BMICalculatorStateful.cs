﻿using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.StateDriven
{
    public class BMICalculatorStateful
    {
        private Units _units;
        private double _weight;
        private double _height;

        private double _bmi;

        public BMICalculatorStateful(Units units, double weight, double height)
        {
            //property setters perform input validation
            Units = units;
            Weight = weight;
            Height = height;
        }

        public Units Units
        {
            get => _units;
            set
            {
                if (!(value == Units.Imperial || value == Units.Metric)) throw new ArgumentException($"Invalid {nameof(value)}: {value}");
                _units = value;
            }
        }
        public double Weight
        {
            get => _weight;
            set
            {
                if (!(Validation.HeightValidator.Validate(value, Units).Flag == Flag.OK)) throw new ArgumentException($"Invalid {nameof(Weight)}");
                _weight = value;
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                if (!(Validation.WeightValidator.Validate(value, Units).Flag == Flag.OK)) throw new ArgumentException($"Invalid {nameof(Height)}");
                _height = value;
            }
        }

        public double BMI
        {
            get => _bmi;
            private set
            {
                if (!(Validation.BMIValidator.Validate(value).Flag == Flag.OK)) throw new ArgumentException($"Invalid {nameof(BMI)}");
                _bmi = value;
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