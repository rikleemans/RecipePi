using Recipe_Pi.Controllers;
using Recipe_Pi.Models;
using System;
using System.Windows;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for NewProductManual.xaml
    /// </summary>
    public partial class NewProductManual : Window
    {
        ProductController productController = new ProductController();
        public NewProductManual()
        {
            InitializeComponent();
        }

        /// <author>Siem</author>
        /// <summary>
        /// Depending on if the ProductDate field is filled it Calls NewProductManual with 2/3 paramters
        /// Shows confirmation message with object productModel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string message;
            bool result;
            (result, message) = productController.CheckNewProductFieldsFilled(tbProductName.Text, tbProductCount.Text);
            if (!result)
            {
                if (productController.CheckNewProductExpireDateFilled(dpProductDate.Text))
                {
                    ProductModel productModel = new ProductModel { ProductName = tbProductName.Text, ProductCount = int.Parse(tbProductCount.Text) };
                    var Result = MessageBox.Show("Weet u zeker dat u " + productModel.ToString() + " wilt toevoegen?", "Product toevoegen (zonder verloopdatum)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {
                        productController.NewProductManual(tbProductName.Text, int.Parse(tbProductCount.Text));
                    }
                    else if (Result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    ProductModel productModel = new ProductModel { ProductName = tbProductName.Text, ProductCount = int.Parse(tbProductCount.Text), ExpireDate = dpProductDate.Text };
                    var Result = MessageBox.Show("Weet u zeker dat u " + productModel.ToString() + " wilt toevoegen?", "Product toevoegen met verloopdatum", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {
                        productController.NewProductManual(tbProductName.Text, int.Parse(tbProductCount.Text), dpProductDate.Text);
                    }
                    else if (Result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(message);

            }



        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
