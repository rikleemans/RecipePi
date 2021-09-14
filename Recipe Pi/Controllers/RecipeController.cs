using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System.Collections.Generic;

namespace Recipe_Pi.Controllers
{
    public class RecipeController
    {
		DBRecipePi conn = new DBRecipePi();


		/// <author>Siem</author>
		/// <summary>
		///		returns all rows from the recipe table in a list
		/// </summary>
		/// <returns>list</returns>
		public List<RecipeModel> GetAllRecipes()
		{
			conn.OpenConnection();

			var command = conn.connection.CreateCommand();
			command.CommandText = "select * FROM recipe";

			List<RecipeModel> list = new List<RecipeModel>();
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(GetRecipeModelFromReader(reader));
			}

			conn.CloseConnection();

			return list;
		}

		/// <author>Siem</author>
		/// <summary>
		///		Put RecipeID and Recipename in The Recipemodel
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private RecipeModel GetRecipeModelFromReader(MySqlDataReader reader)
		{
			return new RecipeModel
			{
				RecipeId = reader.GetInt32("RecipeID"),
				RecipeName = reader.GetString("Recipename")
			};
		}
				
	}
}
