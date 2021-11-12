using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal
{
    partial class Program
    {
        void DisplayingSPecificObjOption()
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
                    BaseStation baseStation = BL.BaseStationDisplay(GettingId("Base Station"));
                    Console.WriteLine(baseStation);
                    break;
                case 2:
                    Drone drone = dalObject.DroneDisplay(GettingId("Drone"));
                    Console.WriteLine(drone);
                    break;
                case 3:
                    Customer customer = dalObject.CustomerDisplay(GettingIdAsString("Customer"));
                    Console.WriteLine(customer);
                    break;
                case 4:
                    Parcel parcel = dalObject.ParcelDisplay(GettingId("Parcel"));
                    Console.WriteLine(parcel);
                    break;
                default:
                    break;

            }
        }
    }
}
