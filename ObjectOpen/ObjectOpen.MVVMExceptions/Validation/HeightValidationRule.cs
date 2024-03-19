using System.Globalization;
using System.Windows.Controls;
using ObjectOpen.MVVMExceptions.Localization;

namespace ObjectOpen.MVVMExceptions.Validation
{
    public class HeightValidationRule : ValidationRule
    {
        public Units Units { get; set; }
        public HeightValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double height = 0;

            try
            {
                if (((string)value).Length > 0)
                    height = double.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            var result = HeightValidator.Validate(height, Units);
            if (result.Flag != Patterns.Solvers.Flag.OK) return new ValidationResult(false, result.Message);

            return ValidationResult.ValidResult;
        }
    }
}
