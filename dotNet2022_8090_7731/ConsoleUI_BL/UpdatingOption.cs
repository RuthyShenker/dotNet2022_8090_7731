using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    partial class Program
    {
        void UpdatingOption()
        {
            Console.WriteLine(
                                $"1 - Belong a package to a drone\n" +
                                $"2 - Raising package by drone\n" +
                                $"3 - Delivery of a package to the destination\n" +
                                $"4 - Sending drone for charging at the base station\n" +
                                $"5 - Release drone from charging at the base station");
            int input;
            CheckValids.CheckValid(1, 5, out input);

            switch (input)
            {
                case 1:
                    dalObject.BelongingParcel(GettingId("Parcel"));
                    break;
                case 2:
                    dalObject.PickingUpParcel(GettingId("Parcel"));
                    break;
                case 3:
                    dalObject.DeliveryPackage(GettingId("Parcel"));
                    break;
                case 4:
                    dalObject.ChargingDrone(GettingId("Drone"));
                    break;
                case 5:
                    dalObject.ReleasingDrone(GettingId("Drone"));
                    input = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
