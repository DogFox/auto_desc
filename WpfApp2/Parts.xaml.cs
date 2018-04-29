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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Parts.xaml
    /// </summary>
    public partial class Parts : MetroWindow
    {
        PartsDataContext pdc = new PartsDataContext();
        DataView parts_list; 
        private order add_to_order = new order();

        public Parts()
        {
            InitializeComponent();
        }
        public Parts( order order)
        {
            InitializeComponent();
            add_to_order = order;

            parts_list = pdc.GetAllParts();
            PartsGrid.ItemsSource = parts_list;
            PartsGrid.Items.Refresh();
        }
        public void ChosePart_Click( object sender, RoutedEventArgs e)
        {
            DataRowView drv = PartsGrid.SelectedItem as DataRowView;
            part returnPart = new part(drv);
            double price_level = 1;
            customer cust = customer.GetCustomer(this.add_to_order.cust_id);
            switch (cust.price_level)
            {
                case 1:
                    price_level = 1.2;
                    break;
                case 2:
                    price_level = 1.25;
                    break;
                case 3:
                    price_level = 1.4;
                    break;
            }

            string insert_part_to_order = "insert into dbo.parts_order " +
                                                          "(  producer, part_number, name, model, sup_price, price, ratio, count, code, sup_id, order_id )" +
                                                          "values( '" + returnPart.producer + "', '" + returnPart.part_number + "', '" + returnPart.name + "', '" +
                                                           returnPart.model + "', cast(replace('" + returnPart.sup_price + "',',','.') as float), " +
                                                           " Round( cast(replace('" + returnPart.sup_price + "',',','.') as float) * cast(replace('" + price_level + "',',','.') as float), 2), " +
                                                           returnPart.ratio + ", " + returnPart.count + ", '" +
                                                           returnPart.code + "', " + returnPart.sup_id + ", " + add_to_order.id + " ) ";

            DataView add_part_to_order = ConnectToBase.ExecuteQuery(insert_part_to_order);
            this.Close();
        }
         
        public void FilterPart_Click( object sender, RoutedEventArgs e)
        {
            var filter = "select p.id, p.producer, p.part_number, p.name, p.model, p.sup_price, p.ratio, p.count, p.code, s.name supplier, p.sup_id " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.part_number like '%" + FilterTextBox.Text + "%'";

            DataView parts_list = ConnectToBase.ExecuteQuery(filter);

            PartsGrid.ItemsSource = parts_list;
            PartsGrid.Items.Refresh();
        }
    }
}
