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
    using System.Collections;

    public partial class OrdersDataContext_Mod : OrdersDataContext
    {
       /* public IEnumerable<order_view> GetCustomersForDataGrid(auto76DataSet db)
        {
            IEnumerable<order_view> ords = (from c in db.order_view
                                           select c).AsEnumerable();
            return ords;
        }*/
        public DataView GetAllOrders()
        {
            DataView list = ConnectToBase.ExecuteQuery(@"select o.id, o.number, o.cust_id , o.comment, o.status, o.date, o.author
                                                                 , c.name, count(po.id) count, sum(po.price) price, sum( po.price - po.sup_price) marge
                                                            from dbo.orders o
                                                            join dbo.customers c on c.id = o.cust_id
                                                            join dbo.parts_order po on po.order_id = o.id
                                                            group by o.id, o.number, o.cust_id, o.comment, o.status, o.date, o.author, c.name");
            return list;
        } 

        public string GetLastNumber()
        {
            string last_number = "0";

            DataView list = ConnectToBase.ExecuteQuery(@"select max( number ) number from dbo.orders");
            
            //last_number = all.Select(row => row.number).Max();
            /*if (all.Select(row => row.number).Max() == null)
                last_number = "1";
            else
                last_number = all.Select(row => row.number).Max();
                */
            return last_number;
        }

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

    }
}
