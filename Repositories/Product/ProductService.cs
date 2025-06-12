using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Common;
using Microsoft.EntityFrameworkCore;
using PD_Store.DbContextFolder;
using PD_Store.Helper;
using PD_Store.Models.Product;
using PD_Store.ViewModels.Product;

namespace PD_Store.Repositories.Product
{
    public class ProductService : IProductService
    {
        private readonly AdminDbContext _context;

        private readonly ILogger _logger;
        public ProductService(AdminDbContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger<ProductService>();
        }

        //Get list product
        public async Task<DataResult<List<Products>>> GetListProduct()
        {
            try
            {
                var listProduct = await _context.Products.Include(x => x.ListProductImages).ToListAsync();

                return new DataResult<List<Products>>
                {
                    Status = Contants.StatusCodeSuccessed,
                    Message = "Product list retrieved successfully.",
                    Data = listProduct
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product list.");
                return new DataResult<List<Products>>
                {
                    Status = Contants.StatusCodeInternalServerError,
                    Message = "An error occurred while retrieving the product list.",
                };
            }
        }

        //Get product by id
        public async Task<DataResult<Products>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(x => x.ListProductImages).FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                {
                    return new DataResult<Products>
                    {
                        Status = Contants.StatusCodeNotFound,
                        Message = "Product not found."
                    };
                }

                return new DataResult<Products>
                {
                    Status = Contants.StatusCodeSuccessed,
                    Message = "Product retrieved successfully.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product by id.");
                return new DataResult<Products>
                {
                    Status = Contants.StatusCodeInternalServerError,
                    Message = "An error occurred while retrieving the product."
                };
            }
        }

        public async Task<DataResult<bool>> CreateProduct(ProductVM product)
        {
            try
            {
                _context.Products.Add(new Products
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Weight = product.Weight,
                    NRC = product.NRC,
                    STC = product.STC,
                    FireResistance = product.FireResistance,
                    StockQuantity = product.StockQuantity,
                    Desc = product.Desc,
                    ListProductImages = product.ListProductImages?.Select(img => new ProductImages
                    {
                        ImageLink = img.ImageLink
                    }).ToList()
                });
                await _context.SaveChangesAsync();

                return new DataResult<bool>
                {
                    Status = Contants.StatusCodeSuccessed,
                    Message = "Product created successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return new DataResult<bool>
                {
                    Status = Contants.StatusCodeInternalServerError,
                    Message = "An error occurred while creating the product."
                };
            }

        }
    }
}