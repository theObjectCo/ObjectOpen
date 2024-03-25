using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.Validation
{
    public class BMIValidator
    {
        private const double MIN_BMI = 0;
        private const double MAX_BMI = 1000;

        public static Result Validate(double bmi)
        {
            if (bmi < MIN_BMI) return new Result(Flag.Error, $"Minimal BMI is {MIN_BMI} pounds. Got {bmi}");
            if (bmi > MAX_BMI) return new Result(Flag.Error, $"Maximal BMI is {MAX_BMI}. Got {bmi}");

            return new Result();
        }
    }
}