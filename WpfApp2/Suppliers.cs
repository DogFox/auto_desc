
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

namespace WpfApp2
{
    partial class SuppliersDataContext
    {
        public DataView GetAllSuppliers()
        {
            DataView supplier_list = ConnectToBase.ExecuteQuery(@"select s.id, s.name, s.phone, s.full_name, s.addres, s.kpp, s.inn, sum( isnull(po.sup_price, 0 )) summ
                                                                    from dbo.suppliers s
                                                                    left join ( dbo.parts_order po 
                                                                                join dbo.orders o on o.id = po.order_id and o.type = 1
                                                                                ) on po.sup_id = s.id
                                                                    group by s.id, s.name, s.phone, s.full_name, s.addres, s.kpp, s.inn");

            return supplier_list;
        }
        public void SaveChangesInSupplier(supplier sup)
        {
            var str = "update dbo.suppliers set   name  = '" + sup.name +
                                        "', full_name = '" + sup.full_name +
                                        "', phone = " + sup.phone +
                                        ", addres = '" + sup.addres +
                                        "', inn  = " + sup.inn +
                                        ", kpp  = " + sup.kpp +
                                        " where id = " + sup.id;
            ConnectToBase.ExecuteQuery(str);
        }
    }
    public partial class supplier : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public supplier(DataRowView _sup)
        {
            this.id = Convert.ToInt32(_sup.Row["id"]);
            this.name = _sup.Row["name"].ToString();
            this.phone = Convert.ToInt32(_sup.Row["phone"]);
            this.addres = _sup.Row["addres"].ToString();
            this.kpp = _sup.Row["kpp"].ToString();
            this.inn = _sup.Row["inn"].ToString();
            this.full_name = _sup.Row["full_name"].ToString();
        }
    }
}