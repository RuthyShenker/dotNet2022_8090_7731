using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    partial class Program 
    {
        void AddingOption()
        {
            Console.WriteLine(
                                $"1 - Adding a base station to the stations list.\n" +
                                $"2 - Adding a drone to the drones list.\n" +
                                $"3 - Admission of a new customer to the customer list.\n" +
                                $"4 - Receipt of package for shipment.");
            CheckValids.CheckValid(1, 4, out input);
            switch (input)
            {
                case 1:
                    dalObject.Add(InputObj.GettingBaseStation());
                    break;
                case 2:
                    dalObject.Add(InputObj.GettingDrone());
                    break;
                case 3:
                    dalObject.Add(InputObj.GettingCustomer());
                    break;
                case 4:
                    dalObject.Add(InputObj.GettingParcel());
                    break;
                default:
                    break;

            }
        }
    }
    public static BaseStation GettingBaseStation()
    {

        int iInput;
        double dInput;
        Console.WriteLine("Enter The Id Of The Station:");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter The Name Of The Station:");
        string nameStation = Console.ReadLine();

        Console.WriteLine("Enter The Location. Longitude:");
        CheckValids.InputValiDoubleNum(out dInput, 90);
        double longitude = (double)dInput;

        Console.WriteLine("Latitude:");
        CheckValids.InputValiDoubleNum(out dInput, 180);
        double latitude = (double)dInput;

        Console.WriteLine("Enter The Number Of The Charging Stations:");
        CheckValids.CheckValid(1, 50, out iInput);
        int numberOfChargingStations = iInput;

        Location location = new Location(longitude, latitude);
        List<BL_ChargingDrone> lBL_ChargingDrone = new List<BL_ChargingDrone>();

        return new BL_Station(id, nameStation, location, numberOfChargingStations, lBL_ChargingDrone);
    }
}

