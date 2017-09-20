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
    /// 
    public class OrderInfoView
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public float SupPriice { get; set; }
        public string PartNum { get; set; }
        public string Supplier { get; set; }
    }
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
            OrderPartsDataContext opdc = new OrderPartsDataContext();
            PartsDataContext pdc = new PartsDataContext();
            SuppliersDataContext sdc = new SuppliersDataContext();


            IEnumerable< OrderInfoView> orderParts_list;
            orderParts_list = ( from t1 in opdc.part_orders
                                join t2 in pdc.parts on t1.part_id equals t2.id
                                join t3 in sdc.suppliers on t2.sup_id equals t3.id
                                select new OrderInfoView()
                                {
                                    PartNum = t2.part_number,
                                    Price = t2.price,
                                    Name = t2.name,
                                    SupPriice = t2.sup_price,
                                    Supplier = t3.name
                                });

        }
        private void OrderPartsGrid_Loaded( object sender, RoutedEventArgs e)
        { }
    }
}
