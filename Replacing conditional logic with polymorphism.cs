using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderForm orderForm = new OrderForm();
            PayService payService = new PayService(orderForm);
            payService.Start();

            Console.ReadKey();
        }
    }

    public class OrderForm
    {
        public string ShowForm(string namesSystems)
        {
            ShowInfo($"Мы принимаем: {namesSystems}");

            // симуляция веб интерфейса
            ShowInfo("Какое системой вы хотите совершить оплату?");

            return Console.ReadLine();
        }

        public void ShowInfo(string text)
        {
            Console.Write(text);
        }
    }

    public class PayService
    {
        private const string CommandQIWI = "QIWI";
        private const string CommandWebMoney = "WebMoney";
        private const string CommandCard = "Card";

        private OrderForm _orderForm;

        private IReadOnlyDictionary<string, IPaymentSystemFactory> _paymentFactories;

        public PayService(OrderForm orderForm)
        {
            _orderForm = orderForm ?? throw new ArgumentNullException();

            _paymentFactories = new Dictionary<string, IPaymentSystemFactory>()
            {
                { CommandQIWI, new QIWIFactory() },
                { CommandWebMoney, new WebMoneyFactory() },
                { CommandCard, new CardFactory() },
            };
        }

        public void Start()
        {
            try
            {
                Pay(_orderForm.ShowForm($"{CommandQIWI} {CommandWebMoney} {CommandCard}"));
            }
            catch (Exception exception)
            {
                _orderForm.ShowInfo(exception.Message);
            }
        }

        private void Pay(string command)
        {
            if (_paymentFactories.ContainsKey(command) == false)
                throw new ArgumentException("Попробуйте выбрать другую платёжную систему");

            _orderForm.ShowInfo($"Связь с банком {_paymentFactories[command]}...");
            PaymentHandler paymentHandler = new PaymentHandler(_paymentFactories[command]);

            _orderForm.ShowInfo($"Проверка платежа через {_paymentFactories[command]}...");

            if (paymentHandler.Pay())
                _orderForm.ShowInfo("Оплата прошла успешно!");

            throw new InvalidOperationException("К сожалению, оплата не прошла. Попробуйте ещё раз, или напишите в техподдержку");
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

        public bool Pay()
        {
            if (_paymentSystem == null)
                throw new ArgumentNullException();

            return _paymentSystem.Payment();
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
