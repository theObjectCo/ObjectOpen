using Architect.Builder.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace VMHirarchy.ViewModels
{
    public class MainVM : ViewModelBase
    {
        public MainVM()
        {
            PluginsNames = new ObservableCollection<string>();
        }

        public ObservableCollection<string> PluginsNames { get; private set; }

        public void AddAName(string nextName)
        {
            PluginsNames.Add(nextName);
            base.OnPropertyChanged(nameof(PluginsNames));
        }

        internal void DeclareEndOfNames()
        {
            MessageBox.Show("We're out of loaded plugins");
        }
    }
}
