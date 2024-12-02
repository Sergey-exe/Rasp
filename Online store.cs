using System;
using System.Collections.Generic;

internal class Program
{
    //    private static void Main(string[] args)
    //    {
    //        //Good iPhone12 = new Good("IPhone 12");
    //        //Good iPhone11 = new Good("IPhone 11");

    //        //Warehouse warehouse = new Warehouse();

    //        //Shop shop = new Shop(warehouse);

    //        //warehouse.Delive(iPhone12, 10);
    //        //warehouse.Delive(iPhone11, 1);

    //        //Console.WriteLine("Вывод всех товаров на складе с их остатком");
    //        //shop.ShowOll(); //Вывод всех товаров на складе с их остатком
    //        //Console.ReadKey();


    //        //Cart cart = shop.Cart();
    //        //cart.Add(iPhone12, 4);
    //        ////cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе
    //        //Console.ReadKey();

    //        //Console.WriteLine("Вывод всех товаров в корзине");
    //        ////Вывод всех товаров в корзине
    //        //cart.ShowOll();
    //        //Console.ReadKey();
    //        //shop.By(cart);

    //        //shop.ShowOll();
    //        //cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
    //        //Console.WriteLine("Ошибка, после заказа со склада убираются заказанные товары");
    //        //shop.By(cart);
    //        //Console.ReadKey();
    //    }
    //}

    //public class Warehouse
    //{
    //    private List<Good> _goods;

    //    public IReadOnlyList<Good> Goods => _goods;

    //    public Warehouse()
    //    {
    //        _goods = new List<Good>();
    //    }

    //    public void Delive(Good good, int countGoods)
    //    {
    //        if (good == null)
    //            throw new ArgumentNullException();

    //        if(countGoods <= 0)
    //            throw new ArgumentOutOfRangeException();

    //        for (int i = countGoods; i > 0; i--)
    //        {
    //            _goods.Add(good);
    //        }
    //    }

    //    public Good GetGood(Good userGood)
    //    {
    //        if(userGood == null)
    //            throw new ArgumentNullException();

    //        foreach(Good good in _goods)
    //        {
    //            if(good.Name == userGood.Name)
    //            {
    //                _goods.Remove(good);
    //                return good;
    //            }
    //        }

    //        throw new ArgumentNullException();
    //    }
    //}

    //public class Good
    //{
    //    public string Name { get; private set; }

    //    public Good(string name)
    //    {
    //        Name = name;
    //    }
    //}

    //public class Shop
    //{
    //    private Warehouse _warehouse;

    //    public Cart ShopCart { get; private set; }

    //    public Shop(Warehouse warehouse)
    //    {
    //        ShopCart = new Cart();

    //        _warehouse = warehouse;
    //    }

    //    public void ShowOll()
    //    {
    //        foreach (Good good in _warehouse.Goods)
    //        {
    //            Console.WriteLine(good.Name);
    //        }
    //    }

    //    public Cart Cart()
    //    {
    //        return ShopCart;
    //    }

    //    public void By(Cart cart)
    //    {
    //        List<Good> userGoods = new List<Good>();
    //        List<Good> shoopGoods = new List<Good>();
    //        List<Good> soldGoods = new List<Good>();
    //        userGoods.AddRange(cart.GetGoods());
    //        shoopGoods.AddRange(_warehouse.Goods);

    //        if(userGoods.Count <= 0)
    //            throw new IndexOutOfRangeException();

    //        foreach (Good soldGood in userGoods)
    //        {
    //            foreach (Good good in shoopGoods)
    //            {
    //                if (good.Name == soldGood.Name)
    //                {
    //                    soldGoods.Add(_warehouse.GetGood(good));
    //                }
    //            }
    //        }
    //    }
    //}

    //public class Cart
    //{
    //    private List<Good> _goods;

    //    public IReadOnlyList<Good> Goods => _goods;

    //    public Cart()
    //    {
    //        _goods = new List<Good>();
    //    }

    //    public void Add(Good good, int count)
    //    {
    //        if(good == null) 
    //            throw new ArgumentNullException();

    //        for (int i = 0; i < count; i++)
    //        {
    //            _goods.Add(good);
    //        }
    //    }

    //    public void ShowOll()
    //    {
    //        foreach (Good good in _goods) 
    //            Console.WriteLine(good.Name);
    //    }

    //    public IReadOnlyList<Good> GetGoods()
    //    {
    //        return Goods;
    //    }

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
