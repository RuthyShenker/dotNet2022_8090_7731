using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DO;
using static DalObject.DataSource;
using static DalObject.DataSource.Config;
namespace DalObject
{
    public class DalObject
    {
        
        public DalObject()
        {
            Initialize();
        }
        public void AddingBaseStation(BaseStation baseStation)
        {
            BaseStationArr[IndexBaseStationArr] = baseStation;
        }
        public void  AddingDrone(Drone drone)
        {
            DroneArr[IndexDroneArr] = drone;
        }
        public void AddingCustomer(Customer customer)
        {
            CustomerArr[IndexCustomerArr] = customer;
        }
        public void GettingParcelForDelivery(Parcel parcel)
        {
            ParcelArr[IndexParcelArr] = parcel;
        }
        public void AssigningParcelToDrone()
        {

        }
        public void CollectingParcelByDrone()
        {

        }
        public void DeliveryParcelTodestination()
        {

        }
        public void SendingDroneforChargingAtBaseStation()
        {

        }
        public void ReleasingDroneFromChargingAtBaseStation()
        {

        }
        public void BaseStationDisplay(int Id)
        {
            Console.WriteLine(BaseStationArr[Id]);
        }
        public void DroneDisplay(int Id)
        {

        }
        public void CustomerDisplay(int Id)
        {

        }
        public void ParcelDisplay(int Id)
        {

        }

        public void DisplayingListOfBaseStations()
        {

        }
        public void DisplayingListOfDrones()
        {

        }
        public void DisplayingListOfCustomers()
        {

        }
        public void DisplayingListOfParcels()
        {

        }
        public void DisplayingListOfParcelsNotYetAssociatedToDrone()
        {

        }
        public void DisplayingListOfBaseStationsWithAvailableChargingStation()
        {

        }

    }

}
