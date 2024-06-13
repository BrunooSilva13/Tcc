using System;
using System.Collections.Generic;

public class SaleDTO
{
    public int ClientId { get; set; }
    public List<SaleItemDTO> Items { get; set; }
    public decimal TotalPrice { get; set; }

    public SaleDTO()
    {
        Items = new List<SaleItemDTO>();
    }
}

public class SaleItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
