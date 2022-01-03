using BO;
using PL.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace PL.ViewModels
{
    public class ParcelListViewModel : INotifyPropertyChanged
    {
        BlApi.IBL bl;
        public ListCollectionView parcelList;
        private ParcelStatus parcelStatusSelected;
        DateTime? startTime, endTime;

        public RelayCommand<object> MouseDoubleCommand { get; set; }
        public RelayCommand<object> AddParcelCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }

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
            //ParcelList.SortDescriptions.Add(new SortDescription(nameof(ParcelToList.GetterName), ListSortDirection.Descending));
            ParcelList.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ParcelToList.SenderName)));
            //ParcelList.GroupBySelector = MyGroup;


            this.bl = bl;

            //ParcelList = bl.GetParcels();

            //ParcelListBySender = bl.GetParcels().GroupBy(parcel => parcel.SenderName)
            //    .ToDictionary(key => key.Key, value => new ObservableCollection<ParcelToList>(value));

            MouseDoubleCommand = new RelayCommand<object>(EditParcel);
            AddParcelCommand = new RelayCommand<object>(AddParcel);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
        }

        private GroupDescription MyGroup(CollectionViewGroup group, int level)
        {
            throw new NotImplementedException();
        }

        private bool FilterParcel(object obj)
        {
            if (obj is ParcelToList parcelToList)
            {
                if ((ParcelStatusSelected == default || parcelToList.Status == ParcelStatusSelected))
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
        public Dictionary<string, ObservableCollection<ParcelToList>> ParcelListBySender { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
