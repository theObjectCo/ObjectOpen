using ObjectOpen.MVVMExceptions.Localization;
using ObjectOpen.Patterns.Solvers;

namespace ObjectOpen.MVVMExceptions.Validation
{
    public class WeightValidator
    {
        //Typical newborn is 3kg, we assume 1kg for some wiggle room 
        //The heaviest person ever was https://en.wikipedia.org/wiki/Jon_Brower_Minnoch, 635 kg - will round up to 1000 kg

        private const double TO_KG = 0.45359237;
        private const double MIN_WEIGHT_KG = 3;
        private const double MAX_WEIGHT_KG = 1000;

        public static Result Validate(double weight, Units unit)
        {
            if (unit == Units.Imperial)
            {
                if ((weight * TO_KG) < MIN_WEIGHT_KG) return new Result(Flag.Error, $"Minimal weight is {MIN_WEIGHT_KG / TO_KG} pounds. Got {weight} pounds");
                if ((weight * TO_KG) > MAX_WEIGHT_KG) return new Result(Flag.Error, $"Maximal weight is {MAX_WEIGHT_KG / TO_KG}. Got {weight} pounds");
            }
            else if (unit == Units.Metric)
            {
                if (weight < MIN_WEIGHT_KG) return new Result(Flag.Error, $"Minimal weight is {MIN_WEIGHT_KG} kilograms. Got {weight} kg");
                if (weight > MAX_WEIGHT_KG) return new Result(Flag.Error, $"Maximal weight is {MAX_WEIGHT_KG} kilograms. Got {weight} kg");
            }

            return new Result();
        }
    }
}