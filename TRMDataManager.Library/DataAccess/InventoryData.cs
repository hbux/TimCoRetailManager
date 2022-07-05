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
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _dataAccess;

        public InventoryData(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<InventoryModel> GetInventory()
        {
            var output = _dataAccess.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "TRMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            _dataAccess.SaveData<InventoryModel>("spInventory_Insert", item, "TRMData");
        }
    }
}
