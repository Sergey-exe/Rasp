using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        Product iPhone12 = new Product("IPhone 12");
        Product iPhone11 = new Product("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);

        warehouse.ShowInfo(); //Вывод всех товаров на складе с их остатком

        Cart cart = shop.GetCart();
        cart.Add(iPhone12, 10);
        cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        cart.ShowInfo(); //Вывод всех товаров в корзине

        Console.WriteLine(cart.Order().Paylink);

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

    public interface IProductSale
    {
        bool IsProductsInWarehouse(Product good, int countProducts);

        List<Product> ByProduct(Product good);
    }

    public class Warehouse : IProductSale
    {
        private Dictionary<Product, int> _catalogGoods;

        public IReadOnlyDictionary<Product, int> Goods => _catalogGoods;

        public Warehouse()
        {
            _catalogGoods = new Dictionary<Product, int>();
        }

        public void ShowInfo()
        {
            foreach (var good in _catalogGoods)
                Console.WriteLine($"{good.Key.Name}, {good.Value} шт.");
        }

        public void Delive(Product good, int countNewGoods)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (_catalogGoods.ContainsKey(good))
            {
                _catalogGoods[good] += countNewGoods;
            }
            else
            {
                _catalogGoods.Add(good, countNewGoods);
            }
        }

        public List<Product> ByProduct(Product good)
        {
            if (good == null)
                throw new ArgumentNullException();

            List<Product> gettingProducts = new List<Product>();

            if(IsProductsInWarehouse(good, 1))
            {
                gettingProducts.Add(good);

                _catalogGoods[good]--;


                if (_catalogGoods[good] == 0)
                    _catalogGoods.Remove(good);

                return gettingProducts;
            }
            
            throw new ArgumentNullException();
        }

        public bool IsProductsInWarehouse(Product good, int countProducts)
        {
            if (good == null)
                throw new ArgumentNullException();

            if(_catalogGoods.TryGetValue(good, out int count) == false)
                return false;

            if (count < countProducts)
                return false;

            return true;
        }
    }

    public class Cart
    {
        private IProductSale _warehouse;
        private List<Product> _goods;

        public Cart(IProductSale warehouse) 
        { 
            if (warehouse == null)
                throw new ArgumentNullException();

            _warehouse = warehouse;
            _goods = new List<Product>();
        }

        public void Add(Product good, int countProduct)
        {
            if (good == null)
                throw new ArgumentNullException();

            if (_warehouse.IsProductsInWarehouse(good, countProduct))
                for (int i = 0; i < countProduct; i++)
                    _goods.Add(good);
            else
                Console.WriteLine("Отсутствуют необходимые продукты на складе!");
        }

        public OrderInfo Order()
        {
            foreach (Product product in _goods)
                _warehouse.ByProduct(product);

            return new OrderInfo();
        }

        public void ShowInfo()
        {
            foreach (Product good in _goods)
                Console.WriteLine(good.Name);
        }
    }

    public class Product
    {
        public readonly string Name;

        public Product(string name)
        {
            if(name == null) 
                throw new ArgumentNullException();

            Name = name;
        }
    }

    public class OrderInfo
    {
        private const int _minNumber = 10000;
        private const int _maxNumber = 99999;

        public readonly string Paylink;

        public OrderInfo() 
        {
            Random random = new Random();

            Paylink = $"Заказ готов! Ваш номер: {random.Next(_minNumber, _maxNumber)}";
        }
    }
}
