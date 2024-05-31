using CoDayContest.Models;

namespace CoDayContest.Interfaces
{
    public interface IElectricityComsumptionCalculator
    {
       Task<ConsumptionResult> CalculateElectricityAndTimeComsumption(ResourcesInfo resourcesInfo);
    }
}
