using System.ComponentModel;
using System.Windows;

namespace CustomControlCommandSample
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DelegateCommand messageCommand;
        private bool canExecuteMessageCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public DelegateCommand MessageCommand
        {
            get
            {
                if (messageCommand == null)
                    messageCommand = new DelegateCommand(OnExecuteMessageCommand, OnCanExecuteMessageCommand);
                return messageCommand;
            }
        }

        public bool CanExecuteMessageCommand
        {
            get { return canExecuteMessageCommand; }
            set
            {
                if (value != canExecuteMessageCommand)
                {
                    canExecuteMessageCommand = value;
                    OnPropertyChanged(nameof(canExecuteMessageCommand));
                    messageCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool OnCanExecuteMessageCommand(object obj)
        {
            return canExecuteMessageCommand;
        }

        private void OnExecuteMessageCommand(object parameter)
        {
            MessageBox.Show((string)parameter, "ViewModel Command");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
