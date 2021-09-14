using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Recipe_Pi.Controllers;
using Recipe_Pi.Models;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for RegisterViewer.xaml
    /// </summary>
    public partial class RegisterViewer : Window
    {
        UserController userController = new UserController();
        public RegisterViewer()
        {
            InitializeComponent();
        }

        /// <author>Siem</author>
        /// <summary>
        /// Registers a new user, 
        /// checks if username allready exists in the database, 
        /// set all the textboxes empty after refrencing user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string message;
            bool result;

            if (userController.PasswordEqualsConfirmPassword(pbPass.Password, pbPassConfirm.Password))
            {
                if (!userController.UsernameAlreadyExists(tbUser.Text))
                {
                    (result, message) = await userController.Register(tbUser.Text, pbPass.Password, tbCountry.Text, tbUserfirstname.Text, tbUserlastname.Text);

                    if (result)
                    {
                        UserModel userModel = new UserModel { UserFirstName = tbUserfirstname.Text, UserLastName = tbUserlastname.Text };
                        MessageBox.Show(userModel.ToString() + " is geregistreerd.");
                        tbUser.Text = String.Empty;
                        tbUserfirstname.Text = String.Empty;
                        tbUserlastname.Text = String.Empty;
                        tbCountry.Text = String.Empty;
                        pbPass.Password = String.Empty;
                        pbPassConfirm.Password = String.Empty;
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }
                }
                else
                {
                    MessageBox.Show("Deze gebruikernaam is al in gebruik.");
                }
            }
            else
            {
                MessageBox.Show("De wachtwoorden komen niet overeen.");
            }
        }

        /// <author>Siem</author>
        /// <summary>
        /// Returns to loginviewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginViewer loginViewer = new LoginViewer();
            loginViewer.Show();
            Close();
        }
    }
}
