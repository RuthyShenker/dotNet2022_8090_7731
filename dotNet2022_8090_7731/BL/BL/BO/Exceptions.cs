//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL
//{
//    ///// <summary>
//    ///// ???????
//    ///// </summary>
//    //[Serializable]
//    //public class MyExceptions : Exception
//    //{
//    //    public int capacity { get; private set; }

//    //    public OverloadCapacityException() : base() { }
//    //    public OverloadCapacityException(string message) : base(message) { }
//    //    public OverloadCapacityException(string message, Exception inner) : base(message, inner) { }
//    //    protected OverloadCapacityException(SerializationInfo info, StreamingContext context) : base(info, context) { }     // special constructor for our custom exception     public OverloadCapacityException(int capacity, string message) : base(message)     {         this.capacity = capacity;     }     override public string ToString()     {         return "OverloadCapacityException: DAL capacity of " + capacity + " overloaded\n" + Message;     } } 



//    //    public ArgumentNullException(object Id, object typeList)
//    //    {
//    //        Console.WriteLine("{0} isnt exist in {1} list.", Id, typeList);
//    //    }

//    /// <summary>
//    /// A class UpdatingFailedIdNotExistsException : Exception
//    /// gets A string
//    /// </summary>
//    public class UpdatingFailedIdNotExistsException : Exception
//    {
//        /// <summary>
//        /// A class UpdatingFailedIdNotExistsException : Exception
//        /// gets A string
//        /// </summary>
//        public UpdatingFailedIdNotExistsException(string message) : base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class ThereIsntEnoughBatteryToTheDrone : Exception
//    /// gets A string
//    /// </summary>
//    public class ThereIsntEnoughBatteryToTheDroneException : Exception
//    {
//        /// <summary>
//        /// A class ThereIsntEnoughBatteryToTheDrone : Exception
//        /// gets A string
//        /// </summary>
//        public ThereIsntEnoughBatteryToTheDroneException(string message) : base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class StationDoesntHaveAvailablePositionsException : Exception
//    /// gets A string
//    /// </summary>
//    public class StationDoesntHaveAvailablePositionsException : Exception
//    {
//        /// <summary>
//        /// A class StationDoesntHaveAvailablePositionsException : Exception
//        /// gets A string
//        /// </summary>
//        public StationDoesntHaveAvailablePositionsException(string message) : base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class IdIsNotValidException : Exception
//    /// gets A string
//    /// </summary>
//    public class IdIsNotValidException : Exception
//    {
//        /// <summary>
//        /// A class IdIsNotValidException : Exception
//        /// gets A string
//        /// </summary>
//        public IdIsNotValidException(string message) : base()
//        {

//        }
//    }

//    /// <summary>
//    /// A class TheStationDoesNotHaveFreePositions : Exception
//    /// gets A string
//    /// </summary>
//    public class TheStationDoesNotHaveFreePositionsException : Exception
//    {
//        /// <summary>
//        /// A class TheStationDoesNotHaveFreePositions : Exception
//        /// gets A string
//        /// </summary>
//        public TheStationDoesNotHaveFreePositionsException(string message) : base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class UpdatingCustomerDetails : Exception
//    /// gets A string
//    /// </summary>
//    public class UpdatingCustomerDetailsException : Exception
//    {
//        /// <summary>
//        /// A constructor of UpdatingCustomerDetails with one parameter of string
//        /// </summary>
//        /// <param name="message"></param>
//        public UpdatingCustomerDetailsException(string message) : base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class SendingDroneToCharge : Exception
//    /// gets A string
//    /// </summary>
//    public class SendingDroneToChargeException : Exception
//    {
//        /// <summary>
//        /// A constructor of SendingDroneToCharge with one parameter of string
//        /// </summary>
//        /// <param name="message"></param>
//        public SendingDroneToChargeException(string message):base()
//        {

//        }
//    }

//    /// <summary>
//    /// A class BelongingParcel : Exception
//    /// gets A string
//    /// </summary>
//    public class BelongingParcelException:Exception
//    {
//        /// <summary>
//        /// A constructor of BelongingParcel with one parameter of string
//        /// </summary>
//        /// <param name="message"></param>
//        public BelongingParcelException(string message):base()
//        {

//        }
//    }

