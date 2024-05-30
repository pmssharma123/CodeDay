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

                var entryPoint = entryExitDetails.Where(x => x.EntryExitPoint == item.EntryPoint).FirstOrDefault();
                var exitPoint = entryExitDetails.Where(x => x.EntryExitPoint == item.ExitPoint).FirstOrDefault();

                var remainingBatteryInUnits = (item.RemainingBatteryPercentage/100) * vehicleDetails.NumberOfUnitsForFullyCharge;

                var distanceThatCanBeCovered = (vehicleDetails.Mileage / vehicleDetails.NumberOfUnitsForFullyCharge) * remainingBatteryInUnits;

                var TotalDistanceCanBeCovered = entryPoint.DistanceFromStart + distanceThatCanBeCovered;

              //  if(TotalDistanceCanBeCovered <= exitPoint.DistanceFromStart)   last station



                var NearestChargingPoint = chargeStationDetails.Where(x => x.DistanceFromStart >= entryPoint.DistanceFromStart && x.DistanceFromStart <= TotalDistanceCanBeCovered).LastOrDefault();

                var DistanceSaved= TotalDistanceCanBeCovered - NearestChargingPoint.DistanceFromStart;

                var KmsPerUnit = vehicleDetails.Mileage / vehicleDetails.NumberOfUnitsForFullyCharge;

                var unitsSaved = DistanceSaved / KmsPerUnit;

                var timeToCharge = timeToChargeDetails.Where(x => x.VehicleType == item.VehicleType && x.ChargingStation == NearestChargingPoint.ChargingStation).FirstOrDefault();

                var TotalTimeToCharge = (vehicleDetails.NumberOfUnitsForFullyCharge - unitsSaved) * timeToCharge.TimeToChargePerUnit; 
                
                var TotalEnergyConsumed = vehicleDetails.NumberOfUnitsForFullyCharge;  // this can change

                if(!con.ConsumptionDetails.Exists(x => x.VehicleType == vehicleDetails.VehicleType))
                {
                    con.ConsumptionDetails.Add(new ConsumptionDetails
                    {
                        VehicleType = vehicleDetails.VehicleType,
                        TotalTimeRequired= TotalTimeToCharge,
                        TotalUnitConsumed = vehicleDetails.NumberOfUnitsForFullyCharge,
                        NumberOfTripsFinished = 1
                    });
                }
                else
                {
                    var ConsumptionDetails = con.ConsumptionDetails.First(x => x.VehicleType == vehicleDetails.VehicleType);
                    ConsumptionDetails.TotalTimeRequired += TotalTimeToCharge;
                    ConsumptionDetails.TotalUnitConsumed += TotalEnergyConsumed;
                    ConsumptionDetails.NumberOfTripsFinished += 1;
                }   

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
