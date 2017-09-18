using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
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
        string active_tab_item = "Order"; // так как начинаем с окна заказов
        private BindingListCollectionView OrdersView;
        private BindingListCollectionView CustomersView;
        OrdersDataContext odc = new OrdersDataContext();
        CustomersDataContext cdc = new CustomersDataContext();

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
                    this.cdc.SubmitChanges();
                    break;
                case "Order":
                    this.odc.SubmitChanges();
                    break;
           }
           // this.dc.SubmitChanges();
           // isAdd = false;
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
            isAdd = true;

            switch (active_tab_item)
            {
                case "Cust":
                    var items = cdc.GetAllCustomers();

                    customer new_cust = new customer();
                    new_cust.name = "11";
                    new_cust.phone = 0;
                    new_cust.addres = "11";
                    new_cust.price_level = 1;
                    new_cust.id = 1;

                    cdc.customers.InsertOnSubmit(new_cust);
                    cdc.SubmitChanges();

                    this.CustGrid.UpdateLayout();                    
                    break;
            }
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
            switch (active_tab_item)
            {
                case "Cust":
                    var itemCust = row.DataContext as customer;
                    if (itemCust != null)
                    {
                        var origCust = cdc.customers.GetOriginalEntityState(itemCust);
                        if (origCust == null)
                            this.cdc.customers.Attach(itemCust);
                    }
                    break;
                case "Order":
                    var itemOrd = row.DataContext as order;
                    if (itemOrd != null)
                    {
                        var orig = odc.orders.GetOriginalEntityState(itemOrd);
                        if ( orig == null )
                            this.odc.orders.Attach(itemOrd);

                        switch (itemOrd.status.Value)
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
                    break;
            }
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
