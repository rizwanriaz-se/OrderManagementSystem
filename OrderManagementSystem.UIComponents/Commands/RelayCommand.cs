﻿using System.Windows.Input;

namespace OrderManagementSystem.UIComponents.Commands
{
    public class RelayCommand : ICommand
    {
        public Action<object> _Execute { get; set; }
        //public Predicate<object> _CanExecute { get; set; }

        //public void RaiseCanExecuteEventChanged()
        //{
        //    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        //}

        public RelayCommand(Action<object> execute)
        {
            _Execute = execute;
            //_CanExecute = canExecute;
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
    //public class RelayCommand : ICommand
    //{
    //    public Action<object> _Execute { get; set; }
    //    public Predicate<object> _CanExecute { get; set; }

    //    public void RaiseCanExecuteEventChanged()
    //    {
    //        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    //    }

    //    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    //    {
    //        _Execute = execute;
    //        _CanExecute = canExecute;
    //    }

    //    public event EventHandler? CanExecuteChanged;

    //    public bool CanExecute(object parameter)
    //    {
    //        return _CanExecute(parameter);
    //    }

    //    public void Execute(object parameter)
    //    {
    //        _Execute(parameter);
    //    }
    //}
}
