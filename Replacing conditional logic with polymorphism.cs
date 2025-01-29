using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            PayService payService = new PayService();
            payService.Pay(Console.ReadLine());

            Console.ReadKey();
        }
    }

    public class PayService
    {
        private const string CommandQIWI = "QIWI";
        private const string CommandWebMoney = "WebMoney";
        private const string CommandCard = "Card";

        private IReadOnlyDictionary<string, IPaymentSystemFactory> _paymentFactories;

        public PayService()
        {
            _paymentFactories = new Dictionary<string, IPaymentSystemFactory>()
            {
                { CommandQIWI, new QIWIFactory() },
                { CommandWebMoney, new WebMoneyFactory() },
                { CommandCard, new CardFactory() },
            };
        }

        public void Pay(string command)
        {
            if (_paymentFactories.ContainsKey(command) == false)
                throw new ArgumentException("Попробуйте выбрать другую платёжную систему");

            PaymentHandler paymentHandler = new PaymentHandler(_paymentFactories[command]);
            paymentHandler.Pay();
        }
    }

    public class PaymentHandler
    {
        private IPaymentSystem _paymentSystem;

        public PaymentHandler(IPaymentSystemFactory paymentFactory)
        {
            if (paymentFactory == null)
                throw new ArgumentNullException();

            _paymentSystem = paymentFactory.Create();
        }

        public void Pay()
        {
            if (_paymentSystem == null)
                throw new ArgumentNullException();

            _paymentSystem.Payment();
        }
    }

    public class QIWIFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new QIWISystem();
        }
    }

    public class WebMoneyFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new WebMoneySystem();
        }
    }

    public class CardFactory : IPaymentSystemFactory
    {
        public IPaymentSystem Create()
        {
            return new CardSystem();
        }
    }

    public interface IPaymentSystemFactory
    {
        IPaymentSystem Create();
    }

    public class QIWISystem : IPaymentSystem
    {
        public bool Payment()
        {
            // Процесс оплаты данной системой
            return true;
        }
    }

    public class WebMoneySystem : IPaymentSystem
    {
        public bool Payment()
        {
            // Процесс оплаты данной системой
            return true;
        }
    }

    public class CardSystem : IPaymentSystem
    {
        public bool Payment()
        {
            // Процесс оплаты данной системой
            return true;
        }
    }

    public interface IPaymentSystem
    {
        bool Payment();
    }
}
