﻿using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBLParcel
    {
        int AddingParcel(Parcel parcel);
        void BelongingParcel(int pId);
        void PickingUpParcel(int dId);
        void DeliveryPackage(int Id);
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetUnbelongParcels();
        Parcel GetParcel(int parcelId);
    }
}