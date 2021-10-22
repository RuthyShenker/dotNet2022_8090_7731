using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        public class CheckValids
        {
            public static void CheckValid(int min, int max, out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < min || input > max)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }

            public static void InputValidWeightCategories(out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < 0 || input > Enum.GetNames(typeof(WeightCategories)).Length)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }
            public static void InputValidUrgencyStatuses(out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < 0 || input > Enum.GetNames(typeof(WeightCategories)).Length)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }
            public static void InputValiDoubleNum(out double input,int max)
            {
                bool check = double.TryParse(Console.ReadLine(), out input);
                while (!check || input < 0 || input > max)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = double.TryParse(Console.ReadLine(), out input);
                }
            }

            
        }
    }
}
