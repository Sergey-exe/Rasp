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
        cart.Add(iPhone12, 10);
        cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        //Вывод всех товаров в корзине
        cart.ShowInfo();

        //Console.WriteLine(cart.Order().Paylink);

        cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
    }

    public class Shop
    {
        private Warehouse _warehouse;

        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException();

            _warehouse = warehouse;
            _cart = new Cart(_warehouse);
        }

        public Cart GetCart()
        {
            return _cart;
        }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _catalogGoods;

        public IReadOnlyDictionary<Good, int> Goods => _catalogGoods;

        public Warehouse()
        {
            _catalogGoods = new Dictionary<Good, int>();
        }

        public void ShowInfo()
        {
            foreach (var good in _catalogGoods)
                Console.WriteLine($"{good.Key.Name}, {good.Value} шт.");
        }

        public void Delive(Good good, int countNewGoods)
        {
            if (good == null)
                throw new ArgumentNullException();

            for (int i = 0; i < countNewGoods; i++)
            {
                if (_catalogGoods.ContainsKey(good))
                {
                    _catalogGoods[good]++;
                }
                else
                {
                    _catalogGoods.Add(good, 1);
                }
            }
        }

        public List<Good> ByProduct(Good good, int countProducts)
        {
            if (good == null)
                throw new ArgumentNullException();

            List<Good> gettingProducts = new List<Good>();

            if(IsProductInWarehouse(good, countProducts))
            {
                for (int i = 0; i <= countProducts; i++)
                {
                    gettingProducts.Add(good);
                }

                _catalogGoods[good] -= countProducts;


                if (countProducts == 0)
                    _catalogGoods.Remove(good);

                return gettingProducts;
            }
            
            throw new ArgumentNullException();
        }

        private bool IsProductInWarehouse(Good good, int countProducts)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (_catalogGoods.ContainsKey(good) == false)
                return false;

            if (_catalogGoods[good] >= countProducts)
                return true;

            return false;
        }
    }

    public class Cart
    {
        private Warehouse _warehouse;

        private List<Good> _goods;

        public Cart(Warehouse warehouse) 
        { 
            if (warehouse == null)
                throw new ArgumentNullException();

            _warehouse = warehouse;
            _goods = new List<Good>();
        }

        public void Add(Good good, int countProduct)
        {
            if (good == null)
                throw new ArgumentNullException();

            _goods.AddRange(_warehouse.ByProduct(good, countProduct));
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
