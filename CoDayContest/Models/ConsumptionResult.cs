namespace CoDayContest.Models
{
    public class ConsumptionResult
    {
        public List<ConsumptionDetails> ConsumptionDetails = new List<ConsumptionDetails>();
        public Dictionary<String, Int64> TotalChargingStationTime = new Dictionary<String, Int64>();
    }
}
