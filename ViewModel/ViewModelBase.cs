using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyWatermark.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        { 
            OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Action<object> executeMethod = null;
        private readonly Func<object, bool> canExecuteMethod = null;
        private Action<object> action;
        private bool p;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public DelegateCommand(Action<object> action, bool p)
        {
            // TODO: Complete member initialization
            this.action = action;
            this.p = p;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod == null) return true;
            return this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            if (executeMethod == null) return;
            this.executeMethod(parameter);
        }
    }
}
