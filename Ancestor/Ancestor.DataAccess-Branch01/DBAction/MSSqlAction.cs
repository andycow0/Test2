using Ancestor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Ancestor.DataAccess.Factory;

namespace Ancestor.DataAccess.DBAction
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/07/31 10:00
    /// Subject : MSSqlAction
    /// 
    /// History : 
    /// 2015/07/31 Andycow0 建立
    /// </summary>
    public class MSSqlAction : DbAction, IDbAction
    {
        public MSSqlAction()
        { }
        public MSSqlAction(DBObject dbObj)
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

        public bool Query(string sqlString, System.Collections.ICollection parameterCollection, ref DataTable dataTable)
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
