using Recipe_Pi.Controllers;
using Recipe_Pi.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Recipe_Pi.Views
{
    /// <summary>
    /// Interaction logic for Purchases.xaml
    /// </summary>
    public partial class Purchases : Window, INotifyPropertyChanged
    {
        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        #endregion

        FridgeController fridgeController = new FridgeController();
        private List<ProductModel> purchase;
        public List<ProductModel> Purchase
        {
            get { return purchase; }
            set 
            { 
                purchase = value;
                
                NotifyPropertyChanged(); 
            }
        }

        public Purchases()
        {
            InitializeComponent();
            this.DataContext = this;
            Purchase = fridgeController.GetAllPurchases();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            RecipeViewer recipeViewer = new RecipeViewer();
            recipeViewer.Show();
            Close();
        }
    }
}
