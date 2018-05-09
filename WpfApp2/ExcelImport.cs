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
using System.Windows.Navigation;
//using Microsoft.Office.Interop.Excel;
using System.IO;
using Spire.Xls;
using System.Data;
using System.Globalization;
using System.Data.OleDb;
using System.Data.SqlClient;
using LumenWorks.Framework.IO.Csv;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public static class SqlBulkCopyExtensions
    {
        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, int sourceColumnOrdinal, int targetColumnOrdinal)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumnOrdinal, targetColumnOrdinal);
            sbc.ColumnMappings.Add(map);

            return map;
        }

        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, string sourceColumn, string targetColumn)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumn, targetColumn);
            sbc.ColumnMappings.Add(map);

            return map;
        }
    }

    public class ExcelImport
    { 
        public void OpenClick( supplier sup, FileInfo file )
        { 
            string name = file.FullName;
            PriceParams param = new PriceParams(sup);

            if (file.Extension != ".csv")
            {
                this.ImportDataFromExcel(name);
            }
            else
            {
                try
                {
                    this.ImportDataFromCSV(name, param );
                }
                catch
                {
                    MessageBox.Show("Прайс загружен с ошибками!" , "Error", MessageBoxButton.OK);
                }
            }

            ConnectToBase.ExecuteQuery(param.del_row);
            ConnectToBase.ExecuteQuery(param.ins_row);
        }
        public void ClearTable( string table )
        {
            string ssqlconnectionstring = ConnectToBase.GetConnectionString();
            //execute a query to erase any previous data from our destination table
            string sclearsql = "delete from " + table;
            SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
            sqlconn.Open();
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();
        }
        public void ImportDataFromCSV(string excelfilepath, PriceParams param)
        {
            string ssqltable = "dbo.parts_import_csv";
            this.ClearTable(ssqltable);

            TextReader textReader = new StreamReader(excelfilepath, Encoding.GetEncoding("windows-1251"));
            //CsvReader csvReader = new CsvReader(new StreamReader(excelfilepath), false, '\t', '"', '"', '#', LumenWorks.Framework.IO.Csv.ValueTrimmingOptions.QuotedOnly);
            Encoding enc = Encoding.GetEncoding(1251);
            var reader = new CsvReader(textReader, param.headers, param.delimeter, param.quote , param.escape, '#', LumenWorks.Framework.IO.Csv.ValueTrimmingOptions.QuotedOnly);
            using (reader)
            {

                reader.Columns = new List<LumenWorks.Framework.IO.Csv.Column>
                {
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c1", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c2", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c3", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c4", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c5", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c6", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c7", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c8", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c9", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c10", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c11", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c12", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c13", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c14", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c15", Type = typeof(string) }
                };

                // Now use SQL Bulk Copy to move the data
                using (var sbc = new SqlBulkCopy(ConnectToBase.GetConnectionString()))
                {
                    sbc.DestinationTableName = ssqltable;
                    sbc.BatchSize = 10000;
                    sbc.WriteToServer(reader);
                }
            }
        }

        public void ImportDataFromExcel(string excelfilepath)
        {
            FileInfo file = new FileInfo(excelfilepath);
            //declare variables - edit these based on your particular situation
            try
            {
            
                string sexcelconnectionstring;
                string myexceldataquery;
                myexceldataquery = "select * from [Sheet1$]";

                sexcelconnectionstring = @"provider=Microsoft.ACE.OLEDB.12.0;data source=" + excelfilepath +
                                                        ";extended Properties=\"Excel 12.0 Macro; HDR = Yes; IMEX = 1\"";

                string ssqltable = "dbo.parts_import";
                this.ClearTable(ssqltable);
                //series of commands to bulk copy data from the excel file into our sql table
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(ConnectToBase.GetConnectionString())
                {
                    DestinationTableName = ssqltable
                };
                while (dr.Read())
                {
                    bulkcopy.WriteToServer(dr);
                }
                dr.Close();
                oledbconn.Close();
            }
            catch 
            {
                //handle exception
            }
        }
    }

    public class PriceParams
    {
        public string del_row;
        public string ins_row;
        public bool headers;
        public char delimeter;
        public char quote;
        public char escape;

        public PriceParams(supplier sup)
        {
            headers = false;
            delimeter = ';';
            quote = '\0';
            escape = '\0';
            del_row = @"delete from dbo.parts
                                    where sup_id = (select top 1 id from dbo.suppliers
										                                    where id = " + sup.id + ")";

            switch (sup.kod.ToLower())
            {
                case "forum_msk"://Форум
                    headers = true;

                    ins_row = @" insert into dbo.parts
                                ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                            select c1, c2, c3, '', isnull( Try_convert(float, c4 ), 0 ) c4
					                                , isnull( Try_convert(int, c5 ), 0 ) c5
					                                , isnull( Try_convert(int, c6 ), 0 ) c6, c7, (select top 1 id from dbo.suppliers
										                            where id = " + sup.id + ") " +
                                        "from dbo.parts_import_csv";
                            break;
                case "mikado"://Микадо
                    ins_row = @" insert into dbo.parts
                                    ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                                select c3, c2, c4, '', isnull( Try_convert(float, c5 ), 0 ) c5
					                                    , isnull( Try_convert(int, c7 ), 0 ) c7
					                                    , isnull( Try_convert(int, c6 ), 0 ) c6, c1, (select top 1 id from dbo.suppliers
																	                                where id = " + sup.id + ") " +
                                        "from dbo.parts_import_csv";
                    break;

                case "shate"://Шате-М

                    headers = true;
                    ins_row = @" insert into dbo.parts
                                ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                            select c1, c2, c3, '', isnull( Try_convert(float, c7 ), 0 ) c7
					                                , isnull( Try_convert(int, c4 ), 0 ) c4
					                                , isnull( Try_convert(int, c5 ), 0 ) c5, c8, (select top 1 id from dbo.suppliers
																	                            where id = " + sup.id + ") " +
                                        "from dbo.parts_import_csv";
                    break;
                case "iksora"://Иксора
                    delimeter = ',';
                    escape = '"';
                    quote = '"';
                    ins_row = @" insert into dbo.parts
                                     ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                                    select c1, c2, c3, '', isnull( Try_convert(float, replace(c5 , ',', '.')), 0 ) c5
                                                           , isnull( Try_convert(int, c4 ), 0 ) c4 
                                                           , isnull( Try_convert(int, c6 ), 0 ) c6, c8, (select top 1 id from dbo.suppliers
																	                                    where id = " + sup.id + ") " +
                                      "from dbo.parts_import_csv";
                    break;
                case "rossko"://Иксора
                    headers = true;
                    escape = '\0';
                    quote = '\0';

                    ins_row = " insert into dbo.parts " +
                                   "  ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) " + 
                                 "   select replace(c2 , '\"', ''), replace(c11 , '\"', ''), replace(c4, '\"', '') " +
                                                    "  , '', isnull(Try_convert(float, replace(replace(c7, '\"', ''), ',', '.')), 0) " +
                                                    "   , isnull(Try_convert(int, replace(c9, '\"', '')), 0) " + 
                                                    "   , isnull(Try_convert(int, replace(c6, '\"', '')), 0) " +
                                                    "   , replace(c1, '\"', ''), (select top 1 id from dbo.suppliers " +
                                                                              " where id = " + sup.id + ") " +
                                      "from dbo.parts_import_csv";
                    break;
                    
            }
        }
    }
}
