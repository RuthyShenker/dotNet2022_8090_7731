using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void UpdatingOption()
        {
            Console.WriteLine($"1 - Updating details of drone\n" +
                $"2 - Updating details of base station\n" +
                $"3 - Updating details of customer\n" +
                $"4 - sending drone to charging\n" + 
                $"5 - Relesing drone from charging " +
                $"6 - belonging a parcel to a drone" + 
                $"7 - PickingUpParcel by drone" + 
                $"8 - DeliveryPackage to the getter ");
            int input=0;
            CheckValids.CheckValid(1, 8, out input);
            int droneId = 0;
            switch (input)
            {
               
                case 1:
                    try
                    {
                        int newModel;
                        GetDetailsOfDrone(out droneId, out newModel);
                        bL.UpdatingDroneName(droneId, newModel);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case 2:
                    int stationId = 0;
                    string stationName;
                    //כמות עמדות טעינה כוללת 
                    int amountOfPositions=0;
                    GetDetailsOfStation(out stationId, out stationName, out amountOfPositions);
                    bL.UpdatingStationDetails(stationId,stationName,amountOfPositions);
                    break;
                case 3:
                    string customerId;
                    string newName;
                    string newPhone;
                    GetDetailsOfCustomer(out customerId,out newName,out newPhone);
                    bL.UpdatingCustomerDetails(customerId,newName,newPhone);
                    break;
                case 4:
                    bL.ChargingDrone(GettingId("Drone"));
                    break;
                case 5:
                    double timeInCharging = 0;
                    GetDetailsOfRelesingDroneFromCharging(out droneId,out timeInCharging);
                    bL.ReleasingDrone(droneId,timeInCharging);
                    break;
                case 6:
                    bL.BelongingParcel(GettingId("drone"));
                    break;
                case 7:
                    bL.PickingUpParcel(GettingId("Drone"));
                    break;
                case 8:
                    bL.DeliveryPackage(GettingId("Drone"));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// A function that Get Details Of Relesing Drone From Charging
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharging"></param>
        private static void GetDetailsOfRelesingDroneFromCharging(out int droneId, out double timeInCharging)
        {
            droneId = GettingId("Drone");
            Console.WriteLine("Enter the time in charging of the drone: ");
            timeInCharging = CheckValids.InputValiDoubleNum();
        }
        /// <summary>
        /// A function that Get Details Of new Customer and enters it to the data base
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        private static void GetDetailsOfCustomer(out string customerId,out string newName,out string newPhone)
        {
            Console.WriteLine("Enter the id of the customer: ");
            customerId = Console.ReadLine();
            Console.WriteLine("Enter the new name of the customer: ");
            newName = Console.ReadLine();
            Console.WriteLine("Enter the new phone of the customer: ");
            newPhone = Console.ReadLine();
        }
        /// <summary>
        /// A function that Get Details Of new Station and enters it to the data base
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="stationName"></param>
        /// <param name="amountOfPositions"></param>
        private static void GetDetailsOfStation(out int stationId, out string stationName, out int amountOfPositions)
        {
            Console.WriteLine("Enter the id of the station: ");
            stationId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the of the station: ");
            stationName = Console.ReadLine();
            Console.WriteLine("Enter the number of all the positions: ");
            amountOfPositions = int.Parse(Console.ReadLine());
        }
        /// <summary>
        /// A function that Get Details Of new Drone 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newModel"></param>
        private static void GetDetailsOfDrone(out int droneId,out int newModel)
        {
            droneId = GettingId("Drone");
            Console.WriteLine("Enter the new model of the drone: ");
            newModel = int.Parse(Console.ReadLine());
        }
    }
}
