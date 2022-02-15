﻿using BO;
using PL.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using static PL.Model.Enum;

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
       
        //public ListCollectionView parcelList;

        //PropertyGroupDescription groupDescription;
        //groupDescription.PropertyName = "Category";
        //listingDataView.GroupDescriptions.Add(groupDescription);



        //view.GroupDescriptions.Add(groupDescription);
        public ParcelListViewModel(BlApi.IBL bl)
        {
            Refresh.ParcelsList += RefreshParcelsList;

            this.bl = bl;
            ParcelList = new(bl.GetParcels().ToList());
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
            ParcelList = new(bl.GetParcels().ToList());

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

                    SortDescription sortDescription = new (groupBy.ToString(), ListSortDirection.Ascending);
                    parcelList.SortDescriptions.Add(sortDescription);
                }
                parcelList.SortDescriptions.Add(new ("Id", ListSortDirection.Ascending));
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
            ParcelToList parcel = obj as ParcelToList;
            return selectedFilter is null or "All" || parcel.Status.Equals(selectedFilter);
        }

        private void AddParcel(object obj)
        {
            if (!bl.GetCustomers().Any())
                MessageBox.Show("There are no customers in the system", "Failed Adding Parcel", MessageBoxButton.OK, MessageBoxImage.Stop);
            else
                new ParcelView(bl).Show();
        }

        private void EditParcel(object obj)
        {
            var parcel = obj as BO.ParcelToList;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, blParcel).Show();
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
