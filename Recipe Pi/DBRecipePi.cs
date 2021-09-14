using MySql.Data.MySqlClient;
using System.Windows;

namespace Recipe_Pi
{
    public class DBRecipePi
	{
		public MySqlConnection connection;
		private string server = "localhost";
		private string database = "recipepi";
		private string uid = "root";

		/// <author>Anas</author>
		/// <summary>
		/// Opens connection with database
		/// </summary>
		/// <returns></returns>
		public bool OpenConnection()
		{
			try
			{
				string connectionString;
				connectionString = "SERVER=" + server + ";" + "DATABASE=" +
				database + ";" + "UID=" + uid + ";" + "PASSWORD=;";

				connection = new MySqlConnection(connectionString);
				connection.Open();
				return true;
			}
			//catch error's and show them
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("Kan niet met server verbinden");
						break;

					case 1045:
						MessageBox.Show("Vekeerde gegevens");
						break;
				}
				return false;
			}
		}

		/// <author>Anas</author>
		/// <summary>
		/// Closes connection with database
		/// </summary>
		/// <returns></returns>
		public bool CloseConnection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}
	}
}
