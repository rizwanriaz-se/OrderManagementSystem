using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManagementSystem.Commands
{
    public class RelayCommand : ICommand
    {
        public Action<object> _Execute { get; set; }
        public Predicate<object> _CanExecute { get; set; }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _CanExecute(parameter);
            //return _CanExecute == null ? true : _CanExecute(parameter);
        }

        public void Execute(object parameter) {
            _Execute(parameter);
        }
    }
}
