using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {/// <summary>
     /// A class that checks valids.
     /// </summary>
        public class CheckValids
        {
            /// <summary>
            /// A function that gets range and requests for input
            /// and checks if the input is valid and in the range if not request again
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <param name="input"></param>
            public static void CheckValid(int min, int max, out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < min || input > max)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }
            /// <summary>
            /// A function that gets an input and checks if it is in the range of 
            /// WeightCategories if it is not it asks for input again.
            /// </summary>
            /// <param name="input"></param>
            public static void InputValidWeightCategories(out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < 0 || input > Enum.GetNames(typeof(WeightCategories)).Length)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }

            /// <summary>
            /// A function that gets an input and checks if it is in the range of 
            /// UrgencyStatuses if it is not it asks for input again.
            /// </summary>
            /// <param name="input"></param>
            public static void InputValidUrgencyStatuses(out int input)
            {
                bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < 0 || input > Enum.GetNames(typeof(UrgencyStatuses)).Length)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }

            /// <summary>
            /// A function that gets a max value and gets from the user an input
            /// and checks if the input is smallest than the max and 
            /// if it isn't valid it asks for input again.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="max"></param>
            public static void InputValiDoubleNum(out double input, int max)
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