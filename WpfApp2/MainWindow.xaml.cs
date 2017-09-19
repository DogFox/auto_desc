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
        OrdersDataContext odc = new OrdersDataContext();
        CustomersDataContext cdc = new CustomersDataContext();
        SuppliersDataContext sdc = new SuppliersDataContext();

        private IEnumerable<customer> cust_list;
        private IEnumerable<order> order_list;
        private IEnumerable<supplier> supplier_list;

        public MainWindow()
        {
            InitializeComponent();

            //var items = dc.GetAllOrders();
            //this.DataContext = items;
            //this.OrdersView = (BindingListCollectionView)(CollectionViewSource.GetDefaultView(items));
            // Добавляем обработчик для всех кнопок на гриде
        }

        private void CustGrid_Loaded(object sender, RoutedEventArgs e)
        {
            cust_list = cdc.GetAllCustomers();
            CustGrid.ItemsSource = cust_list;
        }
        private void OrderGrid_Loaded(object sender, RoutedEventArgs e)
        {
            order_list = odc.GetAllOrders();
            OrderGrid.ItemsSource = order_list;
        }
        private void SupplierGrid_Loaded( object sender, RoutedEventArgs e)
        {
            supplier_list = sdc.GetAllSuppliers();
            SupGrid.ItemsSource = supplier_list;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
           switch( active_tab_item )
           {
                case "Cust":
                    this.cdc.SubmitChanges();
                    CustGrid.Items.Refresh();
                    break;
                case "Order":
                    this.odc.SubmitChanges();
                    OrderGrid.Items.Refresh();
                    break;
                case "Supl":
                    this.sdc.SubmitChanges();
                    SupGrid.Items.Refresh();
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
                    customer new_cust = new customer();
                    new_cust.name = "Ввдите имя";
                    new_cust.phone = "Укажите телефон";
                    new_cust.addres = "Введите адрес";
                    new_cust.price_level = 1;
                    new_cust.id = 1;
                    this.cdc.customers.InsertOnSubmit(new_cust);
                    this.cdc.SubmitChanges();

                    cust_list = cdc.GetAllCustomers();
                    CustGrid.ItemsSource = cust_list;

                    CustGrid.Items.Refresh();

                    break;

                case "Order":
                    order new_order = new order();

                    NewOrder newOrder = new NewOrder();
                    newOrder.Owner = this;
                    newOrder.ShowDialog();

                    order_list = odc.GetAllOrders();
                    OrderGrid.ItemsSource = order_list;
                    OrderGrid.Items.Refresh();

                    break;

                case "Supl":
                    supplier new_supplier = new supplier();
                    new_supplier.name = "имя";
                    new_supplier.full_name = "полное имя";
                    new_supplier.inn = "120";
                    new_supplier.kpp = "1";
                    new_supplier.phone = 1;

                    this.sdc.suppliers.InsertOnSubmit(new_supplier);
                    this.sdc.SubmitChanges();

                    supplier_list = sdc.GetAllSuppliers();
                    SupGrid.ItemsSource = supplier_list;

                    SupGrid.Items.Refresh();

                    break;
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            switch (active_tab_item)
            {
                case "Cust":
                    if (MessageBox.Show("Do you want to delete this customer?", "Delete", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                    {
                        customer item = CustGrid.SelectedItem as customer;
                        this.cdc.customers.DeleteOnSubmit(item);
                        this.cdc.SubmitChanges();

                        cust_list = cdc.GetAllCustomers();
                        CustGrid.ItemsSource = cust_list;
                        CustGrid.Items.Refresh();
                    }
                    isAdd = false;
                    break;

                case "Order":
                    if (MessageBox.Show("Do you want to delete this order?", "Delete", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                    {
                        order item = OrderGrid.SelectedItem as order;
                        this.odc.orders.DeleteOnSubmit(item);
                        this.odc.SubmitChanges();

                        order_list = odc.GetAllOrders();
                        OrderGrid.ItemsSource = order_list;
                        OrderGrid.Items.Refresh();
                    }
                    isAdd = false;
                    break;

                case "Supl":
                    if (MessageBox.Show("Do you want to delete this supplier?", "Delete", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                    {
                        supplier item = SupGrid.SelectedItem as supplier;
                        this.sdc.suppliers.DeleteOnSubmit(item);
                        this.sdc.SubmitChanges();

                        supplier_list = sdc.GetAllSuppliers();
                        SupGrid.ItemsSource = supplier_list;
                        SupGrid.Items.Refresh();
                    }
                    isAdd = false;
                    break;
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {/*
            if (isAdd)
            {
                CustomerView.CancelNew();
                CustomerView.Remove(this.CustomerView.CurrentItem);
            }
            else
            {
                CustomerView.CancelEdit();
                dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                CustomerView.Refresh();
            }*/
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            if( menuItem.Header.ToString().Equals("Выход")) //при нажатии выход - ПО закрывается
               this.Close();
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

                        switch (itemOrd.status)
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
