using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.Commands
{
    public class RelayCommand : ICommand
    {
        public Action<object> _Execute { get; set; }

        public RelayCommand(Action<object> execute)
        {
            _Execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
    }
   
}
