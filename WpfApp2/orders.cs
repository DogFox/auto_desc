namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;


    public partial class orders
    {
        public int id { get; set; }

        [Required]
        public string number { get; set; }

        public int cust_id { get; set; }

        public double? summ { get; set; }

        public int? count { get; set; }

        public string comment { get; set; }

        public int? status { get; set; }

        public DateTime? date { get; set; }

        public string author { get; set; }
    }


    public partial class OrdersDataContext_Mod : OrdersDataContext
    {
        public IEnumerable<order> GetAllOrders()
        {
            var items = this.orders.Select(item => item).OrderBy(item => item.number);

            return items;
        }

        public string GetLastNumber()
        {
            string last_number = "0";


            var all = this.GetAllOrders();

            //last_number = all.Select(row => row.number).Max();
            if (all.Select(row => row.number).Max() == null)
                last_number = "1";
            else
                last_number = all.Select(row => row.number).Max();

            return last_number;
        }

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

    }
}
