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