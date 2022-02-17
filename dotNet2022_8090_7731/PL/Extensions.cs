using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public static class Extensions
    {
        public static IEnumerable<PO.StationToList> MapListFromBLToPL(this IEnumerable<BO.StationToList> list)
        {
            return list.Select(s => new PO.StationToList(s));
        }

        public static IEnumerable<PO.DroneToList> MapListFromBLToPL(this IEnumerable<BO.DroneToList> list)
        {
            return list.Select(d=> new PO.DroneToList(d));
        }

        public static IEnumerable<PO.ParcelToList> MapListFromBLToPL(this IEnumerable<BO.ParcelToList> list)
        {
            return list.Select(p => new PO.ParcelToList(p));
        }

        public static IEnumerable<PO.CustomerToList> MapListFromBLToPL(this IEnumerable<BO.CustomerToList> list)
        {
            return list.Select(c => new PO.CustomerToList(c));
        }
    }
}
