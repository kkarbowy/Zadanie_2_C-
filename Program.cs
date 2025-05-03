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
