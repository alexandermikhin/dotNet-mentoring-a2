using System.Collections.Generic;

namespace Calculator.Task2
{
    public class InsurancePaymentCalculator : ICalculator
    {
        private readonly ICurrencyService currencyService;
        private readonly ITripRepository tripRepository;

        public InsurancePaymentCalculator(
            ICurrencyService currencyService,
            ITripRepository tripRepository)
        {
            this.currencyService = currencyService;
            this.tripRepository = tripRepository;
        }

        public decimal CalculatePayment(string touristName)
        {
            var tripDetails = tripRepository.LoadTrip(touristName);
            var rate = currencyService.LoadCurrencyRate();

            return Constants.A * rate * tripDetails.FlyCost +
                Constants.B * rate * tripDetails.AccomodationCost +
                Constants.C * rate * tripDetails.ExcursionCost;
        }
    }

    public class CachedInsurancePaymentCalculator : ICalculator
    {
        readonly ICalculator calculator;
        readonly Dictionary<string, decimal> paymentsCache = new Dictionary<string, decimal>();

        public CachedInsurancePaymentCalculator(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public decimal CalculatePayment(string touristName)
        {
            if (!paymentsCache.TryGetValue(touristName, out var payment))
            {
                payment = calculator.CalculatePayment(touristName);
                paymentsCache.Add(touristName, payment);
            }

            return payment;
        }
    }
}
