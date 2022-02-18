using BO;
using Singleton;
using System;
using System.Collections.Generic;

namespace BL
{
    /// <summary>
    /// An internal sealed partial class BL inherits from Singleton<BL>,and impliments BlApi.IBL,
    /// </summary>
    internal sealed partial class BL : Singleton<BL>, BlApi.IBL
    {

        /// <summary>
        /// instance of dal.
        /// </summary>
        internal DalApi.IDal dal = DalApi.DalFactory.GetDal();

        /// <summary>
        /// list of drones of type DroneToList.
        /// </summary>
        internal List<DroneToList> lDroneToList;

        /// <summary>
        /// an instance of class Random.
        /// </summary>
        readonly Random rand;

        /// <summary>
        ///data of Power Consumptions.
        /// </summary>
        internal static double powerConsumptionLight { get; set; }
        internal static double powerConsumptionMedium {get; set; }
        internal static double powerConsumptionHeavy {get; set; }
        internal static double PowerConsumptionFree { get; set; }

        /// <summary>
        /// Charging rate per hour
        /// </summary>
        internal static double chargingRate { get; set; }

        /// <summary>
        /// A private constructor of BL that Initialize Power Consumptions, Initialize Drone List,initialize an instance of dal and rand.
        /// </summary>
        private BL()
        {
            rand = new Random();
            dal = DalApi.DalFactory.GetDal();
            InitializePowerConsumption();
            InitializeDroneList();
        }

        /// <summary>
        /// A function that Pulls out from that data base the data of the fields:
        /// powerConsumptionFree
        /// powerConsumptionLight
        /// powerConsumptionMedium
        /// powerConsumptionHeavy
        /// chargingRate
        /// this function doesn't return any thing.
        /// </summary>
        private void InitializePowerConsumption()
        {
            (
                PowerConsumptionFree,
                powerConsumptionLight,
                powerConsumptionMedium,
                powerConsumptionHeavy,
                chargingRate
            ) = dal.PowerConsumptionRequest();
        }

    }
}






