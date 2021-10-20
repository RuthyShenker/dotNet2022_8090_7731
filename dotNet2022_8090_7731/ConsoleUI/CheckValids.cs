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
            public static void CheckValid(int min, int max,out int input)
            {
               bool check = int.TryParse(Console.ReadLine(), out input);
                while (!check || input < min || input > max)
                {
                    Console.WriteLine("Invalid input!, please enter again");
                    check = int.TryParse(Console.ReadLine(), out input);
                }
            }
        }
    }
}
