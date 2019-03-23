using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace y.Extends.SQL
{
    public abstract class EntityBase : ICloneable, INotifyPropertyChanged
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CopyTo <T>(T target) where T : EntityBase, new()
        {
            var oldPropertyInfos = GetType().GetProperties();
            var propertyInfos = typeof (T).GetProperties();

            var list = propertyInfos.Intersect(oldPropertyInfos, new PropertyInfoComparer()).ToList();

            foreach (var property in list)
            {
                var value = property.GetValue(this, null); 
                property.SetValue(target, value);
            }
             
        }

        public T Copy <T>() where T : EntityBase, new()
        {
            var oldPropertyInfos = GetType().GetProperties();
            var propertyInfos = typeof (T).GetProperties();
            var target = new T();
            var list = propertyInfos.Intersect(oldPropertyInfos, new PropertyInfoComparer()).ToList();

            foreach (var property in list)
            {
                var value = property.GetValue(this, null);
                property.SetValue(target, value);
            }
            return target;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}