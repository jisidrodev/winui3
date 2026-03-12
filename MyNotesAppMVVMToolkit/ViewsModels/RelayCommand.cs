using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyNotesApp.ViewsModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool>? _canExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action? action) : this(action,null)
        {          
        }
        public RelayCommand(Action? action, Func<bool>? canExecute)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            this._action = action;
            this._canExecute = canExecute;
        }
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _action();
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this,EventArgs.Empty);
    }
}
