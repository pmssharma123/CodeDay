using CoDayContest.Models;

namespace CoDayContest.Interfaces
{
    public interface IElectricityComsumptionCalculator
    {
        ConsumptionResult CalculateElectricityAndTimeComsumption(ResourcesInfo resourcesInfo);
    }
}
