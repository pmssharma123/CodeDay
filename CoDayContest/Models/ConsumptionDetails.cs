using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDayContest.Models
{
    public class ConsumptionDetails
    {
        public String VehicleType { get; set; }
        public Double TotalUnitConsumed { get; set; }
        public Int64 TotalTimeRequired { get; set; }
        public Int64 NumberOfTripsFinished { get; set; }

       
    }
}
