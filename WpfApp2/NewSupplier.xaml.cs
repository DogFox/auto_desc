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
using MahApps.Metro.Controls.Dialogs;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for NewSupplier.xaml
    /// </summary>
    public partial class NewSupplier : MetroWindow
    {
        private supplier sup_to_send;
        private SuppliersDataContext sdc = new SuppliersDataContext();
        public NewSupplier()
        {
            InitializeComponent();
        }
        public NewSupplier(supplier sup)
        {
            InitializeComponent();

            name.Text = sup.name;
            phone.Text = sup.phone.ToString();
            full_name.Text = sup.full_name;
            addres.Text = sup.addres;
            inn.Text = sup.inn;
            kpp.Text = sup.kpp;

            this.sup_to_send = sup;

        }
        public void Apply_Click( object sender, RoutedEventArgs e)
        {
            sup_to_send.name = name.Text;
            sup_to_send.phone = Convert.ToInt32(phone.Text);
            sup_to_send.full_name = full_name.Text;
            sup_to_send.inn = inn.Text;
            sup_to_send.kpp = kpp.Text;
            sup_to_send.addres = addres.Text;

            sdc.SaveChangesInSupplier(sup_to_send);

            this.Close();
        }
        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
