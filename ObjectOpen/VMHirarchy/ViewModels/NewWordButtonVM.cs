using Architect.Builder.ViewModels.Base;
using Rhino.PlugIns;
using System.Windows.Input;

namespace VMHirarchy.ViewModels
{
    public class NewWordButtonVM : ViewModelBase
    {
        private RelayCommand _fetchNewInfo;

        public NewWordButtonVM()
        {
            _fetchNewInfo = new RelayCommand(FetchNewWordMethod);
        }

        public MainVM GrandParent { get; set; }
        public WordsTableVM Parent { get; set; }

        public ICommand FetchNewInfo =>
            _fetchNewInfo;

        private void FetchNewWordMethod(object obj)
        {
            if (Parent.TryGetNextPluginInfo(out PlugInInfo nextInfo))
                GrandParent.AddData(nextInfo);
            else
                GrandParent.DeclareEndOfNames();
        }
    }
}
