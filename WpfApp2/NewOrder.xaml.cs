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
    public partial class NewOrder : MetroWindow
    {
        private order new_order = new order();
        private order order = new order();
        private order order_to_send = new order();
        //private order_view order = new order_view();
        private DataView orderParts_list;
        private OrdersDataContext_Mod odc = new OrdersDataContext_Mod();
        private OrderPartsDataContext opdc = new OrderPartsDataContext();
        private PartsDataContext pdc = new PartsDataContext();
        private SuppliersDataContext sdc = new SuppliersDataContext(); 
        int isAdd = 0;


        public NewOrder()
        {
            InitializeComponent();
            isAdd = 1;

            orderParts_list = opdc.GetAllOrderParts(new_order); 

            new_order.number = odc.GetLastNumber();
            new_order.date = DateTime.Now.Date;
            new_order.status = 0;
            new_order.summ = 0;
            new_order.count = 0;
            new_order.comment = "";
            new_order.author = Session.Name;
            new_order.type = 1;

            this.odc.orders.InsertOnSubmit(new_order);
            this.odc.SubmitChanges();

            OrderDate.SelectedDate = new_order.date;
            OrderStatus.Text = new_order.status.ToString();
            OrderNum.Text = new_order.number;

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();

            order_to_send = new_order;
        }

        public NewOrder(order order)
        {
            InitializeComponent();

            DataView orderCust = ConnectToBase.ExecuteQuery("select name " +
                                                    "from dbo.Orders o " +
                                                    "join dbo.Customers c on c.id = o.cust_id " +
                                                    "where o.id = " + order.id);

            orderParts_list = opdc.GetAllOrderParts(order);

            OrderDate.SelectedDate = order.date;
            OrderStatus.Text = order.status.ToString();
            OrderNum.Text = order.number;
            OrderComment.Text = order.comment;
            if( orderCust.Count > 0 )
                OrderCustomer.Text = orderCust[0].Row["name"].ToString();

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();

            order_to_send = order;
        }
        private void OrderPartsGrid_DataGridCellEditEndingEventArgs(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "price")
                    {
                        DataRowView row = (DataRowView)OrderPartsGrid.SelectedItem;
                        // супер костыль, неприведи бог id будет не на первом месте в запросе
                        var row_id = row.Row.ItemArray[0];
                        var el = e.EditingElement as TextBox;
                        // rowIndex has the row index
                        // bindingPath has the column's binding
                        // el.Text has the new, user-entered value
                        ConnectToBase.ExecuteQuery("update dbo.parts_order set price = " + el.Text + " where id = " + row_id);
                    }
                }
            }
        }

        private void NewOrder_KeyDown(object sender, KeyEventArgs e)
        {
            // ... Test for Enter key.
            switch( e.Key )
            {
                case Key.Enter :
                    this.Apply_Click(sender, e);
                    break;
                case Key.Escape:
                    this.Cancel_Click(sender, e);
                    break;
            }
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

            order_to_send.number = OrderNum.Text;
            order_to_send.status = Convert.ToInt32(OrderStatus.SelectedIndex);
            order_to_send.comment = OrderComment.Text;
            order_to_send.date = Convert.ToDateTime(OrderDate.Text);
            order_to_send.summ = summOrder;
            order_to_send.count = countOrder;

            odc.SaveChangesInOrder(order_to_send);

            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (isAdd.Equals(1))
            {
                ConnectToBase.ExecuteQuery("delete from dbo.parts_order where order_id = " + order_to_send.id);
                ConnectToBase.ExecuteQuery("delete from dbo.orders where id = " + order_to_send.id);
            }
            this.Close();
        }

        private void OrderCustomerChoose_Click( object sender, RoutedEventArgs e)
        {
            Customers ChoseCust = new Customers(order_to_send);
            ChoseCust.ShowDialog();
            customer addCust = ChoseCust.GetCustomer();

            order_to_send.cust_id = addCust.id;

            OrderCustomer.Text = addCust.name;
            odc.SaveChangesInOrder(order_to_send);

            UpdateLayout();
        }

        

        private void DeletePart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            Parts AddPartWin = new Parts(order_to_send);
            AddPartWin.ShowDialog();


            orderParts_list = opdc.GetAllOrderParts(order_to_send);

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }
        private void OrderPartsGrid_Loaded( object sender, RoutedEventArgs e)
        { }

        private void OrderPartsGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
        }
        private void ComboBox_Init(object sender, RoutedEventArgs e)
        {
            OrderStatus.SelectedIndex = (int)order_to_send.status;
        }
    }
}
