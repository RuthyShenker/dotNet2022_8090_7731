using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
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
                    IEnumerable<StationToList> StationList = bL.GetListToList<IDal.DO.BaseStation,StationToList>();
                    foreach (StationToList baseStation in StationList)
                    {
                        Console.WriteLine(Tools.ToString(baseStation));
                    }
                    break;
                case 2:
                    IEnumerable<DroneToList> DroneList = bL.GetListToList<IDal.DO.Drone, DroneToList>();
                    foreach (DroneToList drone in DroneList)
                    {
                        Console.WriteLine(Tools.ToString(drone));
                    }
                    break;
                case 3:
                    IEnumerable<CustomerToList> CustomerList =bL.GetListToList<IDal.DO.Customer, CustomerToList>();
                    foreach (CustomerToList customer in CustomerList)
                    {
                        Console.WriteLine(Tools.ToString(customer));
                    }
                    break;
                case 4:
                    IEnumerable<ParcelToList> ParcelList = bL.GetListToList<IDal.DO.Parcel, ParcelToList>();
                    foreach (ParcelToList parcel in ParcelList)
                    {
                        Console.WriteLine(Tools.ToString(parcel));
                    }
                    break;
                case 5:
                    IEnumerable<ParcelToList> UnbelongParcelsList = bL.GetUnbelongParcels();
                    foreach (ParcelToList parcel in UnbelongParcelsList)
                    {
                        Console.WriteLine(Tools.ToString(parcel));
                    }
                    break;
                case 6:
                    IEnumerable<StationToList> AvailableSlotsList = bL.AvailableSlots();
                    foreach (StationToList baseStation in AvailableSlotsList)
                    {
                        Console.WriteLine(Tools.ToString(baseStation));
                    }
                    break;
                default:
                    break;

            }
        }
    }
}
