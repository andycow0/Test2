using Ancestor.Core;
using Ancestor.DataAccess.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Ancestor.DataAccess.DBAction
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/07/31 14:00
    /// Subject : MySqlAction 
    /// 
    /// History : 
    /// 2015/07/31 Andycow0 建立
    /// </summary>
    public class MySqlAction : DbAction, IDbAction
    {
        public MySqlAction()
        { }
        public MySqlAction(DBObject dbObj)
        {
            DbObject = dbObj;
        }

        public IDbConnection GetConnectionFactory()
        {
            IDBConnection conn = new ConnectionFactory(DbObject);
            return conn.GetConnectionFactory().GetConnectionObject();
        }

        public bool CheckConnectionState()
        {
            throw new NotImplementedException();
        }

        public bool Query(string sqlString, ICollection parameterCollection, ref DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteNonQuery(string sqlString, ICollection parameterCollection, ref int successRow)
        {
            throw new NotImplementedException();
        }

        
        public bool ExecuteStoredProcedure(string procedureName, bool bindbyName, ICollection parameterCollection, List<DBParameter> dBParameter)
        {
            throw new NotImplementedException();
        }
    }
}