using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DalObject
{
    partial class Program
    {
        void DisplayingListOption()
        {
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
                    IEnumerable<BaseStation> StationList = dalObject.DisplayingBaseStations();
                    foreach (BaseStation baseStation in StationList)
                    {
                        Console.WriteLine(baseStation);
                    }
                    break;
                case 2:
                    IEnumerable<Drone> DroneList = dalObject.DisplayingDrones();
                    foreach (Drone drone in DroneList)
                    {
                        Console.WriteLine(drone);
                    }
                    break;
                case 3:
                    IEnumerable<Customer> CustomerList = dalObject.DisplayingCustomers();
                    foreach (Customer customer in CustomerList)
                    {
                        Console.WriteLine(customer);
                    }
                    break;
                case 4:
                    IEnumerable<Parcel> ParcelList = dalObject.DisplayingParcels();
                    foreach (Parcel parcel in ParcelList)
                    {
                        Console.WriteLine(parcel);
                    }
                    break;
                case 5:
                    IEnumerable<Parcel> UnbelongParcelsList = dalObject.DisplayingUnbelongParcels();
                    foreach (Parcel parcel in UnbelongParcelsList)
                    {
                        Console.WriteLine(parcel);
                    }
                    break;
                case 6:
                    IEnumerable<BaseStation> AvailableSlotsList = dalObject.AvailableSlots();
                    foreach (BaseStation baseStation in AvailableSlotsList)
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
