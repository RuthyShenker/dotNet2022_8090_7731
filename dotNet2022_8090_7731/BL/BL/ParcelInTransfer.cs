﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;

namespace IBL.BO
{
    class ParcelInTransfer
    {
        public int PId { get; set; }
        public bool IsInWay { get; set; }
        public priority MPriority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Getter { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryLocation { get; set; }
        public float TransDistance { get; set; }
    }
}
