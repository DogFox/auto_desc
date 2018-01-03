using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private ConnectToBase bc = new ConnectToBase();
        void LogIn(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            LoginDialog result = new LoginDialog();
            result.ShowDialog();

            DataView session = bc.ExecuteQuery("select login, password from dbo.users where Login = '" + result.Login.Text + "' and password = '" + result.Password.Text + "'");
            if (session.Count == 1)
            {
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Unable to load data.", "Error", MessageBoxButton.OK);
                Shutdown();
            }
        }
    }


}
