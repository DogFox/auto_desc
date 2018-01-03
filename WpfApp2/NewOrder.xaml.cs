using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
    public partial class NewOrder : MetroWindow
    {
        private order new_order = new order();
        private order order = new order();
        private DataView orderParts_list;
        private OrdersDataContext_Mod odc = new OrdersDataContext_Mod();
        private OrderPartsDataContext opdc = new OrderPartsDataContext();
        private PartsDataContext pdc = new PartsDataContext();
        private SuppliersDataContext sdc = new SuppliersDataContext();
        private ConnectToBase bc = new ConnectToBase();
        int isAdd = 0;


        public NewOrder()
        {
            InitializeComponent();
            isAdd = 1;

            DataView orderCust = bc.ExecuteQuery("select name " +
                                                    "from dbo.Orders o " +
                                                    "join dbo.Customers c on c.id = o.cust_id " +
                                                    "where o.id = " + order.id);

            orderParts_list = bc.ExecuteQuery("select p.name as part_name, part_number, sup_price, sup_price price, s.name " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "join dbo.part_order po on po.part_id = p.id " +
                                                        "where po.order_id = " + new_order.id);


            new_order.number = odc.GetLastNumber();
            new_order.date = DateTime.Now.Date;
            new_order.status = 0;
            new_order.summ = 0;
            new_order.count = 0;
            new_order.comment = "";
            //new_order.autor_id = Session.id;

            this.odc.orders.InsertOnSubmit(new_order);
            this.odc.SubmitChanges();

            OrderDate.SelectedDate = new_order.date;
            OrderStatus.Text = new_order.status.ToString();
            OrderNum.Text = new_order.number;

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }

        public NewOrder( order order)
        {
            InitializeComponent();

            DataView orderCust = bc.ExecuteQuery("select name " +
                                                    "from dbo.Orders o " +
                                                    "join dbo.Customers c on c.id = o.cust_id " +
                                                    "where o.id = " + order.id);

            orderParts_list = bc.ExecuteQuery("select p.name as part_name, part_number, sup_price, sup_price price, s.name " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "join dbo.part_order po on po.part_id = p.id "+
                                                        "where po.order_id = " + order.id );

            OrderDate.SelectedDate = order.date;
            OrderStatus.Text = order.status.ToString();
            OrderNum.Text = order.number;
            OrderComment.Text = order.comment;
            if( orderCust.Count > 0 )
                OrderCustomer.Text = orderCust[0].Row["name"].ToString();
            this.order = order; //передали входящий объект в объект класса

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }

        private void Apply_Click( object sender, RoutedEventArgs e)
        {
            double summOrder = 0;
            int countOrder = 0;
            foreach (DataRowView drv in orderParts_list)
            {
                summOrder += Convert.ToDouble(drv.Row["price"]);
                countOrder++;
            }


            if (isAdd.Equals(1))
            {
                new_order.number = OrderNum.Text;
                new_order.status = Convert.ToInt32(OrderStatus.Text);
                new_order.comment = OrderComment.Text;
                new_order.date = Convert.ToDateTime(OrderDate.Text);
                new_order.summ = summOrder;
                new_order.count = countOrder;
                //new_order.summ = 120;
                //new_order.count = 1;
                //new_order.cust_id = 1;
            }
            else
            {
                order.number = OrderNum.Text;
                order.status = Convert.ToInt32(OrderStatus.Text);
                order.comment = OrderComment.Text;
                order.date = Convert.ToDateTime(OrderDate.Text);
                order.summ = summOrder;
                order.count = countOrder;

                //order.summ = 120;
                //order.count = 1;
                //order.cust_id = 1;
            }
            odc.SubmitChanges();

            this.Close();
        }

        private void OrderCustomerChoose_Click( object sender, RoutedEventArgs e)
        {
            Customers ChoseCust = new Customers();
            ChoseCust.ShowDialog();
            customer addCust = ChoseCust.GetCustomer();

            if (isAdd.Equals(1))
                new_order.cust_id = addCust.id;
            else
                order.cust_id = addCust.id;

            OrderCustomer.Text = addCust.name;
            odc.SubmitChanges();

            UpdateLayout();
        }



        private void Cancel_Click( object sender, RoutedEventArgs e)
        {
            if( isAdd.Equals(1) )
            {
                bc.ExecuteQuery("delete from dbo.part_order where order_id = " + new_order.id);
                bc.ExecuteQuery("delete from dbo.orders where id = " + new_order.id);
            }
            this.Close();
        }

        private void DeletePart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            order order_to_send = new order();
            if( isAdd.Equals( 1 ))
            {
                order_to_send = new_order;
            }
            else
            {
                order_to_send = order;
            }
            Parts AddPartWin = new Parts(order_to_send);
            AddPartWin.ShowDialog();


            DataView orderParts_list = bc.ExecuteQuery("select p.name as part_name, part_number, sup_price, sup_price price, s.name " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "join dbo.part_order po on po.part_id = p.id " +
                                                        "where po.order_id = " + order_to_send.id);

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }
        private void OrderPartsGrid_Loaded( object sender, RoutedEventArgs e)
        { }
    }
}
