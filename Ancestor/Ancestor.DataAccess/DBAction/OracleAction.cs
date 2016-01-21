using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using Ancestor.Core;
using Ancestor.DataAccess.Factory;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Ancestor.DataAccess.DBAction
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/07/28
    /// Subject : Oracle Action
    /// 
    /// History : 
    /// 2015/07/28 WizTonE 建立
    /// 2015/09/01 Andycow0, 因發現 delete 時, 未回傳影響欄位數量, 而增加 ExecuteNonQuery 回傳 effectRows 的值
    /// </summary>
    public class OracleAction : DbAction, IDbAction
    {
        OracleConnection DbConnection { get; set; }
        OracleCommand DbCommand { get; set; }
        OracleDataAdapter adapter { get; set; }
        string testString { get; set; }

        public IDbConnection GetConnectionFactory()
        {
            IDBConnection conn = new ConnectionFactory(DbObject);
            return conn.GetConnectionFactory().GetConnectionObject();
        }

        public bool CheckConnectionState()
        {
            throw new NotImplementedException();
        }

        public OracleAction()
        {

        }

        public OracleAction(DBObject _dBObject)
        {
            DbObject = _dBObject;
            DbConnection = (OracleConnection)GetConnectionFactory();
            testString = "select 1 from dual";
        }

        public bool Query(string sqlString, ICollection parameterCollection, ref DataTable dataTable)
        {
            bool is_success = false;
            ErrorMessage = string.Empty;
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = sqlString;
            adapter = new OracleDataAdapter();
            //DbCommand.BindByName = true;
            //DbCommand.AddRowid = true;

            if (CheckConnection(DbConnection, DbCommand, testString))
            {
                try
                {
                    var parameters = (List<OracleParameter>) parameterCollection;
                    DbCommand.Parameters.AddRange(parameters.ToArray());
                    adapter.SelectCommand = DbCommand;
                    adapter.Fill(dataTable);
                    is_success = true;
                }
                catch (Exception exception)
                {
                    is_success = false;
                    ErrorMessage = exception.ToString();
                }
            }
            DbConnection.Close();
            return is_success;
        }

        public bool ExecuteNonQuery(string sqlString, ICollection parameterCollection, ref int effectRows)
        {
            bool isSuccessful = false;
            ErrorMessage = string.Empty;
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = sqlString;
            //DbCommand.BindByName = true;
            DbCommand.AddRowid = true;

            if (CheckConnection(DbConnection, DbCommand, testString))
            {
                try
                {
                    var parameters = (List<OracleParameter>)parameterCollection;
                    DbCommand.Parameters.AddRange(parameters.ToArray());
                    DbCommand.CommandText = sqlString;
                    // 2015-09-01
                    //DbCommand.ExecuteNonQuery();
                    effectRows = DbCommand.ExecuteNonQuery();
                    isSuccessful = true;
                }
                catch (Exception exception)
                {
                    isSuccessful = false;
                    ErrorMessage = exception.ToString();
                }
            }
            DbConnection.Close();
            return isSuccessful;
        }

        public bool ExecuteStoredProcedure(string procedureName, bool bindbyName, ICollection parameterCollection, List<DBParameter> dBParameter)
        {
            bool is_success = false;
            ErrorMessage = string.Empty;
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = procedureName;
            DbCommand.CommandType = CommandType.StoredProcedure;
            DbCommand.BindByName = bindbyName;
            //DbCommand.AddRowid = true;

            if (CheckConnection(DbConnection, DbCommand, testString))
            {
                try
                {
                    var parameters = (List<OracleParameter>)parameterCollection;
                    DbCommand.Parameters.AddRange(parameters.ToArray());
                    DbCommand.ExecuteNonQuery();
                    is_success = true;

                    foreach (DBParameter Parameter in dBParameter)
                    {
                        if (Parameter.ParameterDirection == ParameterDirection.Output)
                        {
                            foreach (OracleParameter OPara in DbCommand.Parameters)
                            {
                                if (OPara.Direction == ParameterDirection.Output)
                                {
                                    if (OPara.ParameterName == Parameter.Name)
                                    {
                                        if (OPara.OracleDbType == OracleDbType.RefCursor)
                                        {
                                            adapter = new OracleDataAdapter(DbCommand);
                                            DataTable dt = new DataTable("Result");
                                            adapter.Fill(dt, (OracleRefCursor)OPara.Value);
                                            Parameter.Value = dt;
                                        }
                                        else
                                            Parameter.Value = OPara.Value;
                                    }
                                }
                            }
                        }
                        if (Parameter.ParameterDirection == ParameterDirection.InputOutput)
                        {
                            foreach (OracleParameter OPara in DbCommand.Parameters)
                            {
                                if (OPara.Direction == ParameterDirection.InputOutput)
                                {
                                    if (OPara.ParameterName == Parameter.Name)
                                    {
                                        if (OPara.OracleDbType == OracleDbType.RefCursor)
                                        {
                                            adapter = new OracleDataAdapter(DbCommand);
                                            DataTable dt = new DataTable("Result");
                                            adapter.Fill(dt, (OracleRefCursor)OPara.Value);
                                            Parameter.Value = dt;
                                        }
                                        else
                                            Parameter.Value = OPara.Value;
                                    }
                                }
                            }
                        }
                        if (Parameter.ParameterDirection == ParameterDirection.ReturnValue)
                        {
                            foreach (OracleParameter OPara in DbCommand.Parameters)
                            {
                                if (OPara.Direction == ParameterDirection.ReturnValue)
                                {
                                    if (OPara.ParameterName == Parameter.Name)
                                    {
                                        if (OPara.OracleDbType == OracleDbType.RefCursor)
                                        {
                                            adapter = new OracleDataAdapter(DbCommand);
                                            DataTable dt = new DataTable("Result");
                                            adapter.Fill(dt, (OracleRefCursor)OPara.Value);
                                            Parameter.Value = dt;
                                        }
                                        else
                                            Parameter.Value = OPara.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    is_success = false;
                    ErrorMessage = exception.ToString();
                }
            }
            DbConnection.Close();
            return is_success;
        }
    }
}
