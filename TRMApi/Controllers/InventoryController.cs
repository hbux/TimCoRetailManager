using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(_config);

            return data.GetInventory();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData(_config);

            data.SaveInventoryRecord(item);
        }
    }
}
