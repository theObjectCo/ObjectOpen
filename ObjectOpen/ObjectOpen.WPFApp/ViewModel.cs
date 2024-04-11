using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ObjectOpen.WPFApp
{
    public class ViewModel : ViewModelBase
    {
        private string _newEmployeeName = string.Empty;
        private string _newEmployeeAbout = string.Empty;

        public ViewModel()
        {
            try
            {
                List<Employee> existingEmployees = Model.GetExistingEmployees();

                Employees = existingEmployees == null || existingEmployees.Count == 0
                    ? new ObservableCollection<Employee>()
                    : new ObservableCollection<Employee>(existingEmployees);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Well, shit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Employee> Employees { get; private set; } = new ObservableCollection<Employee>();

        public string NewEmployeeName
        {
            get => _newEmployeeName;
            set
            {
                _newEmployeeName = value;
                base.OnPropertyChanged();
            }
        }

        public string NewEmployeeAbout
        {
            get => _newEmployeeAbout;
            set
            {
                _newEmployeeAbout = value;
                base.OnPropertyChanged();
            }
        }

        private RelayCommand? _addNewEmployeeCommand;
        public ICommand AddNewEmployeeCommand => _addNewEmployeeCommand ?? new RelayCommand(AddNewEmployeeCommandMethod);

        private void AddNewEmployeeCommandMethod(object? obj)
        {
            Employees.Add(new Employee(_newEmployeeName, _newEmployeeAbout, 0));

            NewEmployeeName = string.Empty;
            NewEmployeeAbout = string.Empty;
        }
    }
}
