using BO;
using PL.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using static PL.Model.Enum;

namespace PL.ViewModels
{
    public class ParcelListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        ListCollectionView parcelList;
        ParcelStatus parcelStatusSelected;
        DateTime? startTime, endTime;
        GroupBy groupBy;


        public Array GroupOptions { get; set; }
        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

        //public ListCollectionView parcelList;

        //PropertyGroupDescription groupDescription;
        //groupDescription.PropertyName = "Category";
        //listingDataView.GroupDescriptions.Add(groupDescription);



        //view.GroupDescriptions.Add(groupDescription);
        private void RefreshParcelList()
        {

        }
        public ParcelListViewModel(BlApi.IBL bl)
        {
            //var list1 = new ObservableCollection<Parcel>();
            //foreach (var parcel in bl.GetParcels())
            //    list1.Add(bl.GetParcel(parcel.Id));

            var list = new ObservableCollection<ParcelToList>(bl.GetParcels());
            ParcelList = new ListCollectionView(list);
            ParcelList.Filter = FilterParcel;

            ParcelList.SortDescriptions.Add(new SortDescription(groupBy.ToString(), ListSortDirection.Ascending));
            //ParcelList.GroupBySelector = MyGroup;
            //ParcelList.GroupDescriptions = MyGroup;
            this.bl = bl;
            GroupOptions = Enum.GetValues(typeof(GroupBy));
            //ParcelList.GroupDescriptions.Add(new PropertyGroupDescription(nameof(GroupBy)));

            //groupDescription = new PropertyGroupDescription();
            //ParcelList = bl.GetParcels();

            //ParcelListBySender = bl.GetParcels().GroupBy(parcel => parcel.SenderName)
            //    .ToDictionary(key => key.Key, value => new ObservableCollection<ParcelToList>(value));

            MouseDoubleCommand = new RelayCommand<object>(EditParcel);
            AddParcelCommand = new RelayCommand<object>(AddParcel);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
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
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(groupBy.ToString());
                    //groupDescription.PropertyName = groupBy.ToString();
                    parcelList.GroupDescriptions.Add(groupDescription);
                    SortDescription sortDescription = new SortDescription(groupBy.ToString(), ListSortDirection.Ascending);
                    parcelList.SortDescriptions.Add(sortDescription);
                }
            
                parcelList.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            }

        }

        private string GroupByCurrentGroup()
        {

            return groupBy switch
            {
                GroupBy.SenderName => nameof(ParcelToList.SenderName),
                GroupBy.GetterName => nameof(ParcelToList.GetterName),
                GroupBy.Id => "",
                _ => null,
            };
        }
        //private GroupDescription MyGroup(CollectionViewGroup group, int level)
        //{
        //    .Add(new PropertyGroupDescription(nameof(ParcelToList.SenderName)));
        //}

        private bool FilterParcel(object obj)
        {
            if (obj is ParcelToList parcelToList)
            {
                if (ParcelStatusSelected == default || parcelToList.Status == ParcelStatusSelected)
                    //&&(!StartTime.HasValue || ))
                    return true;

                else
                    return false;
            }
            return false;
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void AddParcel(object obj)
        {
            new ParcelView(bl, RefreshParcelList).Show();
        }

        private void EditParcel(object obj)
        {
            var parcel = obj as BO.ParcelToList;
            var blParcel = bl.GetParcel(parcel.Id);
            new ParcelView(bl, RefreshParcelList, blParcel).Show();
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
        public ParcelStatus ParcelStatusSelected
        {
            get => parcelStatusSelected;
            set
            {
                parcelStatusSelected = value;
                ParcelList.Refresh();
            }
        }
        public DateTime? StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                ParcelList.Refresh();
            }
        }
        public DateTime? EndTime
        {
            get => endTime;
            set
            {

                endTime = value;
                ParcelList.Refresh();
            }
        }
        //public Dictionary<string, ObservableCollection<ParcelToList>> ParcelListBySender { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
