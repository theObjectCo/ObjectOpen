using System.Collections.ObjectModel;

namespace ObjectOpen.WPFApp
{
    public class ViewModel : ViewModelBase
    {
        public ObservableCollection<Employee> Employees { get; private set; }


    }
}
