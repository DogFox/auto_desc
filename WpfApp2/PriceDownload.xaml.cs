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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for PriceDownload.xaml
    /// </summary>
    public partial class PriceDownload : MetroWindow
    {
        private supplier chosed_supplier;
        private FileInfo file;
        public PriceDownload()
        {
            InitializeComponent();
        }
        public void FileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Файл Excel|*.XLSX;*.XLS;*.XLSM;*.CSV"
            };
            var result = openDialog.ShowDialog();
            file = new FileInfo(openDialog.FileName);
            File_Name.Text = file.Name;
            UpdateLayout();
        }
        public void SupButton_Click(object sender, RoutedEventArgs e)
        {
            Suppliers ChoseSup = new Suppliers();
            ChoseSup.ShowDialog();
            chosed_supplier = ChoseSup.GetSupplier();

            Supplier_Name.Text = chosed_supplier.name;
            UpdateLayout();
        } 
        public async void OkButton_Click(object sender, RoutedEventArgs e)
        { 
            var controller =  await this.ShowProgressAsync("Подождите...", "Идет загрузка прайслиста!");
            controller.SetIndeterminate();
            controller.SetProgress(.75);

            ExcelImport import = new ExcelImport();

            import.OpenClick(chosed_supplier, file);
            
            await controller.CloseAsync();
            this.Close();
        }
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}
