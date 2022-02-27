using PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    public static class Extensions
    {
        public static IEnumerable<PO.StationToList> MapListFromBLToPL(this IEnumerable<BO.StationToList> list)
        {
            return list.Select(s => new PO.StationToList()
            {
                Id = s.Id,
                Name = s.Name,
                AvailablePositions = s.AvailablePositions,
                FullPositions = s.FullPositions
            });
        }

        public static IEnumerable<PO.DroneToList> MapListFromBLToPL(this IEnumerable<BO.DroneToList> list)
        {
            return list.Select(d => new PO.DroneToList()
            {
                Id = d.Id,
                BatteryStatus = d.BatteryStatus,
                CurrLocation = new()
                {
                    Latitude = d.CurrLocation.Latitude,
                    Longitude = d.CurrLocation.Longitude
                },
                DeliveredParcelId = d.DeliveredParcelId,
                DStatus = (PO.DroneStatus)d.DStatus,
                Model = d.Model,
                Weight = (PO.WeightCategories)d.Weight
            });
        }

        public static IEnumerable<PO.ParcelToList> MapListFromBLToPL(this IEnumerable<BO.ParcelToList> list)
        {
            return list.Select(p => new PO.ParcelToList()
            {
                Id = p.Id,
                Weight = (PO.WeightCategories)p.Weight,
                GetterName = p.GetterName
                ,
                MyPriority = (PO.Priority)p.MyPriority,
                SenderName = p.SenderName,
                Status = (PO.ParcelStatus)p.Status
            });
        }

        public static IEnumerable<PO.CustomerToList> MapListFromBLToPL(this IEnumerable<BO.CustomerToList> list)
        {
            return list.Select(c => new PO.CustomerToList()
            {
                Id = c.Id,
                Got = c.Got,
                InWayToCustomer = c.InWayToCustomer,
                Name = c.Name,
                Phone = c.Phone,
                SentNotSupplied = c.SentNotSupplied,
                SentSupplied = c.SentSupplied
            });
        }

        public static bool WorkerTurnOn()
        {
            if (Refresh.Workers.Any(w => w.Value.IsBusy))
            {
                MessageBox.Show("Action inValid when auto state is turn on", "Auto State IsTurn On", MessageBoxButton.OK);
                return true;
            }
            return false;
        }

        // remove first word of string from exception
        public static void ShowIdExceptionMessage(string exception)
        {
            string messageForUser = exception[(exception.Split()[0].Length + 1)..];
            MessageBox.Show("messageForUser", "Wrong Id", MessageBoxButton.OK);
        }

        public static void ShowTheExceptionMessage(string exception, string windowHeader = "Error")
        {
            MessageBox.Show(exception, windowHeader, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowXMLExceptionMessage(string exception)
        {
            MessageBox.Show(exception, "Wrong Load Profram", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
