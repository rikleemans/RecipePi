using Recipe_Pi.Controllers;
using System.Windows;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for NewProduct.xaml
	/// New product when scanned with barcode.
    /// </summary>
    public partial class NewProduct : Window
	{
		public string barcode;
		//public NewProduct()
		public NewProduct(string barcode)
		{
			InitializeComponent();
			this.barcode = barcode;
		}
		public NewProduct()
        {
			MessageBox.Show("Iets ging fout");
        }

		private void btnSend_Click(object sender, RoutedEventArgs e)
		{
			var newProduct = new ProductController();
			newProduct.NewProduct(barcode, tbProduct.Text.ToString());
		}
	}
}
