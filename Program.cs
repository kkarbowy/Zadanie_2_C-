using System;
using System.Collections.Generic;

class Product
{
    public string Sku { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string sku, string name, double price)
    {
        Sku = sku;
        Name = name;
        Price = price;
    }
}

class Inventory
{
    private Dictionary<Product, int> _stock = new Dictionary<Product, int>();

    public void AddStock(Product product, int quantity)
    {
        if (!_stock.ContainsKey(product)) _stock[product] = 0;
        _stock[product] += quantity;
    }

    public bool RemoveStock(Product product, int quantity)
    {
        int available = GetStock(product);
        if (quantity > available)
        {
            Console.WriteLine($"Brak towaru: {product.Name} (dostępne {available})");
            return false;
        }
        _stock[product] -= quantity;
        return true;
    }

    public int GetStock(Product product)
    {
        return _stock.ContainsKey(product) ? _stock[product] : 0;
    }
}

class ShoppingCart
{
    public Dictionary<Product, int> Items { get; private set; } = new Dictionary<Product, int>();

    public void AddProduct(Product product, int quantity = 1)
    {
        if (!Items.ContainsKey(product)) Items[product] = 0;
        Items[product] += quantity;
    }

    public void RemoveProduct(Product product, int quantity = 1)
    {
        if (Items.ContainsKey(product))
        {
            int newQ = Items[product] - quantity;
            if (newQ > 0)
                Items[product] = newQ;
            else
                Items.Remove(product);
        }
        else
        {
            Console.WriteLine($"Produktu {product.Name} nie ma w koszyku.");
        }
    }

    public double CalculateTotal()
    {
        double total = 0;
        foreach (var kvp in Items)
        {
            total += kvp.Key.Price * kvp.Value;
        }
        return total;
    }

    public void ListItems()
    {
        Console.WriteLine("Zawartość koszyka:");
        foreach (var kvp in Items)
        {
            Console.WriteLine($" - {kvp.Key.Name} x{kvp.Value}: {kvp.Key.Price:F2} zł/szt.");
        }
    }
}

class Order
{
    private ShoppingCart cart;
    private Inventory inventory;
    public string Status { get; private set; }
    public double Total { get; private set; }

    public Order(ShoppingCart cart, Inventory inventory)
    {
        this.cart = cart;
        this.inventory = inventory;
        Status = "Oczekujące";
        Total = cart.CalculateTotal();
    }

    public void Process()
    {
        Console.WriteLine("Przetwarzanie zamówienia...");
        foreach (var item in cart.Items)
        {
            if (inventory.GetStock(item.Key) < item.Value)
            {
                Status = "Nieudane";
                Console.WriteLine($"Nie można zrealizować: {item.Key.Name} (potrzeba {item.Value})");
                return;
            }
        }

        foreach (var item in cart.Items)
        {
            inventory.RemoveStock(item.Key, item.Value);
        }

        Status = "Zrealizowane";
        Console.WriteLine("Zamówienie zrealizowane.");
    }
}

class Program
{
    static void Main()
    {
        var tshirt = new Product("TSH001", "Koszulka Polo - M", 79.99);
        var jeans = new Product("JNS001", "Jeansy Slim - S", 199.50);
        var hat = new Product("HT001", "Czapka zimowa - S", 49.00);

        var inv = new Inventory();
        inv.AddStock(tshirt, 1);
        inv.AddStock(jeans, 5);
        inv.AddStock(hat, 20);

        var cart = new ShoppingCart();
        cart.AddProduct(tshirt, 3);
        cart.AddProduct(jeans, 1);
        cart.RemoveProduct(jeans, 1);
        cart.ListItems();
        Console.WriteLine($"Do zapłaty: {cart.CalculateTotal():F2} zł\n");

        var order = new Order(cart, inv);
        order.Process();
        Console.WriteLine($"Status zamówienia: {order.Status}");
        Console.WriteLine($"Pozostały stan Tshirt: {inv.GetStock(tshirt)} szt.");

        Console.WriteLine("\nUzupełniam stan koszulek...\n");
        inv.AddStock(tshirt, 15);
        order.Process();
        Console.WriteLine($"Status zamówienia: {order.Status}");
        Console.WriteLine($"Pozostały stan Tshirt: {inv.GetStock(tshirt)} szt.");
    }
}
