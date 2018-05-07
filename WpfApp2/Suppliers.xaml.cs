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
    /// Interaction logic for Suppliers.xaml
    /// </summary>
    public partial class Suppliers : MetroWindow
    {
        SuppliersDataContext sdc = new SuppliersDataContext();
        private DataView supplier_list;
        private supplier returnSupplier;
        public Suppliers()
        {
            InitializeComponent();

            supplier_list = sdc.GetAllSuppliers();
            SupGrid.ItemsSource = supplier_list;
        }
        public void Supplier_Click( object sender, RoutedEventArgs e )
        {
            ChoseSup_Button();
        }
        private void Sup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ChoseSup_Button();
        }
        public void ChoseSup_Button()
        {
            DataRowView drv = SupGrid.SelectedItem as DataRowView;
            returnSupplier = new supplier(drv);
            this.Close();

        }
        public supplier GetSupplier()
        {
            return returnSupplier;
        }
    }
}
