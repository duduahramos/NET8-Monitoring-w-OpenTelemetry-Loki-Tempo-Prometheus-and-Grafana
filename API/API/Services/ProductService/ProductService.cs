using CentralizedLoggingAndTracingAPI.Services.ProductService.DTOs;
using Core.Data;

namespace CentralizedLoggingAndTracingAPI.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context; // database context

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // get a list of all products
        public IEnumerable<Product> GetAllProducts()
        {
            var products = _context.Products.ToList();
            return products;
        }

        // get a single product
        public Product GetProductById(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            return product;
        }

        // create a new product
        public bool CreateProduct(CreateProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.Add(product);
            return _context.SaveChanges() > 0;
        }


        // delete a product
        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product != null)
            {
                _context.Remove(product);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }

}
