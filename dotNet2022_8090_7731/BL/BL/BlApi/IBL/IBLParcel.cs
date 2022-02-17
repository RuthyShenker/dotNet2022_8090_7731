﻿
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    /// <summary>
    /// interface IBLParcel includes:
    /// AddingParcel
    /// BelongingParcel
    /// PickingUpParcel
    /// DeliveryPackage
    /// GetParcels
    /// GetUnbelongParcels
    /// GetParcel
    /// DeleteParcel
    /// </summary>
    public interface IBLParcel
    {
        //ADD:
        int AddingParcel(Parcel parcel);

        //UPDATE:
        void BelongingParcel(int dId);
        void PickingUpParcel(int dId);
        void DeliveryPackage(int dId);

        //GET:
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetUnbelongParcels();
        Parcel GetParcel(int parcelId);

        //DELETE:
        string DeleteParcel(int customerId);
    }
}
