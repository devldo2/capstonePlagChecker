using System;
using System.Windows.Input;

namespace AntiPlagiatus.Helpers
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action action;

        private Command() { }
        public Command(Action _action)
        {
            this.action = _action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
        }
    }
    public class Command<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<T> action;

        private Command() { }
        public Command(Action<T> _action)
        {
            this.action = _action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
                action?.Invoke((T)parameter);
        }
    }
}
