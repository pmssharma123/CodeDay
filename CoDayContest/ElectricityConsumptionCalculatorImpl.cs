using CoDayContest.Interfaces;
using CoDayContest.Models;
using CsvHelper;
using System.Formats.Asn1;

namespace CoDayContest
{
    public class ElectricityConsumptionCalculatorImpl : IElectricityComsumptionCalculator
    {
        IEnumerable<ChargeStationDetails>? chargeStationDetails = null;
        IEnumerable<TripDetails>? tripDetailsInfo = null;
        IEnumerable<EntryExitDetails>? entryExitDetails = null;
        IEnumerable<VehicleTypeInfo>? vehicleTypeInfo = null;
        IEnumerable<TimeToChargeDetails>? timeToChargeDetails = null;
        

        public async Task<ConsumptionResult> CalculateElectricityAndTimeComsumption(ResourcesInfo resourcesInfo)
        {
            ConsumptionResult con = new ();
            try
            {
                chargeStationDetails = await readCSV<ChargeStationDetails>(resourcesInfo.ChargingStationInfoFilePath);
                tripDetailsInfo = await readCSV<TripDetails>(resourcesInfo.TripDetailsFilePath);
                entryExitDetails = await readCSV<EntryExitDetails>(resourcesInfo.EntryExitPointInfoFilePath);
                vehicleTypeInfo = await readCSV<VehicleTypeInfo>(resourcesInfo.VehicleTypeInfoFilePath);
                timeToChargeDetails = await readCSV<TimeToChargeDetails>(resourcesInfo.TimeToChargeVehicleInfoFilePath);

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

                    int remainingBatteryInUnits = (item.RemainingBatteryPercentage * vehicleDetails.NumberOfUnitsForFullyCharge) / 100;
                    int distanceThatCanBeCovered = (vehicleDetails.Mileage * remainingBatteryInUnits) / vehicleDetails.NumberOfUnitsForFullyCharge;

                    var TotalDistanceCanBeCovered = entryPoint.DistanceFromStart + distanceThatCanBeCovered;
                    int KmsPerUnit = vehicleDetails.Mileage / vehicleDetails.NumberOfUnitsForFullyCharge;

                    do
                    {
                        int distanceRemained;
                        int unitsSaved;
                        int totalTimeToCharge;
                        int totalEnergyConsumed;
                        int tripsFinished = 0;
                        var nextEntryPoint = entryExitDetails.Where(x => x.DistanceFromStart <= TotalDistanceCanBeCovered && x.DistanceFromStart <= exitPoint.DistanceFromStart).LastOrDefault();

                        var NearestChargingPoint = chargeStationDetails!.Where(x => x.DistanceFromStart >= entryExitDetail.DistanceFromStart && x.DistanceFromStart <= TotalDistanceCanBeCovered).LastOrDefault();

                        if (nextEntryPoint.EntryExitPoint == exitPoint.EntryExitPoint || TotalDistanceCanBeCovered >= exitPoint.DistanceFromStart)
                        {
                            distanceRemained = TotalDistanceCanBeCovered - exitPoint.DistanceFromStart;
                            unitsSaved = distanceRemained / KmsPerUnit;
                            totalTimeToCharge = 0;
                            totalEnergyConsumed = remainingBatteryInUnits - unitsSaved;
                            tripsFinished = 1;
                        }
                        else if(NearestChargingPoint == null)
                        {
                            break;
                        }
                        else
                        {
                            distanceRemained = TotalDistanceCanBeCovered - NearestChargingPoint.DistanceFromStart;

                            var timeToCharge = timeToChargeDetails.Where(x => x.VehicleType == item.VehicleType && x.ChargingStation == NearestChargingPoint.ChargingStation).FirstOrDefault();

                            unitsSaved = distanceRemained / KmsPerUnit;

                            totalTimeToCharge = (vehicleDetails.NumberOfUnitsForFullyCharge - unitsSaved) * timeToCharge.TimeToChargePerUnit;

                            totalEnergyConsumed = remainingBatteryInUnits - unitsSaved;
                        }


                        if (!con.ConsumptionDetails.Exists(x => x.VehicleType == vehicleDetails.VehicleType))
                        {
                            con.ConsumptionDetails.Add(new ConsumptionDetails
                            {
                                VehicleType = vehicleDetails.VehicleType,
                                TotalTimeRequired = totalTimeToCharge,
                                TotalUnitConsumed = totalEnergyConsumed,
                                NumberOfTripsFinished = tripsFinished
                            });
                        }
                        else
                        {
                            var ConsumptionDetails = con.ConsumptionDetails.First(x => x.VehicleType == vehicleDetails.VehicleType);
                            ConsumptionDetails.TotalTimeRequired += totalTimeToCharge;
                            ConsumptionDetails.TotalUnitConsumed += totalEnergyConsumed;
                            ConsumptionDetails.NumberOfTripsFinished += tripsFinished;
                        }

                        if (NearestChargingPoint != null)
                        {
                            if (!con.TotalChargingStationTime.ContainsKey(NearestChargingPoint.ChargingStation))
                            {
                                con.TotalChargingStationTime.Add(NearestChargingPoint.ChargingStation, (long)totalTimeToCharge);
                            }
                            else
                            {
                                con.TotalChargingStationTime[NearestChargingPoint.ChargingStation] += (long)totalTimeToCharge;
                            }

                        }
                        TotalDistanceCanBeCovered = vehicleDetails.Mileage + NearestChargingPoint.DistanceFromStart;
                        entryExitDetail.DistanceFromStart = NearestChargingPoint.DistanceFromStart;
                        entryExitDetail.EntryExitPoint = nextEntryPoint.EntryExitPoint;
                        remainingBatteryInUnits = vehicleDetails.NumberOfUnitsForFullyCharge;

                    }
                    while (exitPoint.EntryExitPoint != entryExitDetail.EntryExitPoint);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }   

            return con;
        }

        private async Task<List<T>> readCSV<T>(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<T>();
                    List<T> list = records.ToList();
                    return list;
                }
            }
            catch (CsvHelperException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }   
        }

    }
}
