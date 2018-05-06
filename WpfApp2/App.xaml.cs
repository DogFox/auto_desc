using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Linq;
using System.Data.SqlClient;
using MahApps.Metro.Controls.Dialogs;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
         
        /*
         * Только админ может зайти и создать пользователей. 
         * ПОльзователи подключаются под своими логинами и используют ПО. Прав на регистрацию у пользователей нет
         * у любого пользовтаеля ПО права админа на SQL из под ПО. НАдо будет поправить.
         */

        void LogIn(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            //Parts2 mainWindow = new Parts2();

            LoginDialog result = new LoginDialog();
            result.ShowDialog();
            mainWindow.Show();
        }
    }

    public static class Session
    {
        public static string Name { get; set; }
        public static int id { get; set; }
    }
    public static class StringExtensions
    {
        public static bool Contains(this String str, String substring,
                                    StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
    public static class ConnectToBase
    {
        public static string GetConnectionString()
        {
            return global::WpfApp2.Properties.Settings.Default.auto76ConnectionString;
        }

        public static DataView ExecuteQuery(string sql)
        {
            //if (!CheckConnectToBase().Equals("")) return new DataView(); //TODO сообщение об ошибке коннекта
            var UsersTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                var printMsg = "";
                connection = new SqlConnection(GetConnectionString());//напрямую стрингой
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.InfoMessage += (object obj, SqlInfoMessageEventArgs e) => {
                    printMsg = e.Message;
                };
                connection.Open();

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Columns.Count == 0 && printMsg != "")
                {
                    dt.Columns.Add(new DataColumn("printMsg"));
                    List<string> list = new List<string>();
                    list.Add(printMsg);
                    dt.Rows.Add(list.ToArray());
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return null;
        }
    }
    public static class MyMessageBox
    {
        public static MetroDialogSettings YesNoMSGBox()
        {
            return new MetroDialogSettings()
            {
                AffirmativeButtonText = "Да",
                NegativeButtonText = "Нет",
                AnimateShow = true,
                AnimateHide = false
            };
        }
    }
 }
