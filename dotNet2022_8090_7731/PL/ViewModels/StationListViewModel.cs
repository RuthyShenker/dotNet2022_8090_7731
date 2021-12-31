using PL.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.ViewModels
{
    public class StationListViewModel: INotifyPropertyChanged
    {
      
            BlApi.IBL bl;
            public IEnumerable<BO.ParcelToList> parcelList;
            public RelayCommand<object> AddParcelCommand { get; set; }
            public RelayCommand<object> MouseDoubleCommand { get; set; }
            public RelayCommand<object> CloseWindowCommand { get; set; }

            public StationListViewModel(BlApi.IBL bl)
            {
                this.bl = bl;
                parcelList = Enumerable.Empty<BO.ParcelToList>();
                RefreshParcelList();
            AddParcelCommand = new RelayCommand<object>(AddingParcel);
                MouseDoubleCommand = new RelayCommand<object>(MouseDoubleClick);
                CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            }

        public IEnumerable<BO.ParcelToList> ParcelList
            {
                get => parcelList;
                set
                {
                parcelList = value;
                    RaisePropertyChanged(nameof(ParcelList));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void RaisePropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private void MouseDoubleClick(object sender)
            {
                var selectedParcel = sender as BO.ParcelToList;
            var blParcel = bl.GetParcel(selectedParcel.Id);
                new ParcelView(bl, RefreshParcelList, blParcel)
                    .Show();
            }
            private void AddingParcel(object sender)
            {
                //if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
                //{
                //    //var viewModel = new AddDroneViewModel(bl,FilterDroneListByCondition);
                //    new DroneView(bl, FilterDroneListByCondition);
                //    //new DroneView(/*bl,*/FilterDroneListByCondition).Show();
                //}
                //else
                //{
                //    MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                new ParcelView(bl, RefreshParcelList)
                    .Show();
            }
            private void RefreshParcelList()
            {
                ParcelList = bl.GetParcels();
            }
            private void CloseWindow(object sender)
            {
                Window.GetWindow((DependencyObject)sender).Close();
            }
        }
}
