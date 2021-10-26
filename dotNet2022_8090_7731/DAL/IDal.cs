using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    interface IDal
    {
        public void AddingBaseStation();
        public void AddingBaseStation(object baseStation);
        public void AddingDrone(object drone);
        public void AddingCustomer(object customer);
        public void GettingParcelForDelivery(object parcel);
        public void BelongingParcel(string pId);
        public void PickingUpParcel(string Id);
        public void DeliveryPackage(string Id);
        public void ChangeDroneStatus(string Id, object newStatus);
        public void ChargingDrone(string IdDrone);
        public void ReleasingDrone(string dId);
        public object BaseStationDisplay(string id);
        public object DroneDisplay(string Id);
        public object CustomerDisplay(string Id);
        public object ParcelDisplay(string Id);
        public IEnumerable<object> DisplayingBaseStations();
        public IEnumerable<object> DisplayingDrones();
        public IEnumerable<object> DisplayingCustomers();
        public IEnumerable<object> DisplayingParcels();
        public IEnumerable<object> DisplayingUnbelongParcels();
        public IEnumerable<object> AvailableSlots();
    }
}