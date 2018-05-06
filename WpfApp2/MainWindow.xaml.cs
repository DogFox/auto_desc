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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WpfApp2
{

    public partial class MainWindow : MetroWindow
    {
        bool isAdd = false;
        string active_tab_item = "Part"; // так как начинаем с окна заказов
        OrdersDataContext_Mod odc = new OrdersDataContext_Mod();
        CustomersDataContext cdc = new CustomersDataContext();
        SuppliersDataContext sdc = new SuppliersDataContext();
        PartsDataContext pdc = new PartsDataContext(); 

        private IEnumerable<customer> cust_list;
        //private IEnumerable<order> order_list;
        private DataView order_list;
        private DataView supplier_list;
        private DataView part_list;

        public MainWindow()
        {
            InitializeComponent();
            //User_Login.Text = Session.Name; 
        }
        // Инициализация датагридов при переключении вкладок
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            switch (active_tab_item)
            {
                case "Part":
                    DataView orderParts_list = ConnectToBase.ExecuteQuery(@"select o.number Заказ, o.date [Дата заказа], p.name as Запчасть
                                                                , part_number [парт номер], sup_price [Цена поставщика] 
                                                                , sup_price Цена, s.name Поставщик, o.author Менеджер 
                                                                from dbo.parts_order p 
                                                                join dbo.suppliers s on s.id = p.sup_id
                                                                join dbo.orders o on o.id = p.order_id 
                                                                where o.type = 1" );
                    PartGrid.ItemsSource = orderParts_list;
                    break;
                case "Cust":
                    cust_list = cdc.GetAllCustomers();
                    CustGrid.ItemsSource = cust_list;
                    break;
                case "Order":
                    order_list = odc.GetAllOrders();
                    OrderGrid.ItemsSource = order_list;
                    break;
                case "Supl":
                    supplier_list = sdc.GetAllSuppliers();
                    SupGrid.ItemsSource = supplier_list;
                    break;
                case "Price":
                    part_list = pdc.GetAllParts();
                    PriceGrid.ItemsSource = part_list;
                    break;
            }
        }
        //Сохранение изменений в датагриде
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            switch (active_tab_item)
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
                case "Price":
                    this.pdc.SubmitChanges();
                    PriceGrid.Items.Refresh();
                    break;
            }
            // this.dc.SubmitChanges();
            // isAdd = false;
        }
        //Добавление новых объектов в датагрид
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

                case "Price":
                    part new_part = new part();

                    NewPart newPart = new NewPart();
                    newPart.Owner = this;
                    newPart.ShowDialog();

                    part_list = pdc.GetAllParts();
                    PriceGrid.ItemsSource = part_list;
                    PriceGrid.Items.Refresh();

                    break;
            }
        }

        private void Edit_Click( object sender, RoutedEventArgs e)
        {
            EditRowDataGrid();
        }
        public void EditRowDataGrid()
        {
            DataRowView drv;
            switch (active_tab_item)
            {
                case "Cust":
                    break;

                case "Order":
                    drv = OrderGrid.SelectedItem as DataRowView;
                    order edit_order = new order(drv);

                    NewOrder newOrder = new NewOrder(edit_order);
                    newOrder.Owner = this;
                    newOrder.ShowDialog();

                    odc.SubmitChanges();
                    order_list = odc.GetAllOrders();
                    OrderGrid.ItemsSource = order_list;
                    OrderGrid.Items.Refresh();

                    break;

                case "Supl":
                    drv = SupGrid.SelectedItem as DataRowView;
                    supplier edit_supplier = new supplier(drv);

                    NewSupplier newSupplier = new NewSupplier(edit_supplier);
                    newSupplier.Owner = this;
                    newSupplier.ShowDialog();

                    sdc.SubmitChanges();
                    supplier_list = sdc.GetAllSuppliers();
                    SupGrid.ItemsSource = supplier_list;
                    SupGrid.Items.Refresh();
                    break;

                case "Price":
                    break;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
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
                    var mySettings = MyMessageBox.YesNoMSGBox();
                    var result = await this.ShowMessageAsync("Удалить заказ?",
                        "Вы уверены что хотите перенести заказ в архив?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

                    if (result == MessageDialogResult.Affirmative)
                    {
                        DataRowView drv = OrderGrid.SelectedItem as DataRowView;
                        order item = new order(drv);

                        this.odc.DeleteOrder(item);

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
        {
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Header.ToString().Equals("Выход")) //при нажатии выход - ПО закрывается
                this.Close();
        }

        private async void GetPrice_Click(object sender, RoutedEventArgs e)
        {
            PriceDownload price = new PriceDownload();
            price.Owner = this;
            price.ShowDialog();

            var controller = await this.ShowProgressAsync("Подождите...", "Идет загрузка прайслиста!");
            controller.SetIndeterminate();
            controller.SetProgress(.75);
            await controller.CloseAsync();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    EditRowDataGrid();
                }
            }
        } 

        //Раскрашиваем строки в зависимости от хуй пойми чего
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
                    DataRowView drv = row.DataContext as DataRowView;
                    var itemOrd =  new order( drv );
                    if (itemOrd != null)
                    {
                        /*var orig = odc.orders.GetOriginalEntityState(itemOrd);
                        if (orig == null)
                            this.odc.orders.Attach(itemOrd);*/
                        /*
                                 Background = "LightGray" Foreground = "Black" Content = "Запрос"    
                                 Foreground = "Black"                          Content = "В работе"
                                 Foreground = "Blue"                           Content = "Отправлено поставщику" 
                                 Foreground = "Green"                          Content = "Пришло в офис"
                                 Background = "LightGray" Foreground = "Green" Content = "Выдано" 
                                 Foreground = "Orange"                         Content = "Возврат" 
                                 Foreground = "Red"                            Content = "Отказ" 
                        */
                        switch (itemOrd.status)
                        {
                            case 0:
                                e.Row.Background = new SolidColorBrush(Colors.LightGray);
                                e.Row.Foreground = new SolidColorBrush(Colors.Black);
                                break;
                            case 1:
                                e.Row.Foreground = new SolidColorBrush(Colors.Black);
                                break;
                            case 2:
                                e.Row.Foreground = new SolidColorBrush(Colors.Blue);
                                break;
                            case 3:
                                e.Row.Foreground = new SolidColorBrush(Colors.Green);
                                break;
                            case 4:
                                e.Row.Background = new SolidColorBrush(Colors.LightGray);
                                e.Row.Foreground = new SolidColorBrush(Colors.Green);
                                break;
                            case 5:
                                e.Row.Foreground = new SolidColorBrush(Colors.Orange);
                                break;
                            case 6:
                                e.Row.Foreground = new SolidColorBrush(Colors.Red);
                                break;
                        }
                    }
                    break;
            }
        } 
        private void Get_Focus(object sender, EventArgs e)
        {
            TabControl tabs = new TabControl();
            TabItem item = new TabItem();

            tabs = (TabControl)sender;
            item = (TabItem)tabs.SelectedItem;
            active_tab_item = item.Name;
        }

        public void FilterPart_Click(object sender, RoutedEventArgs e)
        {
            var filter = "select p.producer, p.part_number, p.name, p.model, p.sup_price, p.ratio, p.count, p.code, s.name supplier " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.part_number like '%" + FilterTextBox.Text + "%'";

            DataView parts_list = ConnectToBase.ExecuteQuery(filter);

            this.PriceGrid.ItemsSource = parts_list;
            this.PriceGrid.Items.Refresh();
        }

        public void FilterCustomer_Click(object sender, RoutedEventArgs e)
        {
            var filter = "select p.name, p.phone, p.addres, p.price_level " +
                        "from dbo.Customers p " +
                        "where p.Name like '%" + FilterTextBoxCust.Text + "%' " +
                        "or p.phone like  '%" + FilterTextBoxCust.Text + "%' ";

            DataView parts_list = ConnectToBase.ExecuteQuery(filter);

            this.CustGrid.ItemsSource = parts_list;
            this.CustGrid.Items.Refresh();
        } 
    }
   
}
