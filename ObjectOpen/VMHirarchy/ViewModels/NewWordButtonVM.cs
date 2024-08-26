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

        public MainVM Parent { get; set; }
        public WordsTableVM Sibling { get; set; }

        public ICommand FetchNewWord =>
            _fetchNewWord;

        private void FetchNewWordMethod(object obj)
        {
            if (Sibling.TryGetNextName(out string nextName))
                Parent.AddAName(nextName);
            else
                Parent.DeclareEndOfNames();
        }
    }
}
