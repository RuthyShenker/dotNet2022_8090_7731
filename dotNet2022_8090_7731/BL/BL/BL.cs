using DalObject;
using IBL.BO;
using IDal.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    partial class BL : IBL
    {
        public BL()
        {
            IDal.IDal dalObject = new DalObject.DalObject();
            List<BL_Drone> bL_Drones = new List<BL_Drone>();
            BL_Drone bL_Drone;

            if (dalObject.GetUnbelongParcels()!=null )
            {
                bL_Drone
            }


        }
    }
}
