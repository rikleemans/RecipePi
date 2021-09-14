using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System;
using System.Collections.Generic;

namespace Recipe_Pi.Controllers
{
    public class FridgeController
    {
		DBRecipePi conn = new DBRecipePi();

		/// <author>Siem</author>
		/// <summary>
		/// Returns result true if the product exists in the fridge. 
		/// else it returns result false.
		/// </summary>
		/// <param name="productid"></param>
		/// <returns> boolean result</returns>
		public bool ProductExistsInFridgeProduct(int productid)
		{
			conn.OpenConnection();
			bool result = false;
			var command = conn.connection.CreateCommand();
			command.CommandText = "SELECT COUNT(*) FROM product INNER JOIN fridgeproduct ON product.ProductID = fridgeproduct.FPProductID WHERE productID = @productid";
			command.Parameters.AddWithValue("@productid", productid);
			var x = command.ExecuteScalar();
			int exist = Convert.ToInt32(x);
			if (exist > 0)
			{
				result = true;
			}
			return result;
		}

		/// <author>Siem, Rik</author>
		/// <summary>
		/// Returns productname, product count and purchasedate in a list
		/// </summary>
		/// <returns></returns>
        public List<ProductModel> GetAllPurchases()
        {
            conn.OpenConnection();

            var command = conn.connection.CreateCommand();
            command.CommandText = "SELECT Productname, ProductCount, Purchasedate FROM fridgeproduct INNER JOIN `product`ON `product`.`ProductID` = `fridgeproduct`.`FPProductID`; ";

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
		///	Put ProductID, Productname, ProductCount and Purchasedate in The ProductModel
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private ProductModel GetProductModelFromReader(MySqlDataReader reader)
		{
			return new ProductModel
			{
				ProductName = reader.GetString("Productname"),
				ProductCount = reader.GetInt32("ProductCount"),
				PurchaseDate = reader.GetString("Purchasedate"),
			};
		}
	}
}
