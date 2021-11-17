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
        IEnumerable<ParcelToList> GetParcels()
        {
            IEnumerable<ParcelToList> lparcelToLists = new List<ParcelToList>();
            IEnumerable<IDal.DO.Parcel> parcels = dal.GetParcels();
        }

    }
}
