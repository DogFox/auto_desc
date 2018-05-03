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

    public partial class Order : order
    {
        
    }
    public partial class OrderPartsDataContext
    {
        public DataView GetAllOrderParts(order order)
        {
            DataView list = ConnectToBase.ExecuteQuery(@"select p.id, p.name as part_name, part_number, p.model, p.producer, sup_price, price, s.name, round(price-sup_price, 2 ) marge
                                                        from dbo.parts_order p 
                                                        join dbo.orders o on o.id = p.order_id and o.type = 1
                                                        join dbo.suppliers s on s.id = p.sup_id 
                                                        where p.order_id = " + order.id);
            return list;
        }
    }
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
                                                                 , case when o.status = 0 then 'Запрос'
                                                                        when o.status = 1 then 'В работе'
                                                                        when o.status = 2 then 'Отправлено поставщику'
                                                                        when o.status = 3 then 'Пришло в офис'
                                                                        when o.status = 4 then 'Выдано'
                                                                        when o.status = 5 then 'Возврат'
                                                                      else 'Отказ' end status_text
                                                                 , c.name, count(po.id) count, sum(po.price) price, sum( po.price - po.sup_price) marge
                                                            from dbo.orders o
                                                            left join dbo.customers c on c.id = o.cust_id
                                                            left join dbo.parts_order po on po.order_id = o.id
                                                            where o.type = 1
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
        public void SaveChangesInOrder(order order)
        {
            var str = "update dbo.orders set   number  = " + order.number +
                                        ", cust_id = " + order.cust_id +
                                     //   ((order.summ is null) ? "" : ", summ    = " + order.summ ) +
                                     //   ((order.count is null) ? "" : ", count   = " + order.count ) +
                                        ((order.comment == "" ) ? ", comment = '' " : ", comment = " + order.comment ) +
                                        ", status  = " + order.status +
                                        " where id = " + order.id;
            ConnectToBase.ExecuteQuery(str);
        }

        public void DeleteOrder( order order)
        {
            var str = @"update dbo.orders 
                        set type = 2
                        where id = " + order.id;

            ConnectToBase.ExecuteQuery(str);
        }

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

    }
}
