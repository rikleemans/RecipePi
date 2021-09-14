using Recipe_Pi.Commands;
using System.Windows.Input;

namespace Recipe_Pi.ViewModels
{
    public class MainViewModel : BaseViewModel //Inherits from base viewmodel to use Notifypropertychanged
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            {
                _selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; } 

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
