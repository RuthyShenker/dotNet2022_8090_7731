using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    public static class tools
    {
        public static void PrintEnum(Type typeEnum)
        {
            foreach (var option in Enum.GetValues(typeEnum))
            {
                Console.WriteLine($"\t{(int)option} - {SeparateStringByUpperCase(option.ToString())}");
            }
        }

        public static void PrintMain(Type typeEnum)
        {
            Console.WriteLine("------------------------------------------------");
            foreach (var option in Enum.GetValues(typeEnum))
            {
                Console.WriteLine($"  {(int)option} - {SeparateStringByUpperCase(option.ToString())}");
            }
            Console.WriteLine("------------------------------------------------");
        }
       
        public static string SeparateStringByUpperCase(string str)
        {
            var sb = new StringBuilder();
            char previousChar = char.MinValue; 
            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    // If not the first character and previous character is not a space, insert a space before uppercase
                    if (sb.Length != 0 && previousChar != ' ')
                    {
                        sb.Append(' ');
                    }
                }
                sb.Append(c);
                previousChar = c;
            }
            return sb.ToString();
        }
    }
    
}
