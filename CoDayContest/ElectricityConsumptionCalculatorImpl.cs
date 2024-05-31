using CoDayContest.Interfaces;
using CoDayContest.Models;
using CsvHelper;
using System.Formats.Asn1;

namespace CoDayContest
{
    public class ElectricityConsumptionCalculatorImpl : IElectricityComsumptionCalculator
    {
        List<ChargeStationDetails>? chargeStationDetails = null;
        List<TripDetails>? tripDetailsInfo = null;
        List<EntryExitDetails>? entryExitDetails = null;
        List<VehicleTypeInfo>? vehicleTypeInfo = null;
        List<TimeToChargeDetails>? timeToChargeDetails = null;


        public ConsumptionResult CalculateElectricityAndTimeComsumption(ResourcesInfo resourcesInfo)
        {
            ConsumptionResult con = new ConsumptionResult();

            chargeStationDetails = ReadCSV<ChargeStationDetails>(resourcesInfo.ChargingStationInfoFilePath);
            tripDetailsInfo = ReadCSV<TripDetails>(resourcesInfo.TripDetailsFilePath);
            entryExitDetails = ReadCSV<EntryExitDetails>(resourcesInfo.EntryExitPointInfoFilePath);
            vehicleTypeInfo = ReadCSV<VehicleTypeInfo>(resourcesInfo.VehicleTypeInfoFilePath);
            timeToChargeDetails = ReadCSV<TimeToChargeDetails>(resourcesInfo.TimeToChargeVehicleInfoFilePath);

            foreach (var item in tripDetailsInfo)
            {
                var vehicleDetails = vehicleTypeInfo.FirstOrDefault(x => x.VehicleType == item.VehicleType);
                
                var exitPoint = entryExitDetails.Where(x => x.EntryExitPoint == item.ExitPoint).FirstOrDefault();

                var entryPoint = entryExitDetails.Where(x => x.EntryExitPoint == item.EntryPoint).FirstOrDefault();

                //added new obj for entry exit
                EntryExitDetails entryExitDetail = new()
                {
                    EntryExitPoint = entryPoint.EntryExitPoint,
                    DistanceFromStart = entryPoint.DistanceFromStart
                };

                float remainingBatteryInUnits = (item.RemainingBatteryPercentage * vehicleDetails.NumberOfUnitsForFullyCharge) / 100;
                float distanceThatCanBeCovered = (vehicleDetails.Mileage * remainingBatteryInUnits) / vehicleDetails.NumberOfUnitsForFullyCharge;

                var TotalDistanceCanBeCovered = entryPoint.DistanceFromStart + distanceThatCanBeCovered;
                double KmsPerUnit = Math.Round(vehicleDetails.Mileage / vehicleDetails.NumberOfUnitsForFullyCharge,1);

                do
                {
                    var nextEntryPoint = entryExitDetails.Where(x => x.DistanceFromStart <= TotalDistanceCanBeCovered && x.DistanceFromStart <= exitPoint.DistanceFromStart).LastOrDefault();

                    var NearestChargingPoint = chargeStationDetails.Where(x => x.DistanceFromStart >= entryExitDetail.DistanceFromStart && x.DistanceFromStart <= TotalDistanceCanBeCovered).LastOrDefault();

                    double DistanceRemained = TotalDistanceCanBeCovered - NearestChargingPoint.DistanceFromStart;

                    var timeToCharge = timeToChargeDetails.Where(x => x.VehicleType == item.VehicleType && x.ChargingStation == NearestChargingPoint.ChargingStation).FirstOrDefault();

                    double unitsSaved = Math.Round(DistanceRemained / KmsPerUnit,1);

                    var TotalTimeToCharge = (vehicleDetails.NumberOfUnitsForFullyCharge - unitsSaved) * timeToCharge.TimeToChargePerUnit;

                    var TotalEnergyConsumed = Math.Round(remainingBatteryInUnits - unitsSaved,1);
                    var tripsFinished = 0;

                    if (nextEntryPoint.EntryExitPoint == exitPoint.EntryExitPoint || TotalDistanceCanBeCovered >= exitPoint.DistanceFromStart)
                    {
                        DistanceRemained = TotalDistanceCanBeCovered - exitPoint.DistanceFromStart;
                        unitsSaved = Math.Round(DistanceRemained / KmsPerUnit);
                        TotalTimeToCharge = 0;
                        TotalEnergyConsumed = remainingBatteryInUnits - unitsSaved;
                        tripsFinished = 1;
                    }

                    if (!con.ConsumptionDetails.Exists(x => x.VehicleType == vehicleDetails.VehicleType))
                    {
                        con.ConsumptionDetails.Add(new ConsumptionDetails
                        {
                            VehicleType = vehicleDetails.VehicleType,
                            TotalTimeRequired = (long)TotalTimeToCharge,
                            TotalUnitConsumed = TotalEnergyConsumed,
                            NumberOfTripsFinished = tripsFinished
                        });                     
                    }
                    else
                    {
                        var ConsumptionDetails = con.ConsumptionDetails.First(x => x.VehicleType == vehicleDetails.VehicleType);
                        ConsumptionDetails.TotalTimeRequired += (long)TotalTimeToCharge;
                        ConsumptionDetails.TotalUnitConsumed += TotalEnergyConsumed;
                        ConsumptionDetails.NumberOfTripsFinished += tripsFinished;
                    }


                    if(!con.TotalChargingStationTime.ContainsKey(NearestChargingPoint.ChargingStation))
                    {
                        con.TotalChargingStationTime.Add(NearestChargingPoint.ChargingStation, (long)TotalTimeToCharge);
                    }
                    else
                    {
                        con.TotalChargingStationTime[NearestChargingPoint.ChargingStation] += (long)TotalTimeToCharge;
                    }

                    TotalDistanceCanBeCovered = vehicleDetails.Mileage + NearestChargingPoint.DistanceFromStart;            
                    entryExitDetail.DistanceFromStart = NearestChargingPoint.DistanceFromStart;
                    entryExitDetail.EntryExitPoint = nextEntryPoint.EntryExitPoint;
                    remainingBatteryInUnits = vehicleDetails.NumberOfUnitsForFullyCharge;
                    
                }
                while (exitPoint.EntryExitPoint != entryExitDetail.EntryExitPoint);

            }

            return con;
        }

        List<T>? ReadCSV<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>();
                List<T> list = records.ToList();
                return list;
            }
        }

    }
}
