using System.ComponentModel.DataAnnotations.Schema;

namespace y.Extends.SQL.SQLite.Attrbutes
{
    public class AutoincrementAttribute : DatabaseGeneratedAttribute
    { 
        public AutoincrementAttribute(bool auto=true):base(auto? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None)
        { 
        }
    }
}