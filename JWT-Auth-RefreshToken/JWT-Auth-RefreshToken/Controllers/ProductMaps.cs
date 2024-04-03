namespace JWT_Auth_RefreshToken.Controllers;

public static class ProductMaps
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/products", () => ProductsService.Get())
            .RequireAuthorization();

        return endpoints;
    }
}

public static class ProductsService {
    public static IEnumerable<Product> Get() => new List<Product>()
        {
            new Product(1, "Product 01"),
            new Product(2, "Product 02"),
            new Product(3, "Product 03")
        };
}

public record Product(int Id, string Name);

