using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using IBL.BO;
using static ConsoleUI_BL.MEnum;

namespace ConsoleUI_BL
{
    partial class Program
    {
        private static void DisplayingListOption()
        {
            tools.PrintEnum(typeof(DisplayingList));
            
            CheckValids.CheckValid(1, 6, out int input);
            switch ((DisplayingList)input)
            {
                case DisplayingList.BaseStation:
                    IEnumerable<StationToList> StationList = bL.GetListToList<IDal.DO.BaseStation,StationToList>();
                    foreach (StationToList baseStation in StationList)
                    {
                        Console.WriteLine(Tools.ToStringProps(baseStation));
                    }
                    break;
                
                case DisplayingList.Drone:
                    IEnumerable<DroneToList> DroneList = bL.GetListToList<IDal.DO.Drone, DroneToList>();
                    foreach (DroneToList drone in DroneList)
                    {
                        Console.WriteLine(Tools.ToStringProps(drone));
                    }
                    break;
                
                case DisplayingList.Customer:
                    IEnumerable<CustomerToList> CustomerList =bL.GetListToList<IDal.DO.Customer, CustomerToList>();
                    foreach (CustomerToList customer in CustomerList)
                    {
                        Console.WriteLine(Tools.ToStringProps(customer));
                    }
                    break;
             
                case DisplayingList.Parcel:
                    IEnumerable<ParcelToList> ParcelList = bL.GetListToList<IDal.DO.Parcel, ParcelToList>();
                    foreach (ParcelToList parcel in ParcelList)
                    {
                        Console.WriteLine(Tools.ToStringProps(parcel));
                    }
                    break;
               
                case DisplayingList.PackageWhichArentBelongToDrone:
                    IEnumerable<ParcelToList> UnbelongParcelsList = bL.GetUnbelongParcels();
                    foreach (ParcelToList parcel in UnbelongParcelsList)
                    {
                        Console.WriteLine(Tools.ToStringProps(parcel));
                    }
                    break;
                
                case DisplayingList.StationsWithAvailablePositions:
                    IEnumerable<StationToList> AvailableSlotsList = bL.GetListToList<IDal.DO.BaseStation, StationToList>(baseStation => dal.AreThereFreePositions(baseStation.Id));
                    foreach (StationToList baseStation in AvailableSlotsList)
                    {
                        Console.WriteLine(Tools.ToStringProps(baseStation));
                    }
                    break;
                
                default:
                    break;
            }
        }
    }
}
