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
        }
        public void AddingParcel(Parcel newParcel)
        {
            IDal.DO.Parcel parcel = new IDal.DO.Parcel()
            {
                SenderId = newParcel.Sender.Id,
                GetterId = newParcel.Getter.Id,
                Weight = newParcel.Weight,
                Status = (IDal.DO.UrgencyStatuses)newParcel.MPriority,
                DroneId = 0,
                MakingParcel = DateTime.Now,
                BelongParcel = null,
                PickingUp = null,
                Arrival = null
            };

            dal.GettingParcelForDelivery(parcel);
        }

        /// <summary>
        /// return a list of unbelong parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetUnbelongParcels()
        {
            IEnumerable<ParcelToList> bParcelList = new List<ParcelToList>();
            List<IDal.DO.Parcel> dParcelList = GetList<IDal.DO.Parcel>();
            //IEnumerable<IDal.DO.Parcel> dParcelList = dal.GetParcels();
            foreach (var parcel in dParcelList)
            {
                if (!parcel.belongParcel.HasValue)
                {
                    bParcelList.Add(MapToList(parcel));
                }
            }
            return bParcelList;
        }

        private ParcelToList MapToList(IDal.DO.Parcel parcel)
        {
            ParcelToList nParcel = new ParcelToList();
            nParcel.Id = parcel.Id;
            nParcel.SenderName = Extensions.GetById<Customer>(parcel.SenderId).Name;
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
            Parcel nParcel = new Parcel();
            nParcel.Id = parcel.Id;
            nParcel.Sender = NewCustomerInParcel(parcel.SenderId);
            nParcel.Getter = NewCustomerInParcel(parcel.GetterId);
            nParcel.Weight = parcel.Weight;
            nParcel.MPriority = parcel.Status;
            nParcel.DInParcel = NewDroneInParcel(parcel.DroneId);
            nParcel.MakingParcel =parcel.MakingParcel;
            nParcel.BelongParcel = parcel.BelongParcel;
            nParcel.PickingUp = parcel.PickingUp;
            nParcel.Arrival = parcel.Arrival ;
            return nParcel;
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


