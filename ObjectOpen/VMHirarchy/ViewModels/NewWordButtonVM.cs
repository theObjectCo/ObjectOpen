using Architect.Builder.ViewModels.Base;
using System.Windows.Input;

namespace VMHirarchy.ViewModels
{
    public class NewWordButtonVM : ViewModelBase
    {
        private RelayCommand _fetchNewWord;

        public NewWordButtonVM()
        {
            _fetchNewWord = new RelayCommand(FetchNewWordMethod);
        }

        public MainVM GrandParent { get; set; }
        public WordsTableVM Parent { get; set; }

        public ICommand FetchNewWord =>
            _fetchNewWord;

        private void FetchNewWordMethod(object obj)
        {
            if (Parent.TryGetNextName(out string nextName))
                GrandParent.AddAName(nextName);
            else
                GrandParent.DeclareEndOfNames();
        }
    }
}
