

using PL.View;
using PO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using static PL.Extensions;

namespace PL.ViewModels
{
    public class ParcelListViewModel : INotify
    {
        private readonly BlApi.IBL bl;
        ListCollectionView parcelList;
        object selectedFilter { get; set; } = "All";
        //DateTime? startTime, endTime;
        GroupBy groupBy;

        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        public Array GroupOptions { get; set; } = Enum.GetValues(typeof(GroupBy));
        public IEnumerable FilterParcelOptions { get; } = new List<object>() { "All" }.Union(Enum.GetValues(typeof(BO.ParcelStatus)).Cast<object>());

        public ParcelListViewModel(BlApi.IBL bl)
        {
            Refresh.ParcelsList += RefreshParcelsList;

            this.bl = bl;
            try
            {
                ParcelList = new(bl.GetParcels().MapListFromBLToPL().ToList());
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);
            }
            ParcelList.Filter = FilterCondition;

            MouseDoubleCommand = new RelayCommand<object>(EditParcel);
            AddParcelCommand = new RelayCommand<object>(AddParcel);
            CloseWindowCommand = new RelayCommand<object>(Functions.CloseWindow);
        }

        public void FiterAfterDate(object choosenDate)
        {
            MessageBox.Show("Wow");
        }

        private void RefreshParcelsList()
        {
            try
            {
                ParcelList = new(bl.GetParcels().MapListFromBLToPL().ToList());
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                Extensions.ShowXMLExceptionMessage(exception.Message);
            }

            // keep group and filter status
            FilterParcels = selectedFilter;
            GroupBy = groupBy;
        }

        public GroupBy GroupBy
        {
            get => groupBy;
            set
            {
                groupBy = value;
                parcelList.GroupDescriptions.Clear();
                parcelList.SortDescriptions.Clear();
                if (groupBy != GroupBy.Id)
                {
                    PropertyGroupDescription groupDescription = new(groupBy.ToString());
                    parcelList.GroupDescriptions.Add(groupDescription);

                    SortDescription sortDescription = new(groupBy.ToString(), ListSortDirection.Ascending);
                    parcelList.SortDescriptions.Add(sortDescription);
                }
                parcelList.SortDescriptions.Add(new("Id", ListSortDirection.Ascending));
            }

        }

        //private string GroupByCurrentGroup()
        //{

        //    return groupBy switch
        //    {
        //        GroupBy.SenderName => nameof(ParcelToList.SenderName),
        //        GroupBy.GetterName => nameof(ParcelToList.GetterName),
        //        GroupBy.Id => "",
        //        _ => null,
        //    };
        //}
        //private GroupDescription MyGroup(CollectionViewGroup group, int level)
        //{
        //    .Add(new PropertyGroupDescription(nameof(ParcelToList.SenderName)));
        //}

        public object FilterParcels
        {
            get => selectedFilter;
            set
            {
                selectedFilter = value;
                ParcelList.Filter = FilterCondition;
            }
        }

        private bool FilterCondition(object obj)
        {
            PO.ParcelToList parcel = obj as PO.ParcelToList;
            return selectedFilter is null or "All" || parcel.Status.Equals((PO.ParcelStatus)selectedFilter);
        }

        private void AddParcel(object obj)
        {
            try
            {
                if (!bl.GetCustomers().Any())
                    MessageBox.Show("There are no customers in the system", "Failed Adding Parcel", MessageBoxButton.OK, MessageBoxImage.Stop);
                else
                    new ParcelView(bl).Show();
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                Extensions.ShowXMLExceptionMessage(exception.Message);
            }
        }

        private void EditParcel(object obj)
        {
            if (obj == null) return;

            var parcel = obj as PO.ParcelToList;
            try
            {
                var blParcel = bl.GetParcel(parcel.Id);
                new ParcelView(bl, blParcel).Show();
            }
            catch (BO.IdDoesNotExistException exception)
            {
                ShowIdExceptionMessage(exception.Message);
            }
            catch (BO.XMLFileLoadCreateException exception)
            {
                ShowXMLExceptionMessage(exception.Message);
            }
        }

        public ListCollectionView ParcelList
        {
            get => parcelList;
            set
            {
                parcelList = value;
                RaisePropertyChanged(nameof(ParcelList));
            }
        }
        //public ParcelStatus ParcelStatusSelected
        //{
        //    get => parcelStatusSelected;
        //    set
        //    {
        //        parcelStatusSelected = value;
        //        RefreshParcelList();
        //    }
        //}

        //public DateTime? StartTime
        //{
        //    get => startTime;
        //    set
        //    {
        //        startTime = value;
        //        Refresh.Invoke();
        //    }
        //}
        //public DateTime? EndTime
        //{
        //    get => endTime;
        //    set
        //    {

        //        endTime = value;
        //        Refresh.Invoke();
        //    }
        //}
        //public Dictionary<string, ObservableCollection<ParcelToList>> ParcelListBySender { get; set; }
    }

}
