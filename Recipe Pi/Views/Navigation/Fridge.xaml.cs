using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Recipe_Pi.Controllers;
using Recipe_Pi.Models;

namespace Recipe_Pi.Views.Navigation
{
    /// <summary>
    /// Interaction logic for Fridge.xaml
    /// </summary>
    public partial class Fridge : UserControl
    {
        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        #endregion

        ProductController productController = new ProductController();
        private List<ProductModel> showFridgeProduct;
        public List<ProductModel> ShowFridgeProduct
        {
            get => showFridgeProduct;
            set
            {
                showFridgeProduct = value;
                NotifyPropertyChanged();
            }
        }
        
        public Fridge()
        {
            InitializeComponent();
            this.DataContext = this;
            ShowFridgeProduct = productController.GetAllProducts().Where(rp => rp.ProductCount > 0).ToList();
        }

        /// <author>Rik</author>
        /// <summary>
        /// Takes the selected item
        /// Puts the productid in a string, which it retrieves from Product model
        /// Executes the softdelete function
        /// Retrieves all products again so that you see the change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgProduct.SelectedItem is not null)
            {
                ProductModel productModel = (ProductModel)dgProduct.SelectedItem;
                string ProductID = productModel.ProductId.ToString();
                string Expiredate = productModel.ExpireDate.ToString();
                productController.SoftDelete(ProductID, Expiredate);
                dgProduct.ItemsSource = productController.GetAllProducts();
            }     
        }

        /// <author>Siem</author>
        /// <summary>
        /// Opens new window NewProductManual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            NewProductManual newProductManual = new NewProductManual();
            newProductManual.ShowDialog();
            dgProduct.ItemsSource = productController.GetAllProducts();
        }    
    }
}
