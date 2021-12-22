using System;
using System.Collections.Generic;

namespace Calculator.Task3
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

    public abstract class BaseCalculatorDecorator: ICalculator
    {
        protected ICalculator calculator;

        public BaseCalculatorDecorator(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public virtual decimal CalculatePayment(string touristName)
        {
            return calculator.CalculatePayment(touristName);
        }
    }

    public class RoundingCalculatorDecorator : BaseCalculatorDecorator
    {
        readonly int rounding;

        public RoundingCalculatorDecorator(ICalculator calculator): base(calculator)
        {
            rounding = 0;
        }

        public override decimal CalculatePayment(string touristName)
        {
            var payment = base.CalculatePayment(touristName);
            return Math.Round(payment, rounding);
        }
    }

    public class LoggingCalculatorDecorator: BaseCalculatorDecorator
    {
        readonly ILogger logger;

        public LoggingCalculatorDecorator(ICalculator calculator, ILogger logger): base(calculator)
        {
            this.logger = logger;
        }

        public override decimal CalculatePayment(string touristName)
        {
            logger.Log("Start");
            var payment = base.CalculatePayment(touristName);
            logger.Log("End");

            return payment;
        }
    }

    public class CachedPaymentDecorator : BaseCalculatorDecorator
    {
        readonly Dictionary<string, decimal> paymentsCache = new Dictionary<string, decimal>();

        public CachedPaymentDecorator(ICalculator calculator): base(calculator)
        {
        }

        public override decimal CalculatePayment(string touristName)
        {
            if (!paymentsCache.TryGetValue(touristName, out var payment))
            {
                payment = base.CalculatePayment(touristName);
                paymentsCache.Add(touristName, payment);
            }

            return payment;
        }
    }
}
