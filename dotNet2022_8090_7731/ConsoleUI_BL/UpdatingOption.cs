
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleUI_BL.MEnum;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void UpdatingOption(BlApi.IBL bL)
        {
            tools.PrintEnum(typeof(Updating));
            int input = 0;
            CheckValids.CheckValid(1, 8, out input);
            switch ((Updating)input)
            {
                case Updating.DroneDetails:
                    int droneId = 0;
                    string newModel;
                    GetDetailsOfDrone(out droneId, out newModel);
                    bL.UpdatingDroneName(droneId, newModel);
                    break;
                case Updating.StationDetails:
                    int stationId = 0;
                    string stationName;
                    int amountOfPositions;
                    GetDetailsOfStation(out stationId, out stationName, out amountOfPositions);
                    bL.UpdatingStationDetails(stationId, stationName, amountOfPositions);
                    break;
                case Updating.CustomerDetails:
                    int customerId;
                    string newName;
                    string newPhone;
                    GetDetailsOfCustomer(out customerId, out newName, out newPhone);
                    bL.UpdatingCustomerDetails(customerId, newName, newPhone);
                    break;
                case Updating.SendingDroneToChargingPosition:
                    bL.SendingDroneToCharge(GettingId("Drone"));
                    break;
                case Updating.RealesingDroneFromChargingPosition:
                    double timeInCharging = 0;
                    GetDetailsOfRelesingDroneFromCharging(out droneId, out timeInCharging);
                    bL.ReleasingDrone(droneId);
                    break;
                case Updating.BelongingParcelToDrone: bL.BelongingParcel(GettingId("drone")); break;

                case Updating.PickingParcelByDrone: bL.PickingUpParcel(GettingId("Drone")); break;

                case Updating.DeliveryParcelToDestination: bL.DeliveryPackage(GettingId("Drone")); break;

                default: break;
            }
            #region MyRegion
            //    catch (UpdatingFailedIdNotExistsException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch(IdIsNotValidException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch (StationDoesntHaveAvailablePositionsException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch (ThereIsntEnoughBatteryToTheDroneException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch(ThereIsntEnoughBatteryToTheDrone exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch(BelongingParcelException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch (CantRelasingDroneFromChargingException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch (CantBelongingParcelToDroneException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch(ParcelsStatusIsntMatchException exception)
            //    {
            //        Console.WriteLine(exception.Message);

            //    }
            //    catch(NoParcelAssociatedToTheDroneException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch (ParcelIsAlreadyPickedUpException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //    catch(SendingDroneToChargeException exception)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            #endregion
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
            timeInCharging = CheckValids.InputDoubleValidity("Time");
        }

        /// <summary>
        /// A function that Get Details Of new Customer and enters it to the data base
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="newName"></param>
        /// <param name="newPhone"></param>
        private static void GetDetailsOfCustomer(out int customerId, out string newName, out string newPhone)
        {
            Console.WriteLine("Enter the id of the customer: ");
            customerId = CheckValids.InputIdCustomerValidity();
            Console.WriteLine("Enter the new name of the customer: ");
            newName = CheckValids.InputNameValidity();
            Console.WriteLine("Enter the new phone of the customer: ");
            newPhone = CheckValids.InputPhoneValidity();
        }

        /// <summary>
        /// A function that Get Details Of new Station and enters it to the data base
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="stationName"></param>
        /// <param name="amountOfPositions"></param>
        private static void GetDetailsOfStation(out int stationId, out string stationName, out int amountOfPositions)
        {
            Console.WriteLine("Enter the id of the base station: ");
            stationId = CheckValids.InputNumberValidity("id");
            Console.WriteLine("Enter the name of the of the station: ");
            stationName = CheckValids.InputNameValidity();
            Console.WriteLine("Enter the number of all the positions: ");
            amountOfPositions = CheckValids.InputNumberValidity("number of positions");
        }

        /// <summary>
        /// A function that Gets from the console id of drone and new model and dont rerurn
        /// them because they are sent to the function by out.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="newModel"></param>
        private static void GetDetailsOfDrone(out int droneId, out string newModel)
        {
            droneId = GettingId("Drone");
            Console.WriteLine("Enter the new model of the drone: ");
            //i dont check validity because i dont know what the style of the models of the drones.
            newModel = Console.ReadLine();
        }
    }
}
