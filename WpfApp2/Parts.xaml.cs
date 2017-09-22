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
    /// Interaction logic for Parts.xaml
    /// </summary>
    public partial class Parts : Window
    {
        PartsDataContext pdc = new PartsDataContext();
        IEnumerable<part> parts_list;

        public Parts()
        {
            InitializeComponent();

            parts_list = pdc.GetAllParts();
            PartsGrid.ItemsSource = parts_list;
            PartsGrid.Items.Refresh();
        }

        public part ChosePart_Click2()
        {
            part returnPart = new part();
            returnPart = PartsGrid.SelectedItem as part;
            return returnPart;
        }

        public object ChosePart_Click
        {
            get { return PartsGrid.SelectedItem; }
        }
    }
}
