﻿using Microsoft.AspNetCore.Authorization;
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
    public class ProductController : ControllerBase
    {
        private IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        public List<ProductModel> Get()
        {
            ProductData data = new ProductData(_config);

            return data.GetProducts();
        }
    }
}
