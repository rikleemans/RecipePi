using MySql.Data.MySqlClient;
using Recipe_Pi.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Recipe_Pi.Controllers
{
    public class UserController
    {
		DBRecipePi conn = new DBRecipePi();

		/// <author>Siem</author>
		/// <summary>
		/// Checks if password matches iwth confirmationpassword.
		/// </summary>
		/// <param name="password"></param>
		/// <param name="confirmpassword"></param>
		/// <returns></returns>
		public bool PasswordEqualsConfirmPassword(string password, string confirmpassword)
        {
            return password == confirmpassword;
		}

		/// <author>Siem</author>
		/// <summary>
		/// method to register a new user to the database with an encrypted password.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="usercountry"></param>
		/// <returns>boolean result</returns>
		public async Task<(bool, string)> Register(string username, string password, string usercountry, string userfirstname, string userlastname)
		{
			bool result = false;

			string message = "Niet alle velden zijn ingevuld";
			if (username == string.Empty || password == string.Empty || usercountry == string.Empty || userfirstname == string.Empty || userlastname == string.Empty)
			{
				return (result, message);
			}
			conn.OpenConnection();

			string salt = BCrypt.Net.BCrypt.GenerateSalt();

			password += "RANDOM";

			string hashToStoreInDatabase = BCrypt.Net.BCrypt.HashPassword(password, salt);
			string role = Roles.Gebruiker.ToString();
			try
			{
				var command = conn.connection.CreateCommand();
				command.CommandText = "INSERT INTO user (Username, UserFridgeID, Userpassword, Userfirstname, Userlastname, Usercountry, Userrole) VALUES(@username, 1, @password, @userfirstname, @userlastname, @usercountry, @role)";
				command.Parameters.Add(new MySqlParameter("@username", username));
				command.Parameters.Add(new MySqlParameter("@userfirstname", userfirstname));
				command.Parameters.Add(new MySqlParameter("@userlastname", userlastname));
				command.Parameters.Add(new MySqlParameter("@password", hashToStoreInDatabase));
				command.Parameters.Add(new MySqlParameter("@usercountry", usercountry));
				command.Parameters.Add(new MySqlParameter("@role", role));
				await command.ExecuteNonQueryAsync();

				result = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);

			}
			conn.CloseConnection();
			return (result, message);
		}

		/// <author>Siem</author>
		/// <summary>
		/// checks if username allready exists in the database.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool UsernameAlreadyExists(string username)
		{
			bool result = false;
			conn.OpenConnection();

			var command = conn.connection.CreateCommand();
			command.CommandText = "SELECT COUNT(*) FROM user WHERE Username = @username";
			command.Parameters.AddWithValue("@username", username);
			var x = command.ExecuteScalar();
			var exist = Convert.ToInt32(x);
			if (exist > 0)
			{
				result = true;
			}
			conn.CloseConnection();
			return result;
		}

		/// <author>Siem</author>
		/// <summary>
		/// checks if username and password are filled.
		///	method to login using the database and to verify the encrypted password.
		///	returns the username and userid in usermodel and sets the bool succes true.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>UserModel userModel</returns>
		public (UserModel, string) Login(string username, string password)
		{
			UserModel userModel = new UserModel();
			string message = "Onjuiste gegevens ingevuld!";
			if (username == string.Empty || password == string.Empty)
            {
				return (userModel, message);
			}			

			try
			{
				conn.OpenConnection();
				var command = conn.connection.CreateCommand();
				command.CommandText = "SELECT * FROM user WHERE Username=@username";
				command.Parameters.Add(new MySqlParameter("@username", username));
				MySqlDataReader reader = command.ExecuteReader();
				//if the reader has rows than it will proceed to add the text RANDOM with the given password string to verify the encryption.
				if (reader.HasRows)
				{
					reader.Read();
					
					string sPass = (string)reader["Userpassword"];
					password += "RANDOM";
					bool passwordsMatch = BCrypt.Net.BCrypt.Verify(password, sPass);
					if (passwordsMatch == true)
					{
						userModel.UserName = (string)reader["Username"];
						userModel.UserId = (int)reader["UserID"];
						userModel.UserFirstName = (string)reader["Userfirstname"];
						userModel.UserLastName = (string)reader["Userlastname"];
						userModel.Succes = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				conn.CloseConnection();
			}
			return (userModel, message);
		}
	}
}
