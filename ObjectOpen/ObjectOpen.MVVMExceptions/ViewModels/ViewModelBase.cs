using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ObjectOpen.MVVMExceptions.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase() {; }

        public string DisplayName { get; set; } = "";
        public bool ThrowOnInvalidPropertyName { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public ICommand EnsureCommand(RelayCommand command, Action<object?> action)
        {
            if (command == null) command = new RelayCommand(action);
            return command;
        }
    }

    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object?> execute) : this(execute, null) { }

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        readonly Action<object?> _execute;
        readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
