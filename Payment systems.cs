using System;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Order order = new Order(1945, 26600000);
        MD5HashCreator mD5HashCreator = new MD5HashCreator();
        SHA1HashCreator sHA1HashCreator = new SHA1HashCreator();

        PayGetter1 payGetter1 = new PayGetter1(mD5HashCreator);
        PayGetter2 payGetter2 = new PayGetter2(mD5HashCreator);
        PayGetter3 payGetter3 = new PayGetter3(0000, sHA1HashCreator);

        Console.WriteLine(payGetter1.GetPayingLink(order));
        Console.WriteLine(payGetter2.GetPayingLink(order));
        Console.WriteLine(payGetter3.GetPayingLink(order));

        Console.ReadKey();
    }
}

public class Order
{
    public Order(int id, int amount)
    {
        Id = id;
        Amount = amount;
    }

    public int Id { get; private set; }

    public int Amount { get; private set; }
}

public interface IPaymentSystem
{
    string GetPayingLink(Order order);
}

public interface IHashCreator
{
    string GetStringHash(string hash);
}

public abstract class HashCreator : IHashCreator
{
    protected abstract byte[] HashData(byte[] bytes);

    public string GetStringHash(string line)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(line);

        return Encoding.UTF8.GetString(HashData(bytes));
    }
}

public class MD5HashCreator : HashCreator, IHashCreator
{
    protected override byte[] HashData(byte[] bytes)
    {
        MD5 md5 = MD5.Create();

        return md5.ComputeHash(bytes);
    }
}

public class SHA1HashCreator : HashCreator, IHashCreator
{
    protected override byte[] HashData(byte[] bytes)
    {
        SHA1 sha1 = SHA1.Create();

        return sha1.ComputeHash(bytes);
    }
}

public class PayGetter1 : IPaymentSystem
{
    private IHashCreator _hashCreator;

    public PayGetter1(IHashCreator hashCreator)
    {
        if (hashCreator == null)
            throw new ArgumentNullException();

        _hashCreator = hashCreator;
    }

    public string GetPayingLink(Order order)
    {
        return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={_hashCreator.GetStringHash((order.Id.ToString()))}";
    }
}

public class PayGetter2 : IPaymentSystem
{
    private IHashCreator _hashCreator;

    public PayGetter2(IHashCreator hashCreator)
    {
        if (hashCreator == null)
            throw new ArgumentNullException();

        _hashCreator = hashCreator;
    }

    public string GetPayingLink(Order order)
    {
        return $"order.system2.ru/pay?hash={_hashCreator.GetStringHash(order.Id.ToString())}+{order.Amount}";
    }
}

public class PayGetter3 : IPaymentSystem
{
    private IHashCreator _hashCreator;

    private readonly int Key;

    public PayGetter3(int key, IHashCreator hashCreator)
    {
        if (hashCreator == null)
            throw new ArgumentNullException();

        _hashCreator = hashCreator;
        Key = key;
    }

    public string GetPayingLink(Order order)
    {
        return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={_hashCreator.GetStringHash(order.Amount.ToString())}+{order.Id}+{Key}";
    }
}
