using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AfarsoftResourcePlan.Helper
{
    public class EntityHelper
    {
        public static T CopyValue<S, T>(S source, T target) where T : class where S : class
        {
            if (target == default(S))
            {
                return default(T);
            }
            Type stype = typeof(S);
            PropertyInfo[] spropertys = stype.GetProperties();
            Type ttype = typeof(T);
            PropertyInfo[] tpropertys = ttype.GetProperties();
            foreach (var item in spropertys)
            {
                var p = tpropertys.Where(e => e.Name == item.Name).FirstOrDefault();
                if (p != null)
                {
                    p.SetValue(target, item.GetValue(source));
                }
            }
            return target;
        }
        public static T CopyValue<S, T>(S source) where T : class, new() where S : class
        {
            T t = new T();
            return CopyValue<S, T>(source, t);
        }
    }
}
