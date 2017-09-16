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
        bool isAdd = false;
        string active_tab_item = "";
        private BindingListCollectionView OrdersView;
        OrdersDataContext dc = new OrdersDataContext();
        public MainWindow()
        {
            InitializeComponent();

            //var items = dc.GetAllOrders();
            //this.DataContext = items;
            //this.OrdersView = (BindingListCollectionView)(CollectionViewSource.GetDefaultView(items));
            // Добавляем обработчик для всех кнопок на гриде
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
           switch( active_tab_item )
           {
                case "Cust":
                    break;
                case "Order":
                    break;
           }
           // this.dc.SubmitChanges();
           // isAdd = false;
        }

        private void TabControl_SelectChange( object sender, SelectionChangedEventArgs e )
        {
            // ... Get TabControl reference.
            TabControl item = new TabControl();
            item = (TabControl)sender;
            // ... Set Title to selected tab header.
            var selected = item.SelectedItem as TabItem;
            //this.Title = selected.Header.ToString();
            //TabControl tab = ;
            //ctive_tab_item = e.
        }

        private void Get_Focus(object sender, EventArgs e)
        {
            TabControl tabs = new TabControl();
            TabItem item = new TabItem();

            tabs = (TabControl) sender;
            item = (TabItem) tabs.SelectedItem;
            active_tab_item = item.Name;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //TabControl tab = (TabControl) this
            //TabItem ti = Tabs1.SelectedItem as TabItem;
            // this.dc.SubmitChanges();
            // isAdd = false;
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // this.dc.SubmitChanges();
            // isAdd = false;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // this.dc.SubmitChanges();
            // isAdd = false;
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
                switch( item.status.Value )
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
    }
   
}
