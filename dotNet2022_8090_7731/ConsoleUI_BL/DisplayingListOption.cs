using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void DisplayingListOption()
        {
            int input = 0;
            Console.WriteLine(
                                $"1 - View base stations list\n" +
                                $"2 - View drones list\n" +
                                $"3 - View customers list\n" +
                                $"4 - View packages list\n" +
                                $"5 - View of packages which are unbelong to drones\n" +
                                $"6 - View base stations with available charging positions");
            CheckValids.CheckValid(1, 6, out input);

            switch (input)
            {
                case 1:
                    IEnumerable<StationToList> StationList = bL.GetBaseStations();
                    foreach (StationToList baseStation in StationList)
                    {
                        Console.WriteLine(baseStation);
                    }
                    break;
                case 2:
                    IEnumerable<DroneToList> DroneList = bL.GetDrones();
                    foreach (DroneToList drone in DroneList)
                    {
                        Console.WriteLine(drone);
                    }
                    break;
                case 3:
                    IEnumerable<CustomerToList> CustomerList = bL.GetCustomers();
                    foreach (CustomerToList customer in CustomerList)
                    {
                        Console.WriteLine(customer);
                    }
                    break;
                case 4:
                    IEnumerable<ParcelToList> ParcelList = bL.GetParcels();
                    foreach (ParcelToList parcel in ParcelList)
                    {
                        Console.WriteLine(parcel);
                    }
                    break;
                case 5:
                    IEnumerable<ParcelToList> UnbelongParcelsList = bL.GetUnbelongParcels();
                    foreach (ParcelToList parcel in UnbelongParcelsList)
                    {
                        Console.WriteLine(parcel);
                    }
                    break;
                case 6:
                    IEnumerable<StationToList> AvailableSlotsList = bL.AvailableSlots();
                    foreach (StationToList baseStation in AvailableSlotsList)
                    {
                        Console.WriteLine(baseStation);
                    }
                    break;
                default:
                    break;

            }
        }
    }
}
