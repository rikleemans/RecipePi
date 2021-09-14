namespace Recipe_Pi.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string FavoriteRecipe { get; set; }
        public bool Succes { get; set; }
        public string Role { get; set; }

        /// <author>Rik</author>
        /// <summary>
        /// Override ToString to show Userfirstname and lastname
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserFirstName + " " + UserLastName;
        }
    }
}
