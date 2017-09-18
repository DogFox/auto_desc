namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class customers
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public string phone { get; set; }

        public string addres { get; set; }

        public int? price_level { get; set; }
    }
}
