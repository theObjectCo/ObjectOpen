namespace ObjectOpen.WPFApp
{
    public class Employee
    {
        public Employee(string name, string about, double yearsAtObject)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(about))
                throw new ArgumentNullException(nameof(about));
            if (yearsAtObject < 0)
                throw new ArgumentException($"{nameof(yearsAtObject)} can't be less than 0");

            Name = name;
            About = about;
            YearsAtObject = yearsAtObject;
        }

        public string Name { get; private set; }
        public string About { get; private set; }
        public double YearsAtObject { get; private set; }
    }
}