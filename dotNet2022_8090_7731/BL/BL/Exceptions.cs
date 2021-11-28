﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    ///// <summary>
    ///// ???????
    ///// </summary>
    //[Serializable]
    //public class MyExceptions : Exception
    //{
    //    public int capacity { get; private set; }

    //    public OverloadCapacityException() : base() { }
    //    public OverloadCapacityException(string message) : base(message) { }
    //    public OverloadCapacityException(string message, Exception inner) : base(message, inner) { }
    //    protected OverloadCapacityException(SerializationInfo info, StreamingContext context) : base(info, context) { }     // special constructor for our custom exception     public OverloadCapacityException(int capacity, string message) : base(message)     {         this.capacity = capacity;     }     override public string ToString()     {         return "OverloadCapacityException: DAL capacity of " + capacity + " overloaded\n" + Message;     } } 



    //    public ArgumentNullException(object Id, object typeList)
    //    {
    //        Console.WriteLine("{0} isnt exist in {1} list.", Id, typeList);
    //    }

    /// <summary>
    /// A class UpdatingFailedIdNotExistsException : Exception
    /// gets A string
    /// </summary>
    public class UpdatingFailedIdNotExistsException : Exception
    {
        /// <summary>
        /// A class UpdatingFailedIdNotExistsException : Exception
        /// gets A string
        /// </summary>
        public UpdatingFailedIdNotExistsException(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class ThereIsntEnoughBatteryToTheDrone : Exception
    /// gets A string
    /// </summary>
    public class ThereIsntEnoughBatteryToTheDroneException : Exception
    {
        /// <summary>
        /// A class ThereIsntEnoughBatteryToTheDrone : Exception
        /// gets A string
        /// </summary>
        public ThereIsntEnoughBatteryToTheDroneException(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class StationDoesntHaveAvailablePositionsException : Exception
    /// gets A string
    /// </summary>
    public class StationDoesntHaveAvailablePositionsException : Exception
    {
        /// <summary>
        /// A class StationDoesntHaveAvailablePositionsException : Exception
        /// gets A string
        /// </summary>
        public StationDoesntHaveAvailablePositionsException(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class IdIsNotValidException : Exception
    /// gets A string
    /// </summary>
    public class IdIsNotValidException : Exception
    {
        /// <summary>
        /// A class IdIsNotValidException : Exception
        /// gets A string
        /// </summary>
        public IdIsNotValidException(string message) : base()
        {

        }
    }

    /// <summary>
    /// A class TheStationDoesNotHaveFreePositions : Exception
    /// gets A string
    /// </summary>
    public class TheStationDoesNotHaveFreePositionsException : Exception
    {
        /// <summary>
        /// A class TheStationDoesNotHaveFreePositions : Exception
        /// gets A string
        /// </summary>
        public TheStationDoesNotHaveFreePositionsException(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class UpdatingCustomerDetails : Exception
    /// gets A string
    /// </summary>
    public class UpdatingCustomerDetailsException : Exception
    {
        /// <summary>
        /// A constructor of UpdatingCustomerDetails with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public UpdatingCustomerDetailsException(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class SendingDroneToCharge : Exception
    /// gets A string
    /// </summary>
    public class SendingDroneToChargeException : Exception
    {
        /// <summary>
        /// A constructor of SendingDroneToCharge with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public SendingDroneToChargeException(string message):base()
        {

        }
    }

    /// <summary>
    /// A class BelongingParcel : Exception
    /// gets A string
    /// </summary>
    public class BelongingParcelException:Exception
    {
        /// <summary>
        /// A constructor of BelongingParcel with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public BelongingParcelException(string message):base()
        {

        }
    }

    /// <summary>
    /// A class CantRelasingDroneFromChargingException : Exception
    /// gets A string
    /// </summary>
    public class CantRelasingDroneFromChargingException : Exception
    {
        public CantRelasingDroneFromChargingException(string message):base()
        {

        }
    }
    /// <summary>
    /// A class CantBelongingParcelToDroneException : Exception
    /// gets A string
    /// </summary>
    public class CantBelongingParcelToDroneException : Exception
    {
        public CantBelongingParcelToDroneException(string message):base()
        {

        }
    }
    /// <summary>
    /// A class ParcelIsAlreadyPickedUpException : Exception
    /// gets A string
    /// </summary>
    public class ParcelIsAlreadyPickedUpException : Exception
    {
        public ParcelIsAlreadyPickedUpException(string message):base()
        {
           
        }
    }
    /// <summary>
    /// A class NoParcelAssociatedToTheDroneException : Exception
    /// gets A string
    /// </summary>
    public class NoParcelAssociatedToTheDroneException : Exception
        {
            public NoParcelAssociatedToTheDroneException(string message):base()
            {

            }
        }
    /// <summary>
    /// A class ParcelsStatusIsntMatchException : Exception
    /// gets A string
    /// </summary>
    public class ParcelsStatusIsntMatchException : Exception
    {
        public ParcelsStatusIsntMatchException(string message):base()
        {
                
        }
    }


}




