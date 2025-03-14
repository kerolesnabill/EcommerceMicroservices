﻿namespace ProductService.Features.Products;

public class CreateProductCommand : IRequest<Guid>
{
    public Guid SellerId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = default!;
    public int StockQuantity { get; set; } = default!;
    public string Category { get; set; } = default!;
    public List<string> Tags { get; set; } = [];
}
