using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
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
        protected abstract string Message();
        override public string ToString()
        {
            return $"{GetType().Name}: {Message()}";
        }
    }

    [Serializable]
    public class IdIsNotExistException : Exception
    {
        public IdIsNotExistException() : base() { }
        public IdIsNotExistException(string message) : base(message) { }
        public IdIsNotExistException(string message, Exception inner) : base(message, inner) { }
        protected IdIsNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        //public IdIsNotExistException(Type type, int id) : base(type, id) { }

        //protected override string Message()
        //{
        //    return $"Id {Id} is not exist in {Type.Name} list";
        //}
    }

    [Serializable]
    public class IdIsAlreadyExistException : IdException
    {
        public IdIsAlreadyExistException() : base() { }
        public IdIsAlreadyExistException(string message) : base(message) { }
        public IdIsAlreadyExistException(string message, Exception inner) : base(message, inner) { }
        protected IdIsAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdIsAlreadyExistException(Type type, int id) : base(type, id) { }

        protected override string Message()
        {
            return $"Id {Id} is already exist in {Type.Name} list";
        }
    }

    [Serializable]
    public class ListIsEmptyException : Exception
    {
        public ListIsEmptyException() : base() { }
        public ListIsEmptyException(string message) : base(message) { }
        public ListIsEmptyException(string message, Exception inner) : base(message, inner) { }
        protected ListIsEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public Type Type { get; set; }
        public ListIsEmptyException(Type type) : base()
        {
            Type = type;
        }
        override public string ToString()
        {
            return $"{GetType().Name}: {Type.Name} list is empty";
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

        protected override string Message()
        {
            return $"{GetType().Name}: The action couldn't be done. " + ExceptionDetails + $"in {Type.Name} with Id {Id}";
        }
    }
}