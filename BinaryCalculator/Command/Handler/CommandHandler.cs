using BinaryCalculator.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryCalculator.Command.Handler
{
    public class CommandHandler : ICommand
    {
        private Action<Object> _action;
        private Func<Object, bool> _canExecute;

        public CommandHandler(Action<Object> action, Func<Object, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(Object parameter)
        {
            String s = parameter.ToString();

            return StringConstant.CE.Equals(s) || StringConstant.C.Equals(s) || _canExecute(parameter);
        }

        public void Execute(Object parameter)
        {
            _action(parameter);
        }
    }
}
