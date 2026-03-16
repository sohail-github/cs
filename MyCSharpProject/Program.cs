using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure port
builder.WebHost.UseUrls("http://localhost:3000", "https://localhost:3001");

// Add services to the container
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "A simple Product API for CRUD operations"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        c.RoutePrefix = "swagger"; // Access at /swagger
    });
}
else
{
    app.UseHttpsRedirection();
}

// ==================== REST API ENDPOINTS ====================

// Root endpoint
app.MapGet("/", () => Results.Ok(new { message = "Welcome to Product API", version = "1.0" }))
.WithName("Root");

// GET all products
app.MapGet("/api/products", (IProductService service) =>
{
    return Results.Ok(service.GetAllProducts());
})
.WithName("GetProducts");

// GET product by ID
app.MapGet("/api/products/{id}", (int id, IProductService service) =>
{
    var product = service.GetProductById(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById");

// POST - Create new product
app.MapPost("/api/products", (CreateProductRequest request, IProductService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
    {
        return Results.BadRequest("Name is required and price must be greater than 0");
    }

    var product = service.CreateProduct(request.Name, request.Price, request.Description);
    return Results.Created($"/api/products/{product.Id}", product);
})
.WithName("CreateProduct");

// PUT - Update product
app.MapPut("/api/products/{id}", (int id, UpdateProductRequest request, IProductService service) =>
{
    var product = service.UpdateProduct(id, request.Name, request.Price, request.Description);
    return product != null ? Results.Ok(product) : Results.NotFound();
})
.WithName("UpdateProduct");

// DELETE - Remove product
app.MapDelete("/api/products/{id}", (int id, IProductService service) =>
{
    var success = service.DeleteProduct(id);
    return success ? Results.Ok("Product deleted successfully") : Results.NotFound();
})
.WithName("DeleteProduct");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
.WithName("HealthCheck");

app.Run();

// ==================== DATA MODELS ====================

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}

// ==================== SERVICE INTERFACE & IMPLEMENTATION ====================

public interface IProductService
{
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    Product CreateProduct(string name, decimal price, string description);
    Product? UpdateProduct(int id, string name, decimal price, string description);
    bool DeleteProduct(int id);
}

public class ProductService : IProductService
{
    // In-memory database (in real app, use actual database)
    private static List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High-performance laptop", CreatedAt = DateTime.UtcNow },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m, Description = "Wireless mouse", CreatedAt = DateTime.UtcNow },
        new Product { Id = 3, Name = "Keyboard", Price = 79.99m, Description = "Mechanical keyboard", CreatedAt = DateTime.UtcNow }
    };

    private static int _nextId = 4;

    public List<Product> GetAllProducts()
    {
        return _products.OrderBy(p => p.Id).ToList();
    }

    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public Product CreateProduct(string name, decimal price, string description)
    {
        var product = new Product
        {
            Id = _nextId++,
            Name = name,
            Price = price,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
        _products.Add(product);
        return product;
    }

    public Product? UpdateProduct(int id, string name, decimal price, string description)
    {
        var product = GetProductById(id);
        if (product == null) return null;

        product.Name = name;
        product.Price = price;
        product.Description = description;
        return product;
    }

    public bool DeleteProduct(int id)
    {
        var product = GetProductById(id);
        if (product == null) return false;

        _products.Remove(product);
        return true;
    }
}