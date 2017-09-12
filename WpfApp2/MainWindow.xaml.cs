using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        string leftop = ""; // Левый операнд
        string operation = ""; // Знак операции
        string rightop = ""; // Правый операнд

        public MainWindow()
        {
            InitializeComponent();
            // Добавляем обработчик для всех кнопок на гриде
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            if( menuItem.Header.ToString().Equals("Выход")) //при нажатии выход - ПО закрывается
            {
               this.Close();
            }
        }
    }
}
