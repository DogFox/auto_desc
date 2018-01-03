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

        /*
         * Только админ может зайти и создать пользователей. 
         * ПОльзователи подключаются под своими логинами и используют ПО. Прав на регистрацию у пользователей нет
         * у любого пользовтаеля ПО права админа на SQL из под ПО. НАдо будет поправить.
         */

        void LogIn(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            LoginDialog result = new LoginDialog();
            result.ShowDialog();

            DataView check_autorization = bc.ExecuteQuery("select id, login, password from dbo.users where Login = '" + result.Login.Text + "' and password = '" + result.Password.Text + "'");
            if (check_autorization.Count == 1)
            {
                Session.Name = check_autorization[0].Row["login"].ToString();
                Session.id = check_autorization[0].Row["id"].ToString();

                bc.ExecuteQuery("insert into dbo.Session ( login, id_login, time )"
                                   + " Values ( '" + Session.Name + "', '" + Session.id 
                                   + "', getdate() ) ");

                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Неправильный пароль, пидрилка.", "Error", MessageBoxButton.OK);
                Shutdown();
            }
        }
    }

    public static class Session
    {
        public static string Name { get; set; }
        public static string id { get; set; }
    }


}
