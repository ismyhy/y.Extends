using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace y.Extends.SQL.SQLite
{
   public interface ISQLiteHelper
    {
        /// <summary>
        /// 方法实现之后需要执行 <see cref="WaitExcuteInitialize"/> 方法 来初始化数据
        /// </summary>
          void ExcuteInitialize();
    }  
}
