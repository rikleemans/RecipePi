using Recipe_Pi.ViewModels;
using System;
using System.Windows.Input;

namespace Recipe_Pi.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private readonly MainViewModel _viewModel;
        public event EventHandler CanExecuteChanged;


        public UpdateViewCommand(MainViewModel viewModel)
        {
            this._viewModel = viewModel;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {           
            if (parameter.ToString() == "Fridge")
            {
                _viewModel.SelectedViewModel = new FridgeViewModel();
            }
            else if (parameter.ToString() == "Recipes")
            {
                _viewModel.SelectedViewModel = new RecipesViewModel();
            }
            else if (parameter.ToString() == "Favorites")
            {
                _viewModel.SelectedViewModel = new FavoritesViewModel();
            }
        }
    }
}
