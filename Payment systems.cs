using System;

class Program
{
    static void Main(string[] args)
    {
        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}
    }
}

public class Order
{
    public int Id { get; private set; }
    
    public int MD5Hash { get; private set; }

    public Order(int id, int hash) => (Id, MD5Hash) = (id, hash);
}

public interface IPaymentSystem
{
    string GetPayingLink(Order order);
}

public class PayGetter1 : IPaymentSystem
{
    public string GetPayingLink(Order order)
    {
        return $"pay.system1.ru/order?amount=12000RUB&hash={order.MD5Hash} {order.Id}";
    }
}

public class PayGetter2 : IPaymentSystem
{
    public int Amount { get; private set; } 

    public string GetPayingLink(Order order)
    {
        return $"order.system2.ru/pay?hash={order.MD5Hash} {order.Id} {Amount}";
    }
}

public class PayGetter3 : IPaymentSystem
{
    public int Amount { get; private set; }

    public string GetPayingLink(Order order)
    {
        return $"system3.com/pay?amount=12000&curency=RUB&hash={order.MD5Hash} {order.Id} {Amount}";
    }
}
