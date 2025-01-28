using System;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            var presenterFactory = new PresenterFactory();
            var view = new View(presenterFactory);
            view.ShowForm();

            Console.ReadKey();
        }
    }

    public class View
    {
        private Presenter _presenter;

        public View(PresenterFactory presenterFactory)
        {
            if (presenterFactory == null)
                throw new ArgumentNullException();

            _presenter = presenterFactory.Create(this);
        }

        public void ShowForm()
        {
            ShowInfo("Мы принимаем: QIWI, WebMoney, Card");
            //симуляция веб интерфейса
            ShowInfo("Какое системой вы хотите совершить оплату?");

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
        private View _view;

        public Presenter(View view)
        {
            _view = view ?? throw new ArgumentNullException();
            _payService = new PayService();
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

        public void TryPay(string command)
        {
            IPaymentSystem paymentSystem;

            switch (command)
            {
                default:
                    throw new ArgumentException("Попробуйте выбрать другую платёжную систему");

                case CommandQIWI:
                    paymentSystem = new QIWISystem();
                    break;

                case CommandWebMoney:
                    paymentSystem = new WebMoneySystem();
                    break;

                case CommandCard:
                    paymentSystem = new CardSystem();
                    break;
            }

            if (paymentSystem.Payment() == false)
                throw new InvalidOperationException("К сожалению оплата не прошла! Попробуйте ещё раз или напишите в техподдержку");
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
        public Presenter Create(View view)
        {
            if (view == null)
                throw new ArgumentNullException();

            return new Presenter(view);
        }
    }
}
