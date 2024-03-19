using System.Globalization;
using System.Windows.Controls;
using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.Validation
{
    public class WeightValidationRule : ValidationRule
    {
        public Units Units { get; set; }
        public WeightValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double weight = 0;

            try
            {
                if (((string)value).Length > 0)
                    weight = double.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            var result = WeightValidator.Validate(weight, Units);
            if (result.Flag != Patterns.Solvers.Flag.OK) return new ValidationResult(false, result.Message);

            return ValidationResult.ValidResult;
        }
    }
}
