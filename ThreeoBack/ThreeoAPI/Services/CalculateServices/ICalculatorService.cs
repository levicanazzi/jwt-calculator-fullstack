using ThreeoAPI.Models.Messages.Requests;
using ThreeoAPI.Models.Messages.Responses;

namespace ThreeoAPI.Services.CalculateServices
{
    public interface ICalculatorService
    {
        CalculateResponse PerformCalculation(CalculateRequest calculateRequest);
    }
}
