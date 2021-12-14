using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.Tools;
namespace ConsoleUI_BL
{
    public static class tools
    {
        public static void PrintEnum(Type typeEnum)
        {
            foreach (var option in Enum.GetValues(typeEnum))
            {
                Console.WriteLine($"\t{(int)option} - {SeparateByUpperCase(option.ToString())}");
            }
        }

        public static void PrintMain(Type typeEnum)
        {
            Console.WriteLine("------------------------------------------------");
            foreach (var option in Enum.GetValues(typeEnum))
            {
                Console.WriteLine($"  {(int)option} - {SeparateByUpperCase(option.ToString())}");
            }
            Console.WriteLine("------------------------------------------------");
        }

        
    }

}
