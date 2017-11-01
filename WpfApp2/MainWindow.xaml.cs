using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
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
    public class ConnectToBase
    {
        public string GetConnectionString()
        {
            return global::WpfApp2.Properties.Settings.Default.auto76ConnectionString;
        }

        public DataView ExecuteQuery(string sql)
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


    public partial class MainWindow : MetroWindow
    {
        bool isAdd = false;
        string active_tab_item = "Order"; // так как начинаем с окна заказов
        OrdersDataContext odc = new OrdersDataContext();
        CustomersDataContext cdc = new CustomersDataContext();
        SuppliersDataContext sdc = new SuppliersDataContext();
        PartsDataContext pdc = new PartsDataContext();
        private ConnectToBase bc = new ConnectToBase();

        private IEnumerable<customer> cust_list;
        private IEnumerable<order> order_list;
        private IEnumerable<supplier> supplier_list;
        private IEnumerable<part> part_list;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (o, args) => LogIn();

            //var items = dc.GetAllOrders();
            //this.DataContext = items;
            //this.OrdersView = (BindingListCollectionView)(CollectionViewSource.GetDefaultView(items));
            // Добавляем обработчик для всех кнопок на гриде
        }
        /*
         * Только админ может зайти и создать пользователей. 
         * ПОльзователи подключаются под своими логинами и используют ПО. Прав на регистрацию у пользователей нет
         * у любого пользовтаеля ПО права админа на SQL из под ПО. НАдо будет поправить.
         */
        private async void LogIn()
        {
            LoginDialogData result = await this.ShowLoginAsync("Authentication", "Enter your credentials",
                new LoginDialogSettings { ColorScheme = this.MetroDialogOptions.ColorScheme, InitialUsername = "MahApps" });



            

        }
        /*    private IEnumerable<DataView> GetOrdersList()
            {
                DataView orderParts_list = bc.ExecuteQuery("select p.name as part_name, part_number, sup_price, sup_price price, s.name " +
                                                            "from dbo.parts p " +
                                                            "join dbo.suppliers s on s.id = p.sup_id " +
                                                            "join dbo.part_order po on po.part_id = p.id " +
                                                            "where po.order_id = " + order.id);
            }
       */     // Инициализация датагридов при переключении вкладок
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            switch (active_tab_item)
            {
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
                case "Part":
                    part_list = pdc.GetAllParts();
                    PartsGrid.ItemsSource = part_list;
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
                case "Part":
                    this.pdc.SubmitChanges();
                    PartsGrid.Items.Refresh();
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

                case "Part":
                    part new_part = new part();

                    NewPart newPart = new NewPart();
                    newPart.Owner = this;
                    newPart.ShowDialog();

                    part_list = pdc.GetAllParts();
                    PartsGrid.ItemsSource = part_list;
                    PartsGrid.Items.Refresh();

                    break;
            }
        }

        private void Edit_Click( object sender, RoutedEventArgs e)
        {
            switch (active_tab_item)
            {
                case "Cust": 
                    break;

                case "Order":
                    order edit_order = OrderGrid.SelectedItem as order;

                    NewOrder newOrder = new NewOrder(edit_order);
                    newOrder.Owner = this;
                    newOrder.ShowDialog();

                    odc.SubmitChanges();
                    order_list = odc.GetAllOrders();
                    OrderGrid.ItemsSource = order_list;
                    OrderGrid.Items.Refresh();

                    break;

                case "Supl": 
                    break;

                case "Part": 
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
        {
            this.Close();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Header.ToString().Equals("Выход")) //при нажатии выход - ПО закрывается
                this.Close();
        }

        private void GetPrice_Click(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Header.ToString().Equals("Подкачать прайс")) //при нажатии выход - ПО закрывается
            {
                ExcelImport import_price = new ExcelImport();
                import_price.OpenClick(sender, e);
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
                    var itemOrd = row.DataContext as order;
                    if (itemOrd != null)
                    {
                        var orig = odc.orders.GetOriginalEntityState(itemOrd);
                        if (orig == null)
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

            DataView parts_list = bc.ExecuteQuery(filter);

            this.PartsGrid.ItemsSource = parts_list;
            this.PartsGrid.Items.Refresh();
        }

    }
   
}
