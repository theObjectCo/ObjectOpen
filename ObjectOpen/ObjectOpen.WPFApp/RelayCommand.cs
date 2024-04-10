using System.Diagnostics;
using System.Windows.Input;

namespace ObjectOpen.WPFApp
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object>? execute) : this(execute, null) { }

        public RelayCommand(Action<object>? execute, Predicate<object>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        [DebuggerStepThrough]
        public bool CanExecute(object? parameter) =>
            _canExecute == null || _canExecute(parameter
                ?? throw new ArgumentNullException(nameof(parameter)));


        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter) =>
            _execute(parameter
                ?? throw new ArgumentNullException(nameof(parameter)));
    }
}
