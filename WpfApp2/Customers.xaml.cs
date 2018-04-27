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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Customers.xaml
    /// </summary>
    public partial class Customers : MetroWindow
    {
        private CustomersDataContext_Mod cdc = new CustomersDataContext_Mod();
        DataView cust_list; 
        private customer returnCustomer; 
        public Customers()
        {
            InitializeComponent();

            cust_list = cdc.GetAllCustomers();
            CustomersGrid.ItemsSource = cust_list;
            CustomersGrid.Items.Refresh();
        }
        public Customers( order order )
        {
            InitializeComponent();

            cust_list = cdc.GetAllCustomers();
            CustomersGrid.ItemsSource = cust_list;
            CustomersGrid.Items.Refresh();

            if( order.cust_id != 0 )
                returnCustomer = customer.GetCustomer(order.cust_id);
        }

        public void ChoseCustomer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = CustomersGrid.SelectedItem as DataRowView;
            returnCustomer = new customer(drv);

            this.Close();
        }

        public customer GetCustomer()
        {
            return returnCustomer;
        }
        public void FilterCustomer_Click(object sender, RoutedEventArgs e)
        {
            var filter = "select p.id, p.name, p.phone, p.addres, p.price_level " +
                        "from dbo.Customers p " +
                        "where p.Name like '%" + FilterTextBox.Text + "%' " +
                        "or p.phone like  '%" + FilterTextBox.Text + "%' ";

            DataView parts_list = ConnectToBase.ExecuteQuery(filter);

            CustomersGrid.ItemsSource = parts_list;
            CustomersGrid.Items.Refresh();
        }
    }
}
