using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;

namespace Recipe_Pi.Controllers
{
	public class ProductController
	{

		DBRecipePi conn = new DBRecipePi();
		FridgeController fridgeController = new FridgeController();
		SerialPort mySerialPort = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);

		//public static void MDKain()
		//{
		//	var anInstanceofMyClass = new ProductController();
		//	anInstanceofMyClass.handler();
		//}


		/// <author>Rik</author>
		/// <summary>
		/// Softdeletes the product with given productID
		/// </summary>
		/// <param name="productid"></param>
		/// <param name="expiredate"></param>
		public void SoftDelete(string productid, string expiredate)
		{

			conn.OpenConnection();
			var command = conn.connection.CreateCommand();
			command.CommandText = @"
			UPDATE fridgeproduct
			SET ProductCount=0, SoftDelete=NOW()
			WHERE FPProductID = @productid AND Expiredate = @expiredate";
			command.Parameters.AddWithValue("@productid", productid);
			command.Parameters.AddWithValue("@expiredate", expiredate);
			command.ExecuteNonQuery();
			conn.CloseConnection();
			return;
		}

		/// <author>Siem</author>
		/// <summary>
		/// Checks if product name and product count are filled.
		/// </summary>
		/// <param name="productname"></param>
		/// <param name="productcount"></param>
		/// <returns></returns>
		public (bool, string) CheckNewProductFieldsFilled(string productname, string productcount)
		{
			string message = "Niet alle verplichte velden zijn ingevuld";
			
			return (productname == string.Empty || productcount == string.Empty, message);
		}

		public bool CheckNewProductExpireDateFilled(string productdate)
        {
			return productdate == string.Empty;
		}

		/// <author>Siem</author>
		/// <summary>
		/// Inserts new product in the product table using string barcode.
		/// </summary>
		/// <param name="barcode"></param>
		/// <param name="productname"></param>
		/// <returns>boolean false or true</returns>
		public bool NewProduct(string barcode, string productname)
		{
			conn.OpenConnection();

			var command = conn.connection.CreateCommand();
			command.CommandText = "INSERT INTO `product` (`Productbarcode`, `Productname`) values (@barcode, @productname);";
			command.Parameters.AddWithValue("@barcode", barcode);
			command.Parameters.AddWithValue("@productname", productname);
			if (command.ExecuteNonQuery() > 0)
			{
				conn.CloseConnection();
				NewProduct newProduct = new NewProduct(barcode);
				newProduct.Close();
				GetProductByBarcode(barcode);
				return true;
			}
			else
			{
				MessageBox.Show("Something went wrong");
				conn.CloseConnection();
				return false;
			}
		}

		/// <author>Rik</author>
		/// <summary>
		/// returns all products information in a list
		/// </summary>
		/// <returns> List<ProductModel> list</returns>
		public List<ProductModel> GetAllProducts()
		{
			conn.OpenConnection();

			var command = conn.connection.CreateCommand();
			command.CommandText = @"
				SELECT fridgeproduct.ProductCount, fridgeproduct.Expiredate, product.Productname, product.Productbarcode, product.ProductID 
				FROM product 
					INNER JOIN fridgeproduct 
						ON product.ProductID = fridgeproduct.FPProductID
				ORDER BY product.ProductID";

			List<ProductModel> list = new List<ProductModel>();
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(GetProductModelFromReader(reader));
			}

			conn.CloseConnection();

