namespace ObjectOpen.WPFApp
{
    public static class Model
    {
        public static List<Employee>? GetCurrentEmployees() =>
            new List<Employee> {
                new Employee(
                    "Mateusz Zwierzycki", "Founder, teacher, dad jokes expert, clean code fanatic", double.PositiveInfinity),
                new Employee(
                    "Tomasz Paleski", "Jack of all trades, generalist, passionate about tests, can tolerate people", 3),
                new Employee(
                    "Jakub Oszczyk", "An academic tutor, looks great on the dance floor, loves mathematics, is scared of tests", 3),
                new Employee(
                    "Agnieszka Nowacka", "Designed a boat at the Expo in Dubai, enthusiastic traveller, strong coder", 1),
                new Employee(
                    "Aleksander D'Artan'arek", "The fearless Witcher, most of all enjoys trimming and welding", 3),
                new Employee(
                    "Kiryl Furmanchuk", "Failed at university, is scared of milk, loves coding though", 3)
            };
    }
}
