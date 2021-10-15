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
            static public  bool check = false;
            static public int input;
            static public string strInput;
            public static void CheckValid(int num1, int num2)
            {
                check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < num1 || input > num2)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }
            public static void CheckValid(int num)
            {
                strInput = Console.ReadLine();
                
                while (strInput.Length!=num)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    strInput = Console.ReadLine();
                }
            }
        }
    }
}
