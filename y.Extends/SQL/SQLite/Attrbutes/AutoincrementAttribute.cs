using System.ComponentModel.DataAnnotations.Schema;

namespace y.Extends.SQL.SQLite.Attrbutes
{
    /// <inheritdoc />
    /// <summary>
    ///     设置主键是否自动递增
    /// </summary>
    public class AutoincrementAttribute : DatabaseGeneratedAttribute
    {
        public AutoincrementAttribute(bool auto = true) : base(auto ? DatabaseGeneratedOption.Identity : DatabaseGeneratedOption.None)
        {
        }
    }
}