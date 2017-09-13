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
               this.Close();}
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            DataGridRow row = e.Row;
            var item = row.DataContext as order;
            if (item != null)
            {
                switch( item.isArchive.Value )
                {
                    case 1:
                        e.Row.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case 2:
                        e.Row.Background = new SolidColorBrush(Colors.Blue);
                        break;
                    case 3:
                        e.Row.Background = new SolidColorBrush(Colors.Green);
                        break;
                }
            }
        }
        private void DataGridRow_EditEnding1( object sender, DataGridRowEditEndingEventArgs e)
        {
            var Row = "";
        }
        private void BindingSource_CurrentItemChanged( object sender, EventArgs e)
        {
            /*DataRow ThisDataRow = ((DataRowView)((BindingSource)sender).Current).Row;
            if (ThisDataRow.RowState == DataRowState.Modified)
            {
                TableAdapter.Update(ThisDataRow);
            }*/
        }

        private void Grid_change_background( object sender, RoutedEventArgs e )
        {
            order order = OrdersGrid.SelectedItem as order;

            if( order.isArchive.Equals( 1 ) )
            {
                OrdersGrid.RowBackground = new SolidColorBrush(Colors.WhiteSmoke);
            }
            if (order.isArchive.Equals(2))
            {
                OrdersGrid.RowBackground = new SolidColorBrush(Colors.WhiteSmoke);
            }
        }
    }
   
}
