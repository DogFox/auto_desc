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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        private order new_order = new order();
        private OrdersDataContext odc = new OrdersDataContext();

        public NewOrder()
        {
            InitializeComponent();

            new_order.number = odc.GetLastNumber();
            new_order.date = DateTime.Now.Date;
            new_order.status = 0;
            new_order.summ = 0;
            new_order.count = 0;
            new_order.comment = "";

            this.odc.orders.InsertOnSubmit(new_order);
            this.odc.SubmitChanges();

            OrderDate.SelectedDate = new_order.date;
            OrderStatus.Text = new_order.status.ToString();
            OrderNum.Text = new_order.number;
        }

        private void Apply_Click( object sender, RoutedEventArgs e)
        {
            new_order.number = OrderNum.Text;
            new_order.status = Convert.ToInt32(OrderStatus.Text);
            new_order.comment = OrderComment.Text;
            new_order.date = Convert.ToDateTime(OrderDate.Text);
            new_order.summ = 120;
            new_order.count = 1;
            new_order.cust_id = 1;

            this.odc.SubmitChanges();
            this.Close();
        }

        public order GerNewOrder
        {
            get { return new_order; }
        }

        private void Cancel_Click( object sender, RoutedEventArgs e)
        {

        }

        private void DeletePart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPart_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OrderPartsGrid_Loaded( object sender, RoutedEventArgs e)
        { }
    }
}
