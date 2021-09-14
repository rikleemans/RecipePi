using Recipe_Pi.Models;
using System;
using System.Windows;

namespace Recipe_Pi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserModel UserModel { get; set; }

        /// <author>Siem</author>
        /// <summary>
        /// Check for MySQL database connection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            DBRecipePi dBRecipePi = new DBRecipePi();
            dBRecipePi.OpenConnection();
            base.OnStartup(e);
        }

        /// <author>Siem</author>
        /// <summary>
        /// On exit gives exit message depending on time.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            // Morning
            TimeSpan morningstart = new TimeSpan(06, 0, 0); // 06:00
            TimeSpan morningend = new TimeSpan(11, 59, 0); // 11:59
            // Midday
            TimeSpan middaystart = new TimeSpan(12, 0, 0); // 12:00
            TimeSpan middayend = new TimeSpan(17, 59, 0); // 17:59
            // Evening
            TimeSpan eveningstart = new TimeSpan(18, 0, 0); // 18:00
            TimeSpan eveningend = new TimeSpan(23, 59, 0); // 23:59
            // Night
            TimeSpan nightstart = new TimeSpan(00, 0, 0); // 00:00
            TimeSpan nightend = new TimeSpan(05, 59, 0); // 05:59

            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > morningstart) && (now < morningend))
            {
                MessageBox.Show("Goedemorgen, u bent uitgelogd om " + now.ToString(@"hh\:mm"));
            }
            else if((now > middaystart) && (now < middayend))
            {
                MessageBox.Show("Goedemiddag, u bent uitgelogd om " + now.ToString(@"hh\:mm"));
            }
            else if ((now > eveningstart) && (now < eveningend))
            {
                MessageBox.Show("Goedeavond, u bent uitgelogd om " + now.ToString(@"hh\:mm"));
            }
            else if ((now > nightstart) && (now < nightend))
            {
                MessageBox.Show("Weltruste, u bent uitgelogd om " + now.ToString(@"hh\:mm"));
            }
            base.OnExit(e);
        }
    }
}