			return list;
		}

		/// <author>Rik</author>
		/// <summary>
		///	Put ProductID, Productname, Productbarcode and ProductCount in The ProductModel
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private ProductModel GetProductModelFromReader(MySqlDataReader reader)
		{
			return new ProductModel
			{
				ProductId = reader.GetInt32("ProductID"),
				ProductName = reader.GetString("Productname"),
				ProductBarcode = reader.GetString("Productbarcode"),
				ProductCount = reader.GetInt32("ProductCount"),
				ExpireDate = reader.GetString("Expiredate")
		};
		}

		/// <author>Anas, Siem</author>
		/// <summary>
		///		Get productinformation using the barcode and handle it.
		/// </summary>
		/// <param name="barcode"></param>
		/// <returns>ProductModel model</returns>
		public ProductModel GetProductByBarcode(string barcode)
		{
			conn.OpenConnection();
			var command = conn.connection.CreateCommand();
			command.CommandText = "select * from product WHERE Productbarcode = @barcode";
			command.Parameters.AddWithValue("@barcode", barcode);
			ProductModel product = new ProductModel();
			MySqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				product = new ProductModel()
				{
					ProductId = (int)reader["ProductID"],
					ProductBarcode = (string)reader["Productbarcode"],
					ProductName = (string)reader["Productname"]
				};
				reader.Close();
				if (!fridgeController.ProductExistsInFridgeProduct(product.ProductId))
				{
					//Product doesn't exist, INSERT product in Fridgeproduct and update ProductCount to 1.				
					command.CommandText = "INSERT INTO fridgeproduct (FPFridgeID, FPProductID, ProductCount) VALUES(1, @FPProductID, 1)";
					command.Parameters.AddWithValue("@FPProductID", product.ProductId);
					command.ExecuteNonQuery();
				}
				else
				{
					//Product allready exists, UPDATE ProductCount + 1.
					command.CommandText = "UPDATE fridgeproduct SET ProductCount = ProductCount + 1 WHERE FPProductID = @FPProductID AND FPFridgeID = 1";
					command.Parameters.AddWithValue("@FPProductID", product.ProductId);
					command.ExecuteNonQuery();
				}
			}
			else
			{
				//Invoke from current thread to call UI thread.
				Application.Current.Dispatcher.Invoke(() =>
				{
					NewProduct newProduct = new NewProduct(barcode);
					newProduct.Show();
				});
				MessageBox.Show("unknown product");
			}
			reader.Close();
			conn.CloseConnection();

			return product;
		}

		/// <author>Siem</author>
		/// <summary>
		/// Checks if product allready exists
		/// </summary>
		/// <param name="productname"></param>
		/// <returns></returns>
		public bool ProductExists(string productname)
		{
			conn.OpenConnection();
			bool result = false;
			var command = conn.connection.CreateCommand();
			command.CommandText = "SELECT COUNT(*) FROM product Where productname = @productname";
			command.Parameters.AddWithValue("@productname", productname);
			var x = command.ExecuteScalar();
			int exist = Convert.ToInt32(x);
			if (exist > 0)
			{
				result = true;
			}
			return result;
		}

		/// <author>Siem</author>
		/// <summary>
		/// Calls NewProductManual with the 3rd parameter as an empty string because its overloaded
		/// </summary>
		/// <param name="productname"></param>
		/// <param name="productcount"></param>
		/// <param string=""></param>
		/// <returns></returns>
		public bool NewProductManual(string productname, int productcount)
        {
			// The 3rd parameter is empty since in the ProductModel there is a check if the string is empty.
			return NewProductManual(productname, productcount, "");
		}

		/// <author>Siem</author>
		/// <summary>
		/// Inserts new product into table product into database, 
		/// Calls UpdateProductInFridge with expiredate
		/// </summary>
		/// <param name="productname"></param>
		/// <param name="productcount"></param>
		/// <param name="expiredate"></param>
		/// <returns>boolean result</returns>
		public bool NewProductManual(string productname, int productcount, string expiredate)
		{
			if (!ProductExists(productname))
			{
				conn.OpenConnection();

				var command = conn.connection.CreateCommand();
				command.CommandText = "INSERT INTO `product` (`productname`) values (@productname);";
				command.Parameters.AddWithValue("@productname", productname);
				if (command.ExecuteNonQuery() > 0)
				{
					conn.CloseConnection();
					UpdateProductInFridge(productname, productcount, expiredate);
					return true;
				}
				else
				{
					MessageBox.Show("Something went wrong");
					conn.CloseConnection();
					return false;
				}
			}
			else
			{
				conn.CloseConnection();
				UpdateProductInFridge(productname, productcount, expiredate);
				return true;
			}


		}

		/// <author>Siem</author>
		/// <summary>
		/// Checks if Expiredate allready exists from the same product.
		/// </summary>
		/// <param name="expiredate"></param>
		/// <param name="productname"></param>
		/// <returns>boolean result</returns>
		public bool ExpiredateExists(string expiredate, string productname)
        {
            conn.OpenConnection();
            bool result = false;
            var command = conn.connection.CreateCommand();
            command.CommandText = @"
				SELECT COUNT(*) 
				FROM product 
					INNER JOIN fridgeproduct
						ON product.ProductID = fridgeproduct.FPProductID
				WHERE fridgeproduct.Expiredate = @expiredate
				AND product.productname = @productname";
            command.Parameters.AddWithValue("@expiredate", expiredate);
            command.Parameters.AddWithValue("@productname", productname);
            var x = command.ExecuteScalar();
            int exist = Convert.ToInt32(x);
            if (exist > 0)
            {
                result = true;
            }
            return result;
        }

		/// <author>Siem</author>
		/// <summary>
		/// Checks if Product exists in fridge with expiredate and Update the product in the fridge, else Insert the product in the fridge.
		/// </summary>
		/// <param name="productname"></param>
		/// <param name="productcount"></param>
		/// <param name="expiredate"></param>
		/// <returns>ProductModel product</returns>
		public ProductModel UpdateProductInFridge(string productname, int productcount, string expiredate)
		{
			conn.OpenConnection();
			var command = conn.connection.CreateCommand();
			command.CommandText = "select * from product WHERE Productname = @productname";
			command.Parameters.AddWithValue("@productname", productname);
			ProductModel product = new ProductModel();
			MySqlDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				product = new ProductModel()
				{
					ProductId = (int)reader["ProductID"],
					ProductName = (string)reader["Productname"]
				};
				reader.Close();
				if (!fridgeController.ProductExistsInFridgeProduct(product.ProductId))
				{                   
                    // Product bestaat niet in de koelkast
					command.CommandText = "INSERT INTO fridgeproduct (FPFridgeID, FPProductID, ProductCount, Expiredate, Purchasedate) VALUES(1, @FPProductID, @productcount, @expiredate, @date)";
					command.Parameters.AddWithValue("@FPProductID", product.ProductId);
					command.Parameters.AddWithValue("@productcount", productcount);
					command.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
					command.Parameters.AddWithValue("@expiredate", expiredate);
					command.ExecuteNonQuery();
					MessageBox.Show("Product toegevoegd!");
				}
				else
				{
					if (ExpiredateExists(expiredate, productname))
					{
						// Product bestaat in de koelkast en vervaldatum is gelijk aan de gegeven vervaldatum, wijzig bestaande product.
						command.CommandText = "UPDATE fridgeproduct SET ProductCount = @productcount, Expiredate = @expiredate WHERE FPProductID = @FPProductID AND Expiredate = @expiredate AND FPFridgeID = 1";
						command.Parameters.AddWithValue("@FPProductID", product.ProductId);
						command.Parameters.AddWithValue("@productcount", productcount);
						command.Parameters.AddWithValue("@expiredate", expiredate);
						command.ExecuteNonQuery();
						MessageBox.Show("Bestaande product met nieuw aantal gewijzigd.");
						
					}
                    else
                    {
						// Product bestaat in de koelkast en vervaldatum is anders, voeg nieuwe bestaande product toe.
						command.CommandText = "INSERT INTO fridgeproduct (FPFridgeID, FPProductID, ProductCount, Expiredate, Purchasedate) VALUES(1, @FPProductID, @productcount, @expiredate, @date)";
						command.Parameters.AddWithValue("@FPProductID", product.ProductId);
						command.Parameters.AddWithValue("@productcount", productcount);
						command.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
						command.Parameters.AddWithValue("@expiredate", expiredate);
						command.ExecuteNonQuery();
						MessageBox.Show("Bestaande product toegevoegd!");
					}
					
				}
				reader.Close();
				conn.CloseConnection();
			}
			return product;
		}
    }

}
