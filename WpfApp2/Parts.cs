
using System.Data.Linq.Mapping;
using System.Data;

namespace WpfApp2
{
    partial class PartsDataContext
    {
        public DataView GetAllParts()
        {
            var filter = "select p.producer, p.part_number, p.name, p.model, p.sup_price, p.ratio, p.count, p.code, s.name supplier " +
                                                           "from dbo.parts p " +
                                                           "join dbo.suppliers s on s.id = p.sup_id";

            return ConnectToBase.ExecuteQuery(filter);
            
        }

    }
}