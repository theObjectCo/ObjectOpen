using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.Validation
{
    public class HeightValidator
    {
        //For the sake of argument we say 1' is the minimal allowed height
        //The maximal height should a bit more than 8'11"
        //https://www.aap.org/en/patient-care/newborn-and-infant-nutrition/newborn-and-infant-nutrition-assessment-tools/bmi-for-age-newborn-and-infant-assessment/
        //https://en.wikipedia.org/wiki/List_of_tallest_people

        private const double FROM_INCH = 2.54;
        private const double MIN_HEIGHT_IN = 12;
        private const double MAX_HEIGHT_IN = 9 * 12;

        public static Result Validate(double height, Units unit)
        {
            if (unit == Units.Imperial)
            {
                if (height < MIN_HEIGHT_IN) return new Result(Flag.Error, $"Minimal height is {MIN_HEIGHT_IN} inches. Got {height} in");
                if (height > MAX_HEIGHT_IN) return new Result(Flag.Error, $"Maximal height is {MAX_HEIGHT_IN} inches. Got {height} in");
            }
            else if (unit == Units.Metric)
            {
                if (height < (MIN_HEIGHT_IN * FROM_INCH)) return new Result(Flag.Error, $"Minimal height is {MIN_HEIGHT_IN * FROM_INCH} centimeters. Got {height} cm");
                if (height > (MAX_HEIGHT_IN * FROM_INCH)) return new Result(Flag.Error, $"Maximal height is {MAX_HEIGHT_IN * FROM_INCH} centimeters. Got {height} cm");
            }

            return new Result();
        }
    }
}