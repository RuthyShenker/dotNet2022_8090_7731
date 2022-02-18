using System;
using System.Runtime.Serialization;

namespace DO
{
    /// <summary>
    /// An abstract class of id exception.
    /// </summary>
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

    /// <summary>
    /// A class of id does not exist exception.
    /// </summary>
    [Serializable]
    public class IdDoesNotExistException : Exception
    {
        public IdDoesNotExistException() : base() { }
        public IdDoesNotExistException(string message) : base(message) { }
        public IdDoesNotExistException(string message, Exception inner) : base(message, inner) { }
        protected IdDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        //public IdDoesNotExistException(Type type, int id) : base(type, id) { }

        //protected override string Message()
        //{
        //    return $"Id {Id} is not exist in {Type.Name} list";
        //}
    }

    /// <summary>
    /// A class of id already exists exception.
    /// </summary>
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

    /// <summary>
    /// A class of List Is Empty Exception.
    /// </summary>
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

    /// <summary>
    /// A class of InValid Action Exception.
    /// </summary>
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
            return $"{GetType().Name}: The action couldn't be done. " + ExceptionDetails + $"in {Type.Name} with Id {Id}";
        }
    }


    /// <summary>
    /// A class of XML File Load Create Exception.
    /// </summary>
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
}