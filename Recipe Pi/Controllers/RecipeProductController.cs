using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System.Collections.Generic;

namespace Recipe_Pi.Controllers
{
    public class RecipeProductController
    {
		DBRecipePi conn = new DBRecipePi();

		/// <author>Siem</author>
		/// <summary>
		///	returns the ingredients in the fridge from the according recipe in a list.
		/// </summary>
		/// <param name="recipeid"></param>
		/// <returns>List<RecipeProductModel> list</returns>
		public List<RecipeProductModel> GetRecipeProduct(int recipeid)
		{
			conn.OpenConnection();
			var command = conn.connection.CreateCommand();

			command.CommandText = @"
				SELECT 
					product.Productname, IFNULL(fridgeproduct.ProductCount, 0) as 'ProductCount', fridgeproduct.Expiredate
				FROM 
					product 
				INNER JOIN 
					recipeproduct 
						ON product.ProductID = recipeproduct.RPProductID 
				LEFT OUTER JOIN 
					fridgeproduct 
						ON product.ProductID = fridgeproduct.FPProductID
				WHERE 
					recipeproduct.RPRecipeID = @recipeid
			";
			command.Parameters.AddWithValue("@recipeid", recipeid);

			List<RecipeProductModel> list = new List<RecipeProductModel>();
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(GetRecipeProductModelFromReader(reader));
			}
			conn.CloseConnection();

			return list;
		}

		/// <author>Siem</author>
		/// <summary>
		///	Put Productname, ProductCount and Expiredate in The RecipeProductModel
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private RecipeProductModel GetRecipeProductModelFromReader(MySqlDataReader reader)
		{
			return new RecipeProductModel
			{
				ProductName = reader.GetString("Productname"),
				ProductCount = reader.GetInt32("ProductCount"),
				ExpireDate = reader.GetString("Expiredate")
			};
		}
	}
}
