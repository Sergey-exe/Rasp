using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            var presenterFactory = new PresenterFactory();
            var view = new OrderView(presenterFactory);
            view.ShowForm();

            Console.ReadKey();
        }
    }

    public class OrderView
    {
        private Presenter _presenter;

        public OrderView(PresenterFactory presenterFactory)
        {
            if (presenterFactory == null)
                throw new ArgumentNullException();

            _presenter = presenterFactory.Create(this);
        }

        public void ShowForm()
        {
            _presenter.Start();
            _presenter.TryPay(Console.ReadLine());
        }

        public void ShowInfo(string text)
        {
            Console.WriteLine(text);
        }
    }

    public class Presenter
    {
        private PayService _payService;
        private OrderView _view;

        public Presenter(OrderView view)
        {
            _view = view ?? throw new ArgumentNullException();
            _payService = new PayService();
        }

        public void Start()
        {
            _view.ShowInfo($"Мы принимаем: {_payService.GetServicesNames()}");
            _view.ShowInfo("Какое системой вы хотите совершить оплату?");
        }

        public void TryPay(string command)
        {
            _view.ShowInfo($"Вызов вашей системы платежа...");

            try
            {
                _view.ShowInfo($"Вы оплатили с помощью {command}\nПроверка платежа через {command}...");
                _payService.TryPay(command);
            }
            catch (Exception exception)
            {
                _view.ShowInfo(exception.Message);

                return;
            }

            _view.ShowInfo("Оплата прошла успешно!");
        }
    }

    public class PayService
    {
        private const string CommandQIWI = "QIWI";
        private const string CommandWebMoney = "WebMoney";
        private const string CommandCard = "Card";

        private IReadOnlyDictionary<string, IPaymentSystem> _systems;

        public PayService()
        {
            _systems = new Dictionary<string, IPaymentSystem>()
            {
                { CommandQIWI, new QIWISystem() },
                { CommandWebMoney, new WebMoneySystem() },
                { CommandCard, new CardSystem() },
            };
        }

        public void TryPay(string command)
        {
            IPaymentSystem paymentSystem;

            if (_systems.ContainsKey(command) == false)
                throw new ArgumentException("Попробуйте выбрать другую платёжную систему");

            paymentSystem = _systems[command];

            if (paymentSystem.Payment() == false)
                throw new InvalidOperationException("К сожалению оплата не прошла! Попробуйте ещё раз или напишите в техподдержку");
        }

        public string GetServicesNames()
        {
            return $"{CommandQIWI} {CommandWebMoney} {CommandCard}";
        }
    }

    public interface IPaymentSystem
    {
        bool Payment();
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

    public class PresenterFactory
    {
        public Presenter Create(OrderView view)
        {
            if (view == null)
                throw new ArgumentNullException();

            return new Presenter(view);
        }
    }
}
