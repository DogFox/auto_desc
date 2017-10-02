﻿using System;
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
        IEnumerable<object> parts_list;
        private ConnectToBase bc = new ConnectToBase();
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
            part returnPart = PartsGrid.SelectedItem as part;
            DataView add_part_to_order = bc.ExecuteQuery("insert into dbo.part_order " + 
                                                          "( order_id, part_id ) " + 
                                                          "values( " + add_to_order.id + ", " + returnPart.id + " ) ");
            this.Close();
        }

        public object ChosePart_Click2
        {
            get { return PartsGrid.SelectedItem; }
        }
        public void FilterPart_Click( object sender, RoutedEventArgs e)
        {
            var filter = "select p.producer, p.part_number, p.name, p.model, p.sup_price, p.ratio, p.count, p.code, s.name supplier " +
                                                        "from dbo.parts p " +
                                                        "join dbo.suppliers s on s.id = p.sup_id " +
                                                        "where p.part_number like '%" + FilterTextBox.Text + "%'";

            DataView parts_list = bc.ExecuteQuery(filter);

            PartsGrid.ItemsSource = parts_list;
            PartsGrid.Items.Refresh();
        }
    }
}
