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
    public class UserData
    {
        private IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }

        public List<UserModel> GetUserById(string id)
        {
            // For now, we'll use the direct dependency and add DI later
            SqlDataAccess sql = new SqlDataAccess(_config);

            var p = new
            {
                Id = id,
            };

            return sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");
        }
    }
}
