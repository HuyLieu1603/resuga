using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Common;
using PD_Store.Models.Product;
using PD_Store.ViewModels.Product;

namespace PD_Store.Repositories.Product
{
    public interface IProductService
    {
        public Task<DataResult<List<Products>>> GetListProduct();

        public Task<DataResult<Products>> GetProductById(int id);

        public Task<DataResult<bool>> CreateProduct(ProductVM product);

    }
}