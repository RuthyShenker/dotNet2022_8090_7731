using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// A class of Tools that contains a function 
    /// ToStringProperty that instead ToString.
    /// </summary>
    public static class Tools
    {
        public static string ToString<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                str += item.Name + ":" + item.GetValue(t, null);
            }
            return str;
        }
    }
}
