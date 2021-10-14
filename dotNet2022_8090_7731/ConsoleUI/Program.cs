using System;
using DAL.DO;
namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine
                ($"Options:\n" +
                $"1 - Adding\n" +
                $"2 - Updating\n" +
                $"3 - Display\n" +
                $"4 - List view options\n" +
                $"5 - Exit");
            DalObject.DalObject dalObject = new DalObject.DalObject();
            CheckValids.CheckValid(1, 5);

            switch (CheckValids.input)
            {
                case 1:
                    Console.WriteLine(
                        $"1 - ● Add a base station to the list of stations.\n" +
                        $"2 - ● Add a drone to the list of existing drones.\n" +
                        $"3 - ● Admission of a new customer to the customer list.\n" +
                        $"4 - ● Receipt of package for shipment.");
                    CheckValids.CheckValid(1, 4);
                    switch (CheckValids.input)
                    {
                        case 1:
                            dalObject.AddingBaseStation();
                            break;
                        case 2:
                            dalObject.AddingDrone();
                            break;
                        case 3:
                            dalObject.AddingCustomer();
                            break;
                        case 4:

                            dalObject.GettingParcelForDelivery();
                            break;
                        default:
                            break;

                    }
                    break;


                case 2:
                    Console.WriteLine(
                        $"1 - ● Assigning a package to a skimmer\n" +
                        $"2 - ● Skimmer package assembly\n" +
                        $"3 - ● Delivery of a package to the destination\n" +
                        $"4 - ● Sending a skimmer for charging at a base station\n" +
                        $"5 - ● Release skimmer from charging at base station\n");
                    CheckValids.CheckValid(1, 5);

                    switch (CheckValids.input)
                    {
                        case 1:
                            dalObject.AssigningParcelToDrone();
                            break;
                        case 2:
                            dalObject.CollectingParcelByDrone();
                            break;
                        case 3:
                            dalObject.DeliveryParcelTodestination();
                            break;
                        case 4:
                            dalObject.SendingDroneforChargingAtBaseStation();
                            break;
                        case 5:
                            dalObject.ReleasingDroneFromChargingAtBaseStation();

                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    Console.WriteLine(
                        $"1 - ● Base station view\n" +
                        $"2 - ● Skimmer display\n" +
                        $"3 - ● Customer view\n" +
                        $"4 - ● Package view\n");
                    CheckValids.CheckValid(1, 4);

                    switch (CheckValids.input)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        default:
                            break;

                    }
                    break;
                case 4:
                    Console.WriteLine(
                        $"1 - ● Displays a list of base stations\n" +
                        $"2 - ● Displays the list of skimmers\n" +
                        $"3 - ● View the customer list\n" +
                        $"4 - ● Displays the list of packages\n" +
                        $"5 - ● Displays a list of packages that have not yet been assigned to the glider\n" +
                        $"6 - ● Display base stations with available charging stations\n");
                    CheckValids.CheckValid(1, 6);

                    switch (CheckValids.input)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        default:
                            break;

                    }
                    break;
                case 5:
                    
                    break;

                default:

                    break;
            }

        }


    }
}
