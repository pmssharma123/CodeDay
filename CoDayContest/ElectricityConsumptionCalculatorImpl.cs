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
                EntryExitDetails entryExitDetail = new EntryExitDetails();
                entryExitDetail.EntryExitPoint = entryPoint.EntryExitPoint;
                entryExitDetail.DistanceFromStart = entryPoint.DistanceFromStart;

                int remainingBatteryInUnits = (item.RemainingBatteryPercentage * vehicleDetails.NumberOfUnitsForFullyCharge) / 100;



                int distanceThatCanBeCovered = (vehicleDetails.Mileage * remainingBatteryInUnits) / vehicleDetails.NumberOfUnitsForFullyCharge;

                var TotalDistanceCanBeCovered = entryPoint.DistanceFromStart + distanceThatCanBeCovered;
                double KmsPerUnit = vehicleDetails.Mileage / vehicleDetails.NumberOfUnitsForFullyCharge;

                //  if(TotalDistanceCanBeCovered <= exitPoint.DistanceFromStart)   last station


                do
                {
                    var NearestChargingPoint = chargeStationDetails.Where(x => x.DistanceFromStart >= entryExitDetail.DistanceFromStart && x.DistanceFromStart <= TotalDistanceCanBeCovered).LastOrDefault();

                    double DistanceRemained = TotalDistanceCanBeCovered - NearestChargingPoint.DistanceFromStart;

                    double unitsSaved = (double)(DistanceRemained / KmsPerUnit);

                    var timeToCharge = timeToChargeDetails.Where(x => x.VehicleType == item.VehicleType && x.ChargingStation == NearestChargingPoint.ChargingStation).FirstOrDefault();

                    var TotalTimeToCharge = (vehicleDetails.NumberOfUnitsForFullyCharge - unitsSaved) * timeToCharge.TimeToChargePerUnit;

                    var TotalEnergyConsumed = remainingBatteryInUnits - unitsSaved;

                    if (!con.ConsumptionDetails.Exists(x => x.VehicleType == vehicleDetails.VehicleType))
                    {
                        con.ConsumptionDetails.Add(new ConsumptionDetails
                        {
                            VehicleType = vehicleDetails.VehicleType,
                            TotalTimeRequired = (long)TotalTimeToCharge,
                            TotalUnitConsumed = TotalEnergyConsumed,
                            NumberOfTripsFinished = exitPoint.EntryExitPoint == entryExitDetail.EntryExitPoint ? 1 : 0
                        });
                    }
                    else
                    {
                        var ConsumptionDetails = con.ConsumptionDetails.First(x => x.VehicleType == vehicleDetails.VehicleType);
                        ConsumptionDetails.TotalTimeRequired += (long)TotalTimeToCharge;
                        ConsumptionDetails.TotalUnitConsumed += TotalEnergyConsumed;
                        ConsumptionDetails.NumberOfTripsFinished += exitPoint.EntryExitPoint == entryExitDetail.EntryExitPoint ? 1 : 0;
                    }

                    TotalDistanceCanBeCovered = vehicleDetails.Mileage + NearestChargingPoint.DistanceFromStart;
                    var currentEntryPoint = entryExitDetails.Where(x => x.DistanceFromStart <= TotalDistanceCanBeCovered && x.DistanceFromStart <= exitPoint.DistanceFromStart).LastOrDefault();
                    entryExitDetail.DistanceFromStart = NearestChargingPoint.DistanceFromStart;
                    entryExitDetail.EntryExitPoint = currentEntryPoint.EntryExitPoint;
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
