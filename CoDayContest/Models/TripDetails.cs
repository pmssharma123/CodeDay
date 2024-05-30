using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDayContest.Models
{
    public class TripDetails
    { 

        public string VehicleType { get; set; }
        public string EntryPoint { get; set; }
        public string ExitPoint { get; set; }
        public int RemainingBatteryPercentage { get; set; }
    }
}
