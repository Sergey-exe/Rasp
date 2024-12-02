using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);

        warehouse.ShowInfo(); //Вывод всех товаров на складе с их остатком

        Cart cart = shop.GetCart();
        shop.AddElementInCart(iPhone12, 4);
        shop.AddElementInCart(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        //Вывод всех товаров в корзине
        cart.ShowInfo();

        //Console.WriteLine(cart.Order().Paylink);

        shop.AddElementInCart(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
    }

    public class Shop
    {
        private Warehouse _warehouse;

        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException();

            _cart = new Cart();
            _warehouse = warehouse;
        }

        public void AddElementInCart(Good good, int countGoods)
        {
            if (good == null)
                throw new ArgumentNullException();

            for (int i = 0; i < countGoods; i++)
                _cart.Add(_warehouse.GetGood(good.Name));
        }

        public Cart GetCart()
        {
            return _cart;
        }
    }

    public class Warehouse
    {
        private List<Good> _goods;

        public Warehouse()
        {
            _goods = new List<Good>();
        }

        public Good GetGood(string goodName)
        {
            Good gettingGood;

            foreach (Good good in _goods)
            {
                if(good.Name == goodName)
                {
                    gettingGood = good;
                    _goods.Remove(good);
                    return gettingGood;
                }
            }

            throw new ArgumentNullException();
        }

        public void ShowInfo()
        {
            foreach (Good good in _goods)
                Console.WriteLine(good.Name);
        }

        public void Delive(Good good, int countNewGoods)
        {
            if(good == null) 
                throw new ArgumentNullException();

            for (int i = 0; i < countNewGoods; i++)
                _goods.Add(good);
        }
    }

    public class Cart
    {
        private List<Good> _goods;

        public Cart() 
        { 
            _goods = new List<Good>();
        }

        public void Add(Good good)
        {
            if (good == null)
                throw new ArgumentNullException();

            _goods.Add(good);
        }

        public void ShowInfo()
        {
            foreach (Good good in _goods)
                Console.WriteLine(good.Name);
        }
    }

    public class Good
    {
        public readonly string Name;

        public Good(string name)
        {
            Name = name;
        }
    }
}
