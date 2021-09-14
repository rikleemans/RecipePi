using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Recipe_Pi.Controllers
{
    public class FavoriteController
    {
        DBRecipePi conn = new DBRecipePi();

        /// <author>Rik, Siem</author>
        /// <summary>
        /// with this function we retrieve the favorite recipe from the database,
        /// and we also ensure that the username that logged in is the same as the database.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<FavoriteModel> GetAllFavorites(string userid)
        {
            conn.OpenConnection();

            var command = conn.connection.CreateCommand();
            command.CommandText = "SELECT * FROM favoriterecipe INNER JOIN `recipe`ON `recipe`.`RecipeID` = `favoriterecipe`.`FRRecipeID` INNER JOIN `user` ON `favoriterecipe`.`FRUserID` = `user`.`UserID` WHERE user.Userid = @userid; ";
            command.Parameters.AddWithValue("@userid", userid);

            List<FavoriteModel> list = new List<FavoriteModel>();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {               
                list.Add(GetFavoriteModelFromReader(reader));
            }

            reader.Close();
            conn.CloseConnection();

            return list;
        }

        /// <author>Rik</author>
        /// <summary>
        /// Put FRRecipeID and Recipename in The FavoriteModel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private FavoriteModel GetFavoriteModelFromReader(MySqlDataReader reader)
        {
            return new FavoriteModel
            {
                FRRecipeID = reader.GetInt32("FRRecipeID"),
                RecipeName = reader.GetString("Recipename")
            };
        }

        /// <author>Rik</author>
        /// <summary>
        /// this function we have a query that adds the favorite recipe to the database with "insert".
        /// In addition, we also link the recipeid and userid to each other,
        /// so that this is saved and everyone has their own favorite
        /// </summary>
        /// <param name="FRrecipeid"></param>
        /// <param name="FRuserid"></param>
        /// <returns></returns>
        public bool InsertFav(int FRrecipeid, int FRuserid)
        {
            conn.OpenConnection();
            var command = conn.connection.CreateCommand();
            if (!FavoriteRecipeAlreadyExist(FRuserid, FRrecipeid))
            {
                command.CommandText = "INSERT INTO favoriterecipe (FRRecipeID, FRUserID) VALUES(@FRrecipeid, @FRuserid)";

                command.Parameters.AddWithValue("@FRrecipeid", FRrecipeid);
                command.Parameters.AddWithValue("@FRuserid", FRuserid);
                command.ExecuteNonQuery();
                conn.CloseConnection();
                return true;
            }
            else
            {
                MessageBox.Show("Recept is al favoriet");
                conn.CloseConnection();
                return false;
            }
        }

        /// <author>Rik</author>
        /// <summary>
        /// in this function we check if the favorite recipe has already been added to the list,
        /// which is linked to the user.
        /// in the function insertfav there is an if statement that first executes this function, then pair insert.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="FRrecipeid"></param>
        /// <returns></returns>
        public bool FavoriteRecipeAlreadyExist(int userid, int FRrecipeid)
        {
            bool result = false;
            var command = conn.connection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*) 
                FROM favoriterecipe 
                INNER JOIN `recipe`
                ON `recipe`.`RecipeID` = `favoriterecipe`.`FRRecipeID` 
                INNER JOIN `user` 
                ON `favoriterecipe`.`FRUserID` = `user`.`UserID` 
                WHERE user.Userid = @userID AND favoriterecipe.FRRecipeID = @FRrecipeid";

            command.Parameters.AddWithValue("@userid", userid);
            command.Parameters.AddWithValue("@FRrecipeid", FRrecipeid);
            var x = command.ExecuteScalar();
            int exist = Convert.ToInt32(x);
            if (exist > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
