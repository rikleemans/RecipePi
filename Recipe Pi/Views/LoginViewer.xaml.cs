using Recipe_Pi.Controllers;
using Recipe_Pi.Models;
using System.Windows;
using Recipe_Pi.Views.Navigation;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for LoginViewer.xaml
    /// </summary>
    public partial class LoginViewer : Window
    {
        UserController userController = new UserController();
        public LoginViewer()
        {
            InitializeComponent();
        }

        /// <author>Siem</author>
        /// <summary>
        /// Calls Login function if both textboxes are filled in,
        /// Shows recipeviewer and closes current viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string message;
            (App.UserModel, message) = userController.Login(tbUsername.Text, pbPassword.Password);
            if (App.UserModel.Succes)
            {
                RecipeViewer recipeViewer = new RecipeViewer();
                recipeViewer.Show();
                Close();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        /// <author>Siem</author>
        /// <summary>
        /// Shows registerviewer, closes current viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterViewer registerViewer = new RegisterViewer();
            registerViewer.Show();
            Close();
        }
    }
}
