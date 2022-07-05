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
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _dataAccess;

        public UserData(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<UserModel> GetUserById(string id)
        {
            var p = new
            {
                Id = id,
            };

            return _dataAccess.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");
        }
    }
}
