using Architect.Builder.ViewModels.Base;
using Rhino.PlugIns;
using System.Collections.ObjectModel;
using System.Windows;

namespace VMHirarchy.ViewModels
{
    public class MainVM : ViewModelBase
    {
        public MainVM()
        {
            PluginsInfos = new ObservableCollection<PlugInInfo>();
        }

        public ObservableCollection<PlugInInfo> PluginsInfos { get; private set; }

        public void AddData(PlugInInfo nextInfo) =>
            PluginsInfos.Add(nextInfo);

        public void DeclareEndOfNames() =>
            MessageBox.Show("We're out of loaded plugins");
    }
}
