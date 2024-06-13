using System;
using System.Collections.Generic;

public class Sale
{
    public int SaleId { get; set; }
    public int ClientId { get; set; }
    public List<SaleItem> Items { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime SaleDate { get; set; }

    public Sale()
    {
        Items = new List<SaleItem>();
    }
}
