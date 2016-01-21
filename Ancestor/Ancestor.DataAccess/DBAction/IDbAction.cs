using Ancestor.Core;
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
    /// Author  : WizTonE 
    /// Date    : 2015/07/28
    /// Subject : IDBAction 訂定資料庫操作之Interface
    /// 
    /// History : 
    /// 2015/07/28 WizTonE建立
    /// 
    /// </summary>
    public interface IDbAction
    {
        string ErrorMessage { get; set; }
        IDbConnection GetConnectionFactory();
        bool CheckConnectionState();
        bool Query(string sqlString, ICollection parameterCollection, ref DataTable dataTable);
        bool ExecuteNonQuery(string sqlString, ICollection parameterCollection, ref int successRow);
        bool ExecuteStoredProcedure(string procedureName, bool bindbyName, ICollection parameterCollection, List<DBParameter> dBParameter);

    }
}
