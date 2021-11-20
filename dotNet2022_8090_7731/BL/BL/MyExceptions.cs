using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// ???????
    /// </summary>
    [Serializable]
    public class MyExceptions : Exception
    {
        public int capacity { get; private set; }

        public OverloadCapacityException() : base() { }
        public OverloadCapacityException(string message) : base(message) { }
        public OverloadCapacityException(string message, Exception inner) : base(message, inner) { }
        protected OverloadCapacityException(SerializationInfo info, StreamingContext context) : base(info, context) { }     // special constructor for our custom exception     public OverloadCapacityException(int capacity, string message) : base(message)     {         this.capacity = capacity;     }     override public string ToString()     {         return "OverloadCapacityException: DAL capacity of " + capacity + " overloaded\n" + Message;     } } 



        public ArgumentNullException(object Id, object typeList)
        {
            Console.WriteLine("{0} isnt exist in {1} list.", Id, typeList);
        }
    }
    class UpdatingFailedIdNotExistsException : Exception
    {
        public UpdatingFailedIdNotExistsException(string message):base()
        {

        }
            
   
    }

}
