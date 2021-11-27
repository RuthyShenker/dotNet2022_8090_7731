using IDal.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    /// <summary>
    /// A class that checks valids.
    /// </summary>
    public class CheckValids
    {
        public static int InputNumberValidity(string obj)
        {
            int number;
            while(!int.TryParse(Console.ReadLine(),out number))
            {
                Console.WriteLine(obj+" is not valid ,please enter again");
            }
            return number;
        }
        //public static int InputNumberOfPositions()
        //{
        //    int number;
        //    while (!int.TryParse(Console.ReadLine(), out number) || number<0)
        //    {
        //        Console.WriteLine("Number Of Positions is not valid ,please enter again");
        //    }
        //    return number;
        //}
        public static int InputIdCustomerValidity()
        {
            string number = Console.ReadLine();
            while (number.Length != 9||!number.All(c=>char.IsDigit(c)))
            {
                Console.WriteLine("Id Of Customer is not valid ,please enter again");
                number = Console.ReadLine();
            }
            return int.Parse(number);
        }

        public static string InputNameValidity()
        { 
            string name = Console.ReadLine();
            while (!name.All(ch => char.IsLetter(ch)))
            {
                Console.WriteLine("name must contains only letters ,please enter again");
                name = Console.ReadLine();
            }
            return name;
        }
       
        public static double InputDoubleValidity(string obj)
        {
            double number = 0;
            while (!double.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine(obj+" is not valid ,please enter again");
            }
            return number;
        }
        /// <summary>
        /// A function that gets range and requests for input
        /// and checks if the input is valid and in the range if not request again
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="input"></param>
        public static void CheckValid(int min, int max, out int input)
        {
            while (!int.TryParse(Console.ReadLine(), out input) || input < min || input > max)
            {
                Console.WriteLine("Invalid input!, please enter again");
            }
        }
        
        public static string InputPhoneValidity()
        {
            string phone = Console.ReadLine();
            while (!phone.All(ch => char.IsDigit(ch)))
            {
                Console.WriteLine("The phone is not valid , please enter again ");
                phone = Console.ReadLine();
            }
            return phone;
        }

        /// <summary>
        /// A function that gets an input and checks if it is in the range of 
        /// WeightCategories if it is not it asks for input again.
        /// </summary>
        /// <param name="input"></param>
        public static int InputValidWeightCategories()
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || input < 0 || input >= typeof(IBL.BO.WeightCategories).GetFields().Length)
            {
                Console.WriteLine("Weight is not valid !, please enter again");
            }
            return input;
        }
        public static int InputValidPriority()
        {
            int input = int.Parse(Console.ReadLine());
            while (!int.TryParse(Console.ReadLine(), out input) || input < 0 || input >= typeof(IBL.BO.Priority).GetFields().Length)
            {
                Console.WriteLine("Priority is not valid !, please enter again");
            }
            return input;
        }

        /// <summary>
        /// A function that gets an input and checks if it is in the range of 
        /// UrgencyStatuses if it is not it asks for input again.
        /// </summary>
        /// <param name="input"></param>
        public static void InputValidUrgencyStatuses(out int input)
        {
            bool check = int.TryParse(Console.ReadLine(), out input);
            while (!check || input < 0 || input > typeof(UrgencyStatuses).GetFields().Length)
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
        public static void InputValiDoubleNumWithRange(out double input, int max)
        {
            bool check = double.TryParse(Console.ReadLine(), out input);
            while (!check || input < 0 || input > max)
            {
                Console.WriteLine("Invalid input!, please enter again");
                check = double.TryParse(Console.ReadLine(), out input);
            }
        }
        // צריך להכניס מבנה שעה
        public static double InputValiDoubleNum()
        {
            double num;
            while (!double.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid input!, please enter again");
            }
            return num;
        }

    }
}
