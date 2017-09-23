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
    /// Interaction logic for NewPart.xaml
    /// </summary>
    public partial class NewPart : Window
    {
        private part new_part = new part();
        private PartsDataContext pdc = new PartsDataContext();

        public NewPart()
        {
            InitializeComponent();

            new_part.part_number = "";
            new_part.sup_price = 0;
            new_part.price = 0;
            new_part.sup_id = -1;

            this.pdc.parts.InsertOnSubmit(new_part);
            this.pdc.SubmitChanges();

            Supplier.Text = new_part.sup_id.ToString();
            Price.Text = new_part.price.ToString();
            SupPrice.Text = new_part.sup_price.ToString();
            PartNum.Text = new_part.part_number;
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            new_part.part_number = PartNum.Text;
            new_part.sup_price = Convert.ToSingle(SupPrice.Text);
            new_part.price = Convert.ToSingle(Price.Text);
            new_part.sup_id = Convert.ToInt32(Supplier.Text);

            this.pdc.SubmitChanges();
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