//    /// <summary>
//    /// A class CantRelasingDroneFromChargingException : Exception
//    /// gets A string
//    /// </summary>
//    public class CantRelasingDroneFromChargingException : Exception
//    {
//        public CantRelasingDroneFromChargingException(string message):base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class CantBelongingParcelToDroneException : Exception
//    /// gets A string
//    /// </summary>
//    public class CantBelongingParcelToDroneException : Exception
//    {
//        public CantBelongingParcelToDroneException(string message):base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class ParcelIsAlreadyPickedUpException : Exception
//    /// gets A string
//    /// </summary>
//    public class ParcelIsAlreadyPickedUpException : Exception
//    {
//        public ParcelIsAlreadyPickedUpException(string message):base()
//        {

//        }
//    }
//    /// <summary>
//    /// A class NoParcelAssociatedToTheDroneException : Exception
//    /// gets A string
//    /// </summary>
//    public class NoParcelAssociatedToTheDroneException : Exception
//        {
//            public NoParcelAssociatedToTheDroneException(string message):base()
//            {

//            }
//        }
//    /// <summary>
//    /// A class ParcelsStatusIsntMatchException : Exception
//    /// gets A string
//    /// </summary>
//    public class ParcelsStatusIsntMatchException : Exception
//    {
//        public ParcelsStatusIsntMatchException(string message):base()
//        {

//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public abstract class IdException : Exception
    {
        public IdException() : base() { }
        public IdException(string message) : base(message) { }
        public IdException(string message, Exception inner) : base(message, inner) { }
        protected IdException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public Type Type { get; set; }
        public int Id { get; set; }
        public IdException(Type type, int id) : base()
        {
            Type = type;
            Id = id;
        }
        protected abstract string GetMessage();
        override public string ToString()
        {
            return $"{GetType().Name}: {GetMessage()}";
        }
    }

    [Serializable]
    public class IdIsNotExistException : IdException
    {
        public IdIsNotExistException() : base() { }
        public IdIsNotExistException(string message) : base(message) { }
        public IdIsNotExistException(string message, Exception inner) : base(message, inner) { }
        protected IdIsNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdIsNotExistException(Type type, int id) : base(type, id) { }

        protected override string GetMessage()
        {
            return $"Id {Id} is not exist in {Type.Name} list";
        }
    }

    [Serializable]
    public class IdIsAlreadyExistException : IdException
    {
        public IdIsAlreadyExistException() : base() { }
        public IdIsAlreadyExistException(string message) : base(message) { }
        public IdIsAlreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected IdIsAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdIsAlreadyExistException(Type type, int id) : base(type, id) { }

        protected override string GetMessage()
        {
            return $"Id {Id} is already exist in {Type.Name} list";
        }
    }

    [Serializable]
    public abstract class ListException : Exception
    {
        public ListException() : base() { }
        public ListException(string message) : base(message) { }
        public ListException(string message, Exception inner) : base(message, inner) { }
        protected ListException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public Type Type { get; set; }
        public ListException(Type type) : base()
        {
            Type = type;
        }
        protected abstract string GetMessage();
        override public string ToString()
        {
            return $"{GetType().Name}: {GetMessage()}";
        }
    }

    [Serializable]
    public class ListIsEmptyException : ListException
    {
        public ListIsEmptyException() : base() { }
        public ListIsEmptyException(string message) : base(message) { }
        public ListIsEmptyException(string message, Exception inner) : base(message, inner) { }
        protected ListIsEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ListIsEmptyException(Type type) : base(type) { }

        protected override string GetMessage()
        {
            return $"{Type.Name} list is empty";
        }
    }

    [Serializable]
    public class ThereIsNoMatchObjectInListException : ListException
    {
        public ThereIsNoMatchObjectInListException() : base() { }
        public ThereIsNoMatchObjectInListException(string message) : base(message) { }
        public ThereIsNoMatchObjectInListException(string message, Exception inner) : base(message, inner) { }
        protected ThereIsNoMatchObjectInListException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ThereIsNoMatchObjectInListException(Type type, string exceptionDetails) : base(type)
        {
            ExceptionDetails = exceptionDetails;
        }
        public string ExceptionDetails { get; set; }

        protected override string GetMessage()
        {
            return $"{ExceptionDetails} {Type.Name.ToLower()} list";
        }
    }

    [Serializable]
    public class InValidActionException : IdException
    {
        public InValidActionException() : base() { }
        public InValidActionException(string message) : base(message) { }
        public InValidActionException(string message, Exception inner) : base(message, inner) { }
        protected InValidActionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public InValidActionException(Type type, int id, string exceptionDetails) : base(type, id)
        {
            ExceptionDetails = exceptionDetails;
        }
        internal string ExceptionDetails { get; set; }

        protected override string GetMessage()
        {
            return $"The action couldn't be done. " + ExceptionDetails + $"in {Type?.Name} with Id {Id}";
        }
    }
}

