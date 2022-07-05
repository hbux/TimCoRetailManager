using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IProductData _data;

        public SaleData(ISqlDataAccess dataAccess, IProductData data)
        {
            _dataAccess = dataAccess;
            _data = data;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDbModel> details = new List<SaleDetailDbModel>();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDbModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                var productInfo = _data.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable == true)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            SaleDbModel sale = new SaleDbModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            try
            {
                _dataAccess.StartTransaction("TRMData");
                _dataAccess.SaveDataInTransaction<SaleDbModel>("dbo.spSale_Insert", sale);
                sale.Id = _dataAccess.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate })
                    .FirstOrDefault();

                foreach (var item in details)
                {
                    item.SaleId = sale.Id;

                    _dataAccess.SaveDataInTransaction<SaleDetailDbModel>("dbo.spSaleDetail_Insert", item);
                }

                _dataAccess.CommitTransaction();
            }
            catch
            {
                _dataAccess.RollBackTransaction();

                throw;
            }
        }

        public List<SaleReportModel> GetSaleReport()
        {
            var output = _dataAccess.LoadData<SaleReportModel, dynamic>("spSale_SaleReport", new { }, "TRMData");

            return output;
        }
    }
}
