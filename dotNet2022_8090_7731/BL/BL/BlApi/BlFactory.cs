using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi 
{
    /// <summary>
    /// A public static class BlFactory includes only one function GetBl.
    /// </summary>
    public static class BlFactory
    {

        /// <summary>
        /// A static function that returns instance of BL.
        /// </summary>
        /// <returns></returns>
        public static IBL GetBl() => BL.BL.Instance;

    }
}
