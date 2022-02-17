
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
    public class IdAlreadyExistsException : IdException
    {
        public IdAlreadyExistsException() : base() { }
        public IdAlreadyExistsException(string message) : base(message) { }
        public IdAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected IdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdAlreadyExistsException(Type type, int id) : base(type, id) { }

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

