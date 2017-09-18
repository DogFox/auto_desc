namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
    }
}
