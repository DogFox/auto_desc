﻿using System;
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

            orderParts_list = ConnectToBase.ExecuteQuery("select p.name as part_name, part_number, sup_price, price, s.name " +
                                                        "from dbo.parts_order p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.order_id = " + new_order.id);


            new_order.number = odc.GetLastNumber();
            new_order.date = DateTime.Now.Date;
            new_order.status = 0;
            new_order.summ = 0;
            new_order.count = 0;
            new_order.comment = "";
            new_order.author = Session.Name;

            this.odc.orders.InsertOnSubmit(new_order);
            this.odc.SubmitChanges();

            OrderDate.SelectedDate = new_order.date;
            OrderStatus.Text = new_order.status.ToString();
            OrderNum.Text = new_order.number;

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }

        public NewOrder(order order)
        {
            InitializeComponent();

            DataView orderCust = ConnectToBase.ExecuteQuery("select name " +
                                                    "from dbo.Orders o " +
                                                    "join dbo.Customers c on c.id = o.cust_id " +
                                                    "where o.id = " + order.id);

            orderParts_list = ConnectToBase.ExecuteQuery("select p.id, p.name as part_name, part_number, p.model, p.producer, sup_price, price, s.name, round(price-sup_price, 2 ) marge " +
                                                        "from dbo.parts_order p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.order_id = " + order.id );

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
                        DataRowView row = (DataRowView) OrderPartsGrid.SelectedItem;
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
                ConnectToBase.ExecuteQuery("delete from dbo.parts_order where order_id = " + new_order.id);
                ConnectToBase.ExecuteQuery("delete from dbo.orders where id = " + new_order.id);
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


            DataView orderParts_list = ConnectToBase.ExecuteQuery("select p.name as part_name, part_number, sup_price, price, s.name " +
                                                        "from dbo.parts_order p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.order_id = " + order_to_send.id);

            OrderPartsGrid.ItemsSource = orderParts_list;
            OrderPartsGrid.Items.Refresh();
        }
        private void OrderPartsGrid_Loaded( object sender, RoutedEventArgs e)
        { }

        private void OrderPartsGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
