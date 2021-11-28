﻿using System;
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
    public static class Tools
    {
        public static string ToString<T>(this T t)
        {
           
            string str = "";
            str += typeof(T).Name+' ';
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                //if (item.PropertyType.Namespace!= typeof(int).Namespace)
                //{
                //    str += nameof(ToolboxItemFilterType)+' ';
                //    str += item.PropertyType;
                //    str += "     ";
                //    str += item.PropertyType.DeclaringType;
                //    foreach (var fieldInfo in item.PropertyType.GetFields())
                //    {
                //        str += fieldInfo.Name + ": " + fieldInfo.Attributes + "    ";
                //    }
                //    str += item.PropertyType.GetFields();
                //    //foreach (PropertyInfo pItem in item.PropertyType.GetCustomAttributes())
                //    //{
                //    //    str += pItem.Name + ": " + pItem.GetValue(t, null)+' ';
                //    //}
                //}
                //else
                //{

                str += item.Name + ": " + item.GetValue(t, null) +' ';
                
            }
            return str;
        }
    }
}
// StationId:1NameStation:lSLocation:IBL.BO.LocationNumAvailablePositions:1LBL_ChargingDrone:System.Collections.Generic.List`1[IBL.BO.ChargingDrone]