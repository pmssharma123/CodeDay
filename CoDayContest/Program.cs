using CoDayContest.Interfaces;
using CoDayContest.Models;

namespace CoDayContest
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo();
            resourcesInfo.TripDetailsFilePath = "C:\\Users\\jgujar\\Downloads\\CodersDuo_DotNet\\CoDayContest\\Resources\\TestCase1\\TripDetails.csv";
            resourcesInfo.EntryExitPointInfoFilePath = "C:\\Users\\jgujar\\Downloads\\CodersDuo_DotNet\\CoDayContest\\Resources\\TestCase1\\EntryExitPointInfo.csv";
            resourcesInfo.ChargingStationInfoFilePath = "C:\\Users\\jgujar\\Downloads\\CodersDuo_DotNet\\CoDayContest\\Resources\\TestCase1\\ChargingStationInfo.csv";
            resourcesInfo.TimeToChargeVehicleInfoFilePath = "C:\\Users\\jgujar\\Downloads\\CodersDuo_DotNet\\CoDayContest\\Resources\\TestCase1\\TimeToChargeVehicleInfo.csv";
            resourcesInfo.VehicleTypeInfoFilePath = "C:\\Users\\jgujar\\Downloads\\CodersDuo_DotNet\\CoDayContest\\Resources\\TestCase1\\VehicleTypeInfo.csv";

            IElectricityComsumptionCalculator electricityComsumptionCalculator = new ElectricityConsumptionCalculatorImpl();
            //ConsumptionResult consumptionResult = electricityComsumptionCalculator.CalculateElectricityAndTimeComsumption(resourcesInfo);
            
        }
    }
}
