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
        private readonly IInventoryData _data;

        public InventoryController(IInventoryData data)
        {
            _data = data;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            return _data.GetInventory();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            _data.SaveInventoryRecord(item);
        }
    }
}
