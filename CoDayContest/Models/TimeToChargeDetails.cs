using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDayContest.Models
{
    public class TimeToChargeDetails
    {
        public string VehicleType { get; set; }

        public string ChargingStation { get; set; }
        public int TimeToCharge { get; set; }
    }
}
