using ThreeoAPI.Models.Messages.Requests;
using ThreeoAPI.Models.Messages.Responses;

namespace ThreeoAPI.Services.CalculateServices
{
    public class CalculatorService : ICalculatorService
    {
        public CalculateResponse PerformCalculation(CalculateRequest calculateRequest)
        {
            if (calculateRequest == null)
            {
                throw new ArgumentNullException(nameof(calculateRequest));
            }

            double result = 0;

            switch (calculateRequest.Operation.ToLower())
            {
                case "sum":
                    result = calculateRequest.FirstValue + calculateRequest.SecondValue;
                    break;
                case "subtraction":
                    result = calculateRequest.FirstValue - calculateRequest.SecondValue;
                    break;
                case "multiplication":
                    result = calculateRequest.FirstValue * calculateRequest.SecondValue;
                    break;
                case "division":
                    result = calculateRequest.FirstValue / calculateRequest.SecondValue;
                    break;
                default:
                    throw new ArgumentException("Invalid Operation", nameof(calculateRequest.Operation));
            }

            var response = new CalculateResponse()
            {
                Result = result,
                FirstValue = calculateRequest.FirstValue,
                SecondValue = calculateRequest.SecondValue,
                Operation = calculateRequest.Operation
            };

            return response;
        }
    }

}
