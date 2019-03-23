using System.ComponentModel.DataAnnotations.Schema;

namespace y.Extends.SQL.SQLite.Attrbutes
{
    /// <inheritdoc />
    /// <summary>
    /// 标注该字段为非数据库列字段
    /// </summary>
    public class IgnoreAttribute : NotMappedAttribute
    {
    }
}