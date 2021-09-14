using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Recipe_Pi.Controllers;
using Recipe_Pi.Models;

namespace Recipe_Pi.Views.Navigation
{
    /// <summary>
    /// Interaction logic for Favorites.xaml
    /// </summary>
    public partial class Favorites : UserControl
    {
        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        #endregion

        private List<FavoriteModel> showFavoriteRecipe;

        FavoriteController favoriteController = new FavoriteController();

        public List<FavoriteModel> ShowFavoriteRecipe
        {
            get => showFavoriteRecipe;
            set
            {
                showFavoriteRecipe = value;
                NotifyPropertyChanged();
            }
        }
        public Favorites()
        {
            InitializeComponent();
            this.DataContext = this;
            ShowFavoriteRecipe = favoriteController.GetAllFavorites(App.UserModel.UserId.ToString());
        }
    }
}
