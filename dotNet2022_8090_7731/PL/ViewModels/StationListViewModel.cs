﻿using BO;
using PL.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.ViewModels
{
    public class StationListViewModel 
    {
        private int choosenNumPositions { get; set; }
        private readonly BlApi.IBL bl;
        //public IEnumerable<StationToList> StationList { get; set; }
        public ListCollectionView StationList { get; set; }
        public RelayCommand<object> AddStationCommand { get; set; }
        public RelayCommand<object> ShowStationCommand { get; set; }
        public RelayCommand<object> CloseWindowCommand { get; set; }
        public List<int> AvailablePositionsList { get; set; }
        public RelayCommand<object> FilterListCommand { get; set; }

        public StationListViewModel(BlApi.IBL bl)
        {
            this.bl = bl;
            StationList = new(bl.AvailableSlots().ToList());
            AvailablePositionsList = bl.AvailableSlots().Select(station => station.AvailablePositions).Distinct().ToList();
            
            //RefreshStationList();
            AddStationCommand = new RelayCommand<object>(AddingStation);
            ShowStationCommand = new RelayCommand<object>(ShowStation);
            CloseWindowCommand = new RelayCommand<object>(CloseWindow);
            FilterListCommand = new RelayCommand<object>(FilterList);
            //StationList.Filter = new Predicate<object>(Contains);
        }

        private void AddingStation(object sender)
        {
            if (bl.AvailableSlots().Select(slot => slot.Id).Count() > 0)
            {
                new StationView(bl, null)
                    .Show();
            }
            else
            {
                MessageBox.Show("There is no available slots to charge in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStation(object sender)
        {
            var selectedStation = sender as StationToList;
            var blStation = bl.GetStation(selectedStation.Id);
            new StationView(bl, null, blStation)
                    .Show();
        }

        private void CloseWindow(object sender)
        {
            Window.GetWindow((DependencyObject)sender).Close();
        }

        private void FilterList(object num)
        {
            choosenNumPositions = (int)num;
            StationList.Filter = new Predicate<object>(Contains);
        }

        public bool Contains(object obj)
        {
            StationToList station = obj as StationToList;
            return choosenNumPositions == 0 || station.AvailablePositions == choosenNumPositions;
        }

        //private void RefreshStationList()
        //{
        //    //StationList = bl.GetStations();
        //}

    }
}
