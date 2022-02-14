using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// 
    public static class Tools
    {
        public static string ToStringProps<T>(this T obj)
        {
            StringBuilder myString = new();
            var type = obj.GetType();
            myString.Append($" <{SeparateByUpperCase(type.Name)}> {{ ");
            foreach (PropertyInfo item in type.GetProperties())
            {
                myString.Append($"{SeparateByUpperCase(item.Name)}: {item.GetValue(obj)}, ");
            }
            string result = myString.ToString()+" }";
            return result;
        }

        public static string SeparateByUpperCase(string str)
        {
            var sb = new StringBuilder();
            char previousChar = char.MinValue;
            foreach (char c in str)
            {
                // If not the first character and previous character is not a space, insert a space before uppercase
                if (char.IsUpper(c) && sb.Length != 0 && previousChar != ' ')
                {
                    sb.Append(' ');
                    sb.Append(char.ToLower(c));
                }
                else
                {
                    sb.Append(c);
                }
                previousChar = c;
            }
            return sb.ToString();
        }
    }
}
#region
//    string str = "";
//    str += typeof(T).Name+' ';
//            foreach (PropertyInfo item in t.GetType().GetProperties())
//            {
//                //if (item.PropertyType.Namespace!= typeof(int).Namespace)
//                //{
//                //    str += nameof(ToolboxItemFilterType)+' ';
//                //    str += item.PropertyType;
//                //    str += "     ";
//                //    str += item.PropertyType.DeclaringType;
//                //    foreach (var fieldInfo in item.PropertyType.GetFields())
//                //    {
//                //        str += fieldInfo.Name + ": " + fieldInfo.Attributes + "    ";
//                //    }
//                //    str += item.PropertyType.GetFields();
//                //    //foreach (PropertyInfo pItem in item.PropertyType.GetCustomAttributes())
//                //    //{
//                //    //    str += pItem.Name + ": " + pItem.GetValue(t, null)+' ';
//                //    //}
//                //}
//                //else
//                //{

//                str += item.Name + ": " + item.GetValue(t, null) +' ';
//                if (item.PropertyType!=null)
//                {
//                    var p = item.PropertyType.DeclaringType;


//}
//            }
//            return str;
//        }


//    }
//}
// StationId:1NameStation:lSLocation:IBL.BO.LocationNumAvailablePositions:1LBL_ChargingDrone:System.Collections.Generic.List`1[IBL.BO.ChargingDrone]
#endregion