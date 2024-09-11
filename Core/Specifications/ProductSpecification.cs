using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{

    public ProductSpecification(string? brand, string? category, string? sort) : base(x =>
        (string.IsNullOrEmpty(brand) || x.Brand == brand) &&
        (string.IsNullOrEmpty(category) || x.Category == category)
    )
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderDescBy(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }

}
