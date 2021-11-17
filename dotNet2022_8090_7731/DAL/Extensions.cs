using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    static class Extensions
    {
        //static object FindInList(List<IIdentity> list, object Id)
        //{
        //    try
        //    {
        //        return list.First(item => item.Id == Id);
        //    }
        //    catch (InvalidOperationException ex)
        //    {

        //        throw new InvalidOperationException(Id,list.GetType());
        //    }
        //    catch (ArgumentNullException ex)
        //    {

        //        throw new ArgumentNullException(Id, list.GetType());
        //    }
        //}
       

        public object DisplayItem<T>(List<T> list, object Id)
        {
            
            return list[Id].Clone();
        }

        public object DisplayItem<T>(List<T> list)
        {

            return list.Clone();
        }

    }
}
