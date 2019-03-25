using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace y.Extends.SQL
{
    public abstract class EntityBase
    {
        public abstract object Clone();
    }

    public abstract class EntityBase <T> : EntityBase, INotifyPropertyChanged where T : new()
    {
        /// <summary>
        ///     浅复制
        /// </summary>
        public override object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        ///     深复制过程
        /// </summary>
        public void CopyTo(T target)
        {
            var oldPropertyInfos = GetType().GetProperties();
            var propertyInfos = typeof (T).GetProperties();

            //所有的属性信息
            var list = propertyInfos.Intersect(oldPropertyInfos, new PropertyInfoComparer()).ToList();

            //继承 EntityBase 的属性信息
            //var baseList = list.Where(p => p.PropertyType.IsSubclassOf(typeof (EntityBase))).ToList();

            //所有不继承 EntityBase 的属性信息
            //var notbaseList = list.Except(baseList).ToList();

            //所有不继承 EntityBase 的，但继承 IEnumerable 的属性信息
            //var baseList2 = list.Where(i=>i.PropertyType!=typeof(string)).Where(p => p.PropertyType.GetInterface("IEnumerable",true)==typeof(IEnumerable)).ToList();

            //所有不继承 EntityBase 的，也不继承 IEnumerable 的属性信息
            //notbaseList = notbaseList.Except(baseList2).ToList();
              
            foreach (var property in list)
            {
                var value = property.GetValue(this, null);
                if (value.GetType().IsSubclassOf(typeof (EntityBase)))
                {
                    value = ((EntityBase) value).Clone();
                }
                property.SetValue(target, value);
            }   

            //foreach (var property in baseList)
            //{     
            //    var bBase = (EntityBase) property.GetValue(this, null);

            //    property.SetValue(target, bBase.Clone());
            //}

            //return target;
        }

        /// <summary>
        ///     深复制过程
        /// </summary>
        public T Copy()
        {
            var target = new T();
            CopyTo(target);
            return target;
        }

        private class PropertyInfoComparer : IEqualityComparer <PropertyInfo>
        {
            public bool Equals(PropertyInfo x, PropertyInfo y)
            {
                return ReferenceEquals(x?.PropertyType, y?.PropertyType) && Equals(x?.Name, y?.Name);
            }

            public int GetHashCode(PropertyInfo obj)
            {
                return obj.PropertyType.GetHashCode() ^ obj.Name.GetHashCode();
            }
        }

        #region 通知

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}