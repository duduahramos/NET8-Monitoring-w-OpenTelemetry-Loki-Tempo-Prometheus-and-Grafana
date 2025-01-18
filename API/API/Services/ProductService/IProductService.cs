using CentralizedLoggingAndTracingAPI.Services.ProductService.DTOs;
using Core.Data;

namespace CentralizedLoggingAndTracingAPI.Services.ProductService
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        bool CreateProduct(CreateProductRequest request);
        bool DeleteProduct(int id);
    }
}
