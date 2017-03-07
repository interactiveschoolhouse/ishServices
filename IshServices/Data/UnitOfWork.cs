using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IshServices.Data
{
    public class UnitOfWork : IDisposable
    {
        private MySqlConnection _cn;
        private MySqlTransaction _txn;
        public UnitOfWork(string connectionString)
        {
            _cn = new MySqlConnection(connectionString);
            _cn.Open();
            _txn = _cn.BeginTransaction();
        }

        public MySqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            MySqlCommand cmd = _cn.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            cmd.Transaction = _txn;

            return cmd;
        }

        private bool _committed = false;
        public void Commit()
        {
            _committed = true;
            _txn.Commit();
        }

        public void Dispose()
        {
            if (!_committed)
            {
                _txn.Rollback();
            }
            _txn.Dispose();
            _cn.Dispose();
        }
    }
}