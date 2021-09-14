using Recipe_Pi.Controllers;
using Recipe_Pi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
namespace Recipe_Pi.Views.Navigation
{
    /// <summary>
    /// Interaction logic for Recipes.xaml
    /// </summary>
    public partial class Recipes : UserControl, INotifyPropertyChanged
    {
        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        #endregion

        private RecipeModel selectedRecipe;
        private List<ProductModel> showProduct;
        private List<RecipeProductModel> showFridgeProduct;
        private List<RecipeModel> recipeModels;
        private readonly RecipeProductController _recipeProductController = new RecipeProductController();
        private readonly RecipeController _recipeController = new RecipeController();
        private readonly FavoriteController _favoriteController = new FavoriteController();

        public List<RecipeModel> RecipeModels
        {
            get { return recipeModels; }
            set { recipeModels = value; NotifyPropertyChanged(); }
        }
        public List<ProductModel> ShowProducts
        {
            get => showProduct;
            set
            {
                showProduct = value;
                NotifyPropertyChanged();
            }
        }
        public RecipeModel SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                ShowFridgeProduct = _recipeProductController.GetRecipeProduct(selectedRecipe.RecipeId).Where(rp => rp.ProductCount > 0).ToList();                
                NotifyPropertyChanged();
            }
        }
        public List<RecipeProductModel> ShowFridgeProduct
        {
            get => showFridgeProduct;
            set
            {
                showFridgeProduct = value;
                NotifyPropertyChanged();
            }
        }
        public Recipes()
        {
            InitializeComponent();
            this.DataContext = this;
            RecipeModels = _recipeController.GetAllRecipes();
        }

        /// <author>Rik</author>
        /// <summary>
        /// Takes the selected item.
        /// creates a new favorite controller and makes sure it executes the functions with the correct parameters.
        /// then it shows the added recipe in the list
        private void btnNewFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (dgRecipe.SelectedItem is not null)
            {
                RecipeModel recipeModel = (RecipeModel)dgRecipe.SelectedItem;
                _favoriteController.InsertFav(recipeModel.RecipeId, App.UserModel.UserId);
            }
           
        }
    }
}
