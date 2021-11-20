using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void DisplayingSpecificObjOption()
        {
            Console.WriteLine(
                         $"1 - Base station view\n" +
                         $"2 - Drone view\n" +
                         $"3 - Customer view\n" +
                         $"4 - Package view");
            int input;

            CheckValids.CheckValid(1, 4, out input);

            switch (input)
            {
                case 1:
                    Console.WriteLine(bL.GetStation(GettingId("Base Station")));
                    break;
                case 2:
                    Console.WriteLine(bL.GetDrone(GettingId("Drone")));
                    break;
                case 3:
                    Console.WriteLine(bL.GetCustomer(GettingIdAsString("Customer")));
                    break;
                case 4:
                    Console.WriteLine(bL.GetParcel(GettingId("Parcel")));
                    break;
                default:
                    break;

            }
        }
    }
}
