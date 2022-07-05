using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Internal.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAccess> _logger;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private bool isTransactionClosed;

        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void StartTransaction(string connectionStringName)
        {
            string connectionName = GetConnectionString(connectionStringName);

            _dbConnection = new SqlConnection(connectionName);
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();

            isTransactionClosed = false;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _dbConnection.Execute(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _dbTransaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _dbConnection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _dbTransaction).ToList();

            return rows;
        }

        public void CommitTransaction()
        {
            _dbTransaction?.Commit();
            _dbConnection?.Close();

            isTransactionClosed = true;
        }

        public void RollBackTransaction()
        {
            _dbTransaction?.Rollback();
            _dbConnection?.Close();

            isTransactionClosed = true;
        }

        public void Dispose()
        {
            if (isTransactionClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Commit transaction failed in the dispose method.");
                }
            }

            _dbTransaction = null;
            _dbConnection = null;
        }
    }
}
