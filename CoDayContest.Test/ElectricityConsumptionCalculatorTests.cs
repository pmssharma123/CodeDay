using System.Linq;
using CoDayContest.Interfaces;
using CoDayContest.Models;

namespace CoDayContest.Test
{
    public class ElectricityConsumptionCalculatorTests
    {
        

        static String artifactsDirectory = Environment.CurrentDirectory + @"\..\..\..\..\CoDayContest\Resources\";
        

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Test1()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase1\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase1\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase1\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase1\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase1\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);
            
            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 690.60;
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(1));
            
            //Total Unit Consume by Vehicle Type V1
            double expectedTotalUnitsConsumedByV1 = 534.06;
            double actualTotalUnitsConsumedByV1 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V1")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV1, Is.EqualTo(expectedTotalUnitsConsumedByV1).Within(1));

            //Total Unit Consume by Vehicle Type V2
            double expectedTotalUnitsConsumedByV2 = 156.53;
            double actualTotalUnitsConsumedByV2 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V2")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV2, Is.EqualTo(expectedTotalUnitsConsumedByV2).Within(1));

            //Total Time required for charging Vehicle Type V2
            Int64 expectedTotalTimeRequiredByV2 = 55022;
            Int64 actualTotaTotalTimeRequiredByV2 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V2")
                .Sum(cd => cd.TotalTimeRequired);
            Assert.That(actualTotaTotalTimeRequiredByV2, Is.EqualTo(expectedTotalTimeRequiredByV2).Within(50));

            //Total time required for charging any vehicle at Charging Station Ch2
            Int64 expectedTotalTimeRequiredAtC2 = 10570;
            Int64 actualTotalTimeRequiredAtC2 = resultData.TotalChargingStationTime["C2"];
            Assert.That(actualTotalTimeRequiredAtC2, Is.EqualTo(expectedTotalTimeRequiredAtC2).Within(50));

            //Total time required for charging any vehicle at Charging Station Ch10
            Int64 expectedTotalTimeRequiredAtC10 = 46500;
            Int64 actualTotalTimeRequiredAtC10 = resultData.TotalChargingStationTime["C10"];
            Assert.That(actualTotalTimeRequiredAtC10, Is.EqualTo(expectedTotalTimeRequiredAtC10).Within(50));

            //Number of trips finished
            Int64 expectedNumberOfTripsFinished = 16;
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));

        }

        [Test]
        public void Test2()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase2\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase2\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase2\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase2\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase2\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);
            
            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 3972;
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(2));

            //Total Unit Consume by Vehicle Type V4
            double expectedTotalUnitsConsumedByV4 = 670.27;
            double actualTotalUnitsConsumedByV4 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V4")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV4, Is.EqualTo(expectedTotalUnitsConsumedByV4).Within(1));

            //Total Time required for charging Vehicle Type V1
            Int64 expectedTotalTimeRequiredByV1 = 238437; 
            Int64 actualTotaTotalTimeRequiredByV1 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V1")
                .Sum(cd => cd.TotalTimeRequired);
            Assert.That(actualTotaTotalTimeRequiredByV1, Is.EqualTo(expectedTotalTimeRequiredByV1).Within(120));

        }

        [Test]
        public void Test3()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase3\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase3\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase3\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase3\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase3\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);
            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 0;
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(0));

            //Number of trips finished by verhicle type V2
            Int64 expectedNumberOfTripsFinished = 0; 
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));

        }


        [Test]
        public void Test4()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase4\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase4\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase4\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase4\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase4\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);
            
            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 736;
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(2));
            
            //Total Unit Consume by Vehicle Type V4
            double expectedTotalUnitsConsumedByV4 = 174; 
            double actualTotalUnitsConsumedByV4 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V4")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV4, Is.EqualTo(expectedTotalUnitsConsumedByV4).Within(1));

            //Total Time required for charging Vehicle Type V2
            Int64 expectedTotalTimeRequiredByV2 = 11649; 
            Int64 actualTotaTotalTimeRequiredByV2 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V2")
                .Sum(cd => cd.TotalTimeRequired);
            Assert.That(actualTotaTotalTimeRequiredByV2, Is.EqualTo(expectedTotalTimeRequiredByV2).Within(60));


            //Number of trips finished
            Int64 expectedNumberOfTripsFinished = 40;
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));
        }

        [Test]
        public void Test5()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase5\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase5\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase5\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase5\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase5\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);

            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 351797; 
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(10));

            //Total Unit Consume by Vehicle Type V4
            double expectedTotalUnitsConsumedByV4 = 39110;
            double actualTotalUnitsConsumedByV4 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V4")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV4, Is.EqualTo(expectedTotalUnitsConsumedByV4).Within(10));

            //Total Time required by Vehicle Type V2
            Int64 expectedTotalTimeRequiredByV2 = 10716795;
            Int64 actualTotaTotalTimeRequiredByV2 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V2")
                .Sum(cd => cd.TotalTimeRequired);
            Assert.That(actualTotaTotalTimeRequiredByV2, Is.EqualTo(expectedTotalTimeRequiredByV2).Within(600));


            //Number of trips finished
            Int64 expectedNumberOfTripsFinished = 10000;
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));
        }


        [Test]
        public void Test6()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase6\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase6\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase6\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase6\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase6\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);

            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 2741580;
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(50));

            //Total Unit Consume by Vehicle Type V16
            double expectedTotalUnitsConsumedByV3 = 224771;
            double actualTotalUnitsConsumedByV3 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V16")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV3, Is.EqualTo(expectedTotalUnitsConsumedByV3).Within(10));

            //Total Time required for charging Vehicle Type V12
            Int64 expectedTotalTimeRequiredByV2 = 41961924;
            Int64 actualTotaTotalTimeRequiredByV2 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V12")
                .Sum(cd => cd.TotalTimeRequired);
            Assert.That(actualTotaTotalTimeRequiredByV2, Is.EqualTo(expectedTotalTimeRequiredByV2).Within(600));


            //Number of trips finished
            Int64 expectedNumberOfTripsFinished = 29795;
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));
        }

        [Test]
        public void Test7()
        {
            ResourcesInfo resourcesInfo = new ResourcesInfo()
            {
                ChargingStationInfoFilePath = artifactsDirectory + @"TestCase7\ChargingStationInfo.csv",
                EntryExitPointInfoFilePath = artifactsDirectory + @"TestCase7\EntryExitPointInfo.csv",
                TimeToChargeVehicleInfoFilePath = artifactsDirectory + @"TestCase7\TimeToChargeVehicleInfo.csv",
                TripDetailsFilePath = artifactsDirectory + @"TestCase7\TripDetails.csv",
                VehicleTypeInfoFilePath = artifactsDirectory + @"TestCase7\VehicleTypeInfo.csv",
            };

            IElectricityComsumptionCalculator electricityComsumption = new ElectricityConsumptionCalculatorImpl();
            var resultData = electricityComsumption.CalculateElectricityAndTimeComsumption(resourcesInfo);

            //Total Unit Consume by all vehicles
            double expectedTotalUnitsConsumed = 12823458; 
            double actualTotalUnitsConsumed = resultData.ConsumptionDetails.Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumed, Is.EqualTo(expectedTotalUnitsConsumed).Within(200));
            
            //Total Unit Consume by Vehicle Type V29
            double expectedTotalUnitsConsumedByV1 = 143514.85; 
            double actualTotalUnitsConsumedByV1 = resultData.ConsumptionDetails
                .Where(cd => cd.VehicleType == "V29")
                .Sum(cd => cd.TotalUnitConsumed);
            Assert.That(actualTotalUnitsConsumedByV1, Is.EqualTo(expectedTotalUnitsConsumedByV1).Within(20));

            //Total time required for charging any vehicle at Charging Station C183
            Int64 expectedTotalTimeRequiredAtC10 = 22999411;
            Int64 actualTotalTimeRequiredAtC10 = resultData.TotalChargingStationTime["C183"];
            Assert.That(actualTotalTimeRequiredAtC10, Is.EqualTo(expectedTotalTimeRequiredAtC10).Within(720));

            //Number of trips finished
            Int64 expectedNumberOfTripsFinished = 99227;
            Int64 actualNumberOfTripsFinished = resultData.ConsumptionDetails
                .Sum(cd => cd.NumberOfTripsFinished);
            Assert.That(actualNumberOfTripsFinished, Is.EqualTo(expectedNumberOfTripsFinished));
        }



    }
}