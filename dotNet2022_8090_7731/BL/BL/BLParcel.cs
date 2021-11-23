using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    partial class BL
    {

       /* //יש גנרי
        public IEnumerable<ParcelToList> GetParcels()
        {
            IEnumerable<ParcelToList> bParcelList = new List<ParcelToList>();
            List<IDal.DO.Parcel> dParcelList = GetList<IDal.DO.Parcel>();
            //IEnumerable<IDal.DO.Parcel> dParcelList = dal.GetParcels();
            foreach (var parcel in dParcelList)
            {
                bParcelList.Add(MapToList(parcel));
            }
            return bParcelList;
        }*/

        public void AddingParcel(Parcel newParcel)
        {
            newParcel.MakingParcel = DateTime.Now;
            IDal.DO.Parcel parcel = new IDal.DO.Parcel()
            {   Id =dal.PickingUpAndReturnIndexParcel(),
                SenderId = newParcel.Sender.Id,
                GetterId = newParcel.Getter.Id,
                Weight = newParcel.Weight,
                Status = (IDal.DO.UrgencyStatuses)newParcel.MPriority,
                DroneId = 0,
                MakingParcel = DateTime.Now,
                BelongParcel =null,
                PickingUp = null,
                Arrival = null
            };

            dal.AddingParcel(parcel);
        }

        /// <summary>
        /// return a list of unbelong parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            var unbelongParcelsList = dal.GetUnbelongParcels();
            var bParcelList = new List<ParcelToList>();
            foreach (var parcel in unbelongParcelsList)
	        {
               bParcelList.Add(ConvertToList(parcel));
	        }
            return bParcelList;
        }

        private ParcelToList ConvertToList(IDal.DO.Parcel parcel)
        {
            ParcelToList nParcel = new ParcelToList();
            nParcel.Id = parcel.Id;
            nParcel.SenderName = dal.GetFromDalById<Customer>(parcel.SenderId).Name;
            nParcel.GetterName = Extensions.GetById<Customer>(parcel.GetterId).Name;
            //nParcel.SenderName = customerDalList.First(customer => customer.Id == parcel.SenderId).Name;
            //nParcel.GetterName = customerDalList.First(customer => customer.Id == parcel.GetterId).Name;
            nParcel.Weight = parcel.Weight;
            nParcel.MyPriority = parcel.Status;
            nParcel.Status = GetParcelStatus(parcel);
            return nParcel;
        }

        private Parcel convertToBL(IDal.DO.Parcel parcel)
        {
            var sender = NewCustomerInParcel(parcel.SenderId);
            var getter = NewCustomerInParcel(parcel.GetterId);
            var dInParcel = NewDroneInParcel(parcel.DroneId);
            return new Parcel( parcel.Id, sender, getter,  parcel.Weight, parcel.Status, dInParcel, parcel.MakingParcel,
                parcel.BelongParcel, parcel.PickingUp, parcel.Arrival);
        }

        private ParcelStatus GetParcelStatus(Parcel parcel)
        {
            if (parcel.Arrival.HasValue)
            {
                return ParcelStatus.InDestination;
            }
            if (parcel.PickingUp.HasValue)
            {
                return ParcelStatus.collected;
            }
            if (parcel.BelongParcel.HasValue)
            {
                return ParcelStatus.belonged;
            }
            return ParcelStatus.made;
        }

    }
}


