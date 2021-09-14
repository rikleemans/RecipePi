using Recipe_Pi.Controllers;
using Recipe_Pi.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using Recipe_Pi.ViewModels;
using Recipe_Pi.Views.Navigation;
using System.Windows.Media;
using Recipe_Pi.Views;
using System;
using System.Windows.Threading;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for RecipeViewer.xaml
    /// </summary>
    public partial class RecipeViewer : Window, INotifyPropertyChanged
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public event PropertyChangedEventHandler PropertyChanged;
        ProductController productController = new ProductController();
        SerialPort mySerialPort = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);          

        public RecipeViewer()
        {
            InitializeComponent();
            this.DataContext = this;
            //Handler(); 
            UserModel userModel = new UserModel { UserFirstName = App.UserModel.UserFirstName, UserLastName = App.UserModel.UserLastName };
            lblUser.Content = userModel.ToString();                       
            DataContext = new MainViewModel(); // Binding for navigation.

            // Every second it calls the Timer_Tick event.
            timer.Interval= TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <author>Siem</author>
        /// <summary>
        /// Sets lblClock.content to the time from now in hours:minutes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            lblClock.Content = DateTime.Now.ToString("HH:mm");
        }

        public void NotifyPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }

        /// <author>Anas</author>
        /// <summary>
        /// Creating a handler to activate when the app receives data via the port. 
        /// </summary>
        private void Handler()
        {
            mySerialPort.ReadTimeout = 2000;
            mySerialPort.Open();
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(ProductReceived);
        }

        /// <author>Anas</author>
        /// <summary>
        /// Reading and sending the barcode to the Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProductReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Convert the barcode to a string
            string input = mySerialPort.ReadExisting();
            //Sending the barcode to the Database
            var DBRecipePi = new DBRecipePi();
            productController.GetProductByBarcode(input);
        }

        /// <author>Siem</author>
        /// <summary>
        /// Switches visibilty from closeicon to menuicon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btnMenuOpen.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;

        }

        /// <author>Siem</author>
        /// <summary>
        /// Switches visibilty from menuicon to closeicon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            btnMenuOpen.Visibility = Visibility.Collapsed;
            btnCloseMenu.Visibility = Visibility.Visible;
        }

        /// <author>Siem</author>
        /// <summary>
        /// Returns to loginviewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewer loginViewer = new LoginViewer();
            loginViewer.Show();
            Close();
        }

        /// <author>Rik</author>
        /// <summary>
        /// Method overloading for switchting between light and dark mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            string theme = btnChangeTheme.Content.ToString();

            string Dark = Themes.Donkermodus.ToString();
            string Light = Themes.Lichtmodus.ToString();

            if (theme == Light)
            {

                ChangeTheme(Dark);

            }
            else if (theme == Dark)
            {

                ChangeTheme(Light, Dark);
            }
        }

        public void ChangeTheme(string dark)
        {
            // Lightmode
            btnChangeTheme.Content = dark;
            MessageBox.Show("Lichtmodus");
            grid1.Background = new SolidColorBrush(Color.FromRgb(74, 74, 245));
            grdBackground.Background = new SolidColorBrush(Color.FromRgb(74, 74, 245));
            GridMenu.Background = new SolidColorBrush(Color.FromRgb(230, 230, 240));
            GridMenuIcon.Background = new SolidColorBrush(Color.FromRgb(230, 230, 240));
            btnKoelkast.Foreground = new SolidColorBrush(Color.FromRgb(40, 62, 145));
            btnFavorieten.Foreground = new SolidColorBrush(Color.FromRgb(40, 62, 145));
            btnRecepten.Foreground = new SolidColorBrush(Color.FromRgb(40, 62, 145));
            iMenu.Foreground = new SolidColorBrush(Color.FromRgb(40, 62, 145));
            iArrowLeft.Foreground = new SolidColorBrush(Color.FromRgb(40, 62, 145));
        }

        public void ChangeTheme(string light, string msg)
        {
            // Darkmode
            btnChangeTheme.Content = light;
            MessageBox.Show(msg);
            grid1.Background = new SolidColorBrush(Color.FromRgb(44, 47, 51));
            grdBackground.Background = new SolidColorBrush(Color.FromRgb(44, 47, 51));
            GridMenu.Background = new SolidColorBrush(Color.FromRgb(35, 39, 42));
            GridMenuIcon.Background = new SolidColorBrush(Color.FromRgb(35, 39, 42));
            btnKoelkast.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            btnFavorieten.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            btnRecepten.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            iMenu.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            iArrowLeft.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

        }

        private void btnPurchases_Click(object sender, RoutedEventArgs e)
        {
           Purchases purchases = new Purchases();
           purchases.Show();
           Close();
        }
    }
}
