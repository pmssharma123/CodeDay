using CoDayContest.Interfaces;
using CoDayContest.Models;

namespace CoDayContest
{
    public class ElectricityConsumptionCalculatorImpl : IElectricityComsumptionCalculator
    {
        public ConsumptionResult CalculateElectricityAndTimeComsumption(ResourcesInfo resourcesInfo)
        {
            ConsumptionResult con = new ConsumptionResult();
          
            var tripDetails = File.ReadLines(resourcesInfo.TripDetailsFilePath).Select(line => line.Split(';')).ToList();
            var vehicleType = File.ReadLines(resourcesInfo.VehicleTypeInfoFilePath).Select(line => line.Split(';')).ToList();
            var entryExitPoint = File.ReadLines(resourcesInfo.EntryExitPointInfoFilePath).Select(line => line.Split(';')).ToList();
            var timeToChargeVehicle = File.ReadLines(resourcesInfo.TimeToChargeVehicleInfoFilePath).Select(line => line.Split(';')).ToList();
            var chargeStation = File.ReadLines(resourcesInfo.ChargingStationInfoFilePath).Select(line => line.Split(';')).ToList();


            List<VehicleTypeInfo> vehicleTypeInfo = new ();

            for (int i = 1; i < vehicleType.Count; i++)
            {
                vehicleType[i].ToList().ForEach(x =>
                {
                    var y = x.Split(',');
                    vehicleTypeInfo.Add(new VehicleTypeInfo
                    {
                        VehicleType = y[0],
                        NumberOfUnitsForFullyCharge = Convert.ToInt32(y[1]),
                        Mileage = Convert.ToInt32(y[2])
                    });
                });
            }

            List<TripDetails> tripDetailsInfo = new();

            for (int i = 1; i < tripDetails.Count; i++)
            {
                tripDetails[i].ToList().ForEach(x =>
                {
                    var y = x.Split(',');
                    tripDetailsInfo.Add(new TripDetails
                    {
                        VehicleType = y[1],
                        RemainingBatteryPercentage = Convert.ToInt32(y[2]),
                        EntryPoint = y[3],
                        ExitPoint = y[4],
                                          
                    });
                });
            }

            List<EntryExitDetails> entryExitDetails = new();

            for (int i = 1; i < entryExitPoint.Count; i++)
            {
                entryExitPoint[i].ToList().ForEach(x =>
                {
                    var y = x.Split(',');
                    entryExitDetails.Add(new EntryExitDetails
                    {
                        EntryExitPoint = y[0],
                        Distance = Convert.ToInt32(y[1])
                    });
                   
                });
            }

            List<TimeToChargeDetails> timeToChargeDetails = new();

            for (int i = 1; i < timeToChargeVehicle.Count; i++)
            {
                timeToChargeVehicle[i].ToList().ForEach(x =>
                {
                    var y = x.Split(',');
                    timeToChargeDetails.Add(new TimeToChargeDetails
                    {
                        VehicleType = y[0],
                        ChargingStation = y[1],
                        TimeToCharge = Convert.ToInt32(y[2])           

                    });
                });
            }


            List<ChargeStationDetails> chargeStationDetails = new();

            for (int i = 1; i < chargeStation.Count; i++)
            {
                chargeStation[i].ToList().ForEach(x =>
                {
                    var y = x.Split(',');
                    chargeStationDetails.Add(new ChargeStationDetails
                    {
                       
                        ChargingStationName = y[0],
                        DistanceFromStart = Convert.ToInt32(y[1])

                    });
                });
            }



            return con;
        }
    }
}
