using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace PL.Converters
{
    /// <summary>
    /// A class HasAllTextConverter that impliments:IMultiValueConverter.
    /// </summary>
    public class HasAllTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// A function that 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = true;

            foreach (object val in values)
            {
                if (val is string)
                {
                    if (string.IsNullOrEmpty(val as string))
                    {
                        res = false;
                    }
                }
                else if (val == null)
                {
                    res = false;
                }
            }

            return res;
        }

        /// <summary>
        /// A function that converts back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
