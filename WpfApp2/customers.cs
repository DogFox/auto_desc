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
    
    public partial class CustomersDataContext_Mod : CustomersDataContext
    {
        public new DataView GetAllCustomers()
        {
            DataView list = ConnectToBase.ExecuteQuery(@"select c.*
                                                            from dbo.customers c ");
            return list;
        }
    }
    public partial class customer : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public customer(DataRowView _cust )
        {
            this.name = _cust.Row["name"].ToString();
            this.id = Convert.ToInt32( _cust.Row["id"] );
            this.phone = _cust.Row["phone"].ToString();
            this.addres = _cust.Row["addres"].ToString();
            this.price_level = Convert.ToInt32( _cust.Row["price_level"] );
        }
        public static customer GetCustomer(int _id_cust)
        {
            var filter = "select p.id, p.name, p.phone, p.addres, p.price_level " +
                        "from dbo.Customers p " +
                        "where p.id = " + _id_cust;

            customer returnCustomer;
            DataView customers_list = ConnectToBase.ExecuteQuery(filter);
            if( customers_list.Count > 0 )
                returnCustomer = new customer(customers_list[0]);
            else
                throw new System.ArgumentException("Э бля, куда делся покупатель?", "customers_list");
            return returnCustomer;
        }
    }
}
