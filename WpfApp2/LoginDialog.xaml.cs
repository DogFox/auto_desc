using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : MetroWindow
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private async void Ok_click( object sender, RoutedEventArgs e)
        {
            DataView check_autorization = ConnectToBase.ExecuteQuery("select id, login, password from dbo.users where Login = '" + this.Login.Text + "' and password = '" + this.Password.Text + "'");
            if (check_autorization.Count == 1)
            {
                Session.Name = check_autorization[0].Row["login"].ToString();
                Session.id = (int)check_autorization[0].Row["id"];

                ConnectToBase.ExecuteQuery("insert into dbo.Session ( login, id_login, time )"
                                   + " Values ( '" + Session.Name + "', '" + Session.id
                                   + "', getdate() ) ");
                this.Close();

            }
            else
            {
                await this.ShowMessageAsync("Ошибка!", "Неправильный пароль!");
            }

        }
        private void LoginDialog_KeyDown(object sender, KeyEventArgs e)
        {
            // ... Test for Enter key.
            switch (e.Key)
            {
                case Key.Enter:
                    this.Ok_click(sender, e);
                    break;
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
