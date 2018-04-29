
using System.Data;
using System;
using System.ComponentModel;

namespace WpfApp2
{
    partial class PartsDataContext
    {
        public DataView GetAllParts()
        {
            var filter = "select p.id, p.producer, p.part_number, p.name, p.model, p.sup_price, p.ratio, p.count, p.code, s.name supplier, p.sup_id " +
                                                           "from dbo.parts p " +
                                                           "join dbo.suppliers s on s.id = p.sup_id";

            return ConnectToBase.ExecuteQuery(filter);

        }
    }
    public partial class part : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public part(DataRowView _part)
        {
            this.id = Convert.ToInt32(_part.Row["id"]);
            this.producer = _part.Row["producer"].ToString();
            this.part_number = _part.Row["part_number"].ToString();
            this.name = _part.Row["name"].ToString();
            this.model = _part.Row["model"].ToString();
            this.sup_price = Convert.ToDouble( _part.Row["sup_price"] );
            this.ratio = Convert.ToInt32(_part.Row["ratio"]);
            this.count = Convert.ToInt32(_part.Row["count"]);
            this.sup_id = Convert.ToInt32(_part.Row["sup_id"]);
        }
    }
}
