using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _dataAccess;

        public ProductData(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<ProductModel> GetProducts()
        {
            var output = _dataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = _dataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { id = productId }, "TRMData")
                .FirstOrDefault();

            return output;
        }
    }
}
