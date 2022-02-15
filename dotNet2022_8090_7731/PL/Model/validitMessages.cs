using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PO
{
    public class ValidityMessages
    {
        public static string IdMessage(object value, int length = 0) =>
            //int maxLength = (int)Math.Pow(10, length);
            value switch
            {
                null => "Field is required",
                string => "Input must contain digits only",
                > 10000 or < 1000 => "Input must contain 4 digits",
                _ => "",
            };
        public static string numPositionsMessage(object value, int length = 0)
        {
            //int maxLength = (int)Math.Pow(10, length);
            return value switch
            {
                null => "Field is required",
                string => "Input must contain digits only",
                > 100 or < 0 => "Input must be between 0 - 100",
                _ => "",
            };
        }
        

        public static string IdCustomerMessage(object value)
        {
            return value == null ? 
                        "Field is required" :
                !Regex.IsMatch(value.ToString(), @"^[0-9]+$") ?
                        $"Id has to contain digits only" :
                value.ToString().Length != 9 ?
                        "Id has to contain 9 digits":
                        "";
        }
      
        public static string StringMessage(string value, [CallerMemberName] string propertyName = "")
        {
            return !Regex.IsMatch(value, @"^[a-zA-Z\s]+$") ?
                        $"{propertyName} has to contain letters only" :
                   "";
        }
        

        public static string OnlyStringAndNumberMessage(string value, [CallerMemberName] string propertyName = "")
        {
            return !Regex.IsMatch(value, @"^[a-zA-Z0-9\s]+$") ?
                        $"{propertyName} has to contain letters and numbers only" :
                   "";
        }
        public static string NameMessage(string name)
       {
            return name == string.Empty ||name==null? 
                        "Field is required" :
                   !name.All(l => char.IsLetter(l)) ?
                        "Input must contain letters only" :
                   "";
        }
        public static string IntMessage(object value)
        {
            return value==null ?
                        "" :
                   !Regex.IsMatch(value.ToString(), @"^[0-9]+$") ?
                        "Name has to contain digits only" :
                   "";
        }
        public static string PhoneMessage(string value)
        {
            return value == string.Empty ||value==null? 
                        "Field is required" : 
                !value.All(d => char.IsDigit(d)) ?  
                        "Input must contain digits only" :
                 value.Length != 7?
                        "Phone must contain 10 digits" :
                    "";
            //return value switch
            //{
            //    null => "Field is required",
            //    !value.All(d => char.IsDigit(d)) => "Input must contain digits only",
            //    value.Length !=10  => "Phone must contain 10 digits",
            //    _ => "",
            //};
        }

        public static string RequiredMessage() => "Feild is required";

        public static string LocationMessage(object value, int min = 0, int max = 0)
        {
            //problem:!!!
            return value == null ? "Field is required" :/*^(\-*\s*[0-9]+\.[0-9]+)$*/
                !Regex.IsMatch(value.ToString(), @"^[0-9]+$") ? "Input must contain digits only" :
                (double)value > max ? $"Max value is {max}" :
                (double)value < min ? $"Min value is {min}" :
                "";
        }
    }
}
