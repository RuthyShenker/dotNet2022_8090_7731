using System;
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
    public class ThereIsntEnoughBatteryToTheDrone : Exception
    {
        /// <summary>
        /// A class ThereIsntEnoughBatteryToTheDrone : Exception
        /// gets A string
        /// </summary>
        public ThereIsntEnoughBatteryToTheDrone(string message) : base()
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
    public class TheStationDoesNotHaveFreePositions : Exception
    {
        /// <summary>
        /// A class TheStationDoesNotHaveFreePositions : Exception
        /// gets A string
        /// </summary>
        public TheStationDoesNotHaveFreePositions(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class UpdatingCustomerDetails : Exception
    /// gets A string
    /// </summary>
    public class UpdatingCustomerDetails : Exception
    {
        /// <summary>
        /// A constructor of UpdatingCustomerDetails with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public UpdatingCustomerDetails(string message) : base()
        {

        }
    }
    /// <summary>
    /// A class SendingDroneToCharge : Exception
    /// gets A string
    /// </summary>
    public class SendingDroneToCharge : Exception
    {
        /// <summary>
        /// A constructor of SendingDroneToCharge with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public SendingDroneToCharge(string message):base()
        {

        }
    }

    /// <summary>
    /// A class BelongingParcel : Exception
    /// gets A string
    /// </summary>
    public class BelongingParcel:Exception
    {
        /// <summary>
        /// A constructor of BelongingParcel with one parameter of string
        /// </summary>
        /// <param name="message"></param>
        public BelongingParcel(string message):base()
        {

        }
    }
    public class CantRelasingDroneFromChargingException : Exception
    {
        public CantRelasingDroneFromChargingException(string message):base()
        {

        }
    }
    public class CantBelongingParcelToDroneException : Exception
    {
        public CantBelongingParcelToDroneException(string message):base()
        {

        }
    }

}




