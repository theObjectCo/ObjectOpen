using System.Collections.ObjectModel;

namespace ObjectOpen.WPFApp
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            List<Employee>? existingEmployees = Model.GetCurrentEmployees();
            Employees = existingEmployees == null || existingEmployees.Count == 0
                ? new ObservableCollection<Employee>()
                : new ObservableCollection<Employee>(existingEmployees);
        }

        public ObservableCollection<Employee> Employees { get; private set; } = new ObservableCollection<Employee>();
    }
}
