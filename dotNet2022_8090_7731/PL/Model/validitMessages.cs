﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class ValidityMessages
    {
        public static string IntMessage(object value, int length = 0)
        {
            int maxLength = (int)Math.Pow(10, length);
            return value switch
            {
                null => "Feild is required",
                string => "Input must contain digits only",
                > 10000 or < 1000 => "Input must contain 4 digits",
                _ => "",
            };
        }
        public static string StringMessage(string value)
        {
            return (value == "" || value == null) ? "Feild is required" :
                   !value.All(c => char.IsLetter(c)) ? "Name has to contain letters only" :
                   "";
        }

        public static string LocationMessage(object value, int min = 0, int max = 0)
        {
            return value == null ? "Feild is required" :
                value is string ? "Input must contain digits only" :
                (double)value > max ? $"Max value is {max}" :
                (double)value < min ? $"Min value is {min}" :
                "";
        }
    }
}