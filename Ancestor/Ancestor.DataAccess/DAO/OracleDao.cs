using Ancestor.Core;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Ancestor.DataAccess.DBAction;

namespace Ancestor.DataAccess.DAO
{
    // 2015-10-22 1. GetDbType() 加入 SYSTEM.DATETIME 型態的轉換
    public class OracleDao : DataAccessObject, IDataAccessObject
    {
        public OracleDao()
        {
            SqlString = new StringBuilder();
        }

        public OracleDao(DBObject _dBObject)
            : this()
        {
            DbObject = _dBObject;
            DB = GetActionFactory();
            DbSymbolize = ":";
            DbLikeSymbolize = "||";
        }
        internal override object GetDbType(string typeString)
        {
            switch (typeString.ToUpper())
            {
                case "VARCHAR2": return OracleDbType.Varchar2;
                case "STRING": return OracleDbType.Varchar2;
                case "SYSTEM.DATETIME":     // 2015-10-22 加入 SYSTEM.DATETIME 型態的轉換
                case "DATETIME": return OracleDbType.Date;
                case "DATE": return OracleDbType.Date;
                case "INT64": return OracleDbType.Int64;
                case "INT32": return OracleDbType.Int32;
                case "INT16": return OracleDbType.Int16;
                case "BYTE": return OracleDbType.Byte;
                case "DECIMAL": return OracleDbType.Decimal;
                case "FLOAT": return OracleDbType.Single;
                case "DOUBLE": return OracleDbType.Double;
                case "BYTE[]": return OracleDbType.Blob;
                case "CHAR": return OracleDbType.Char;
                case "CHAR[]": return OracleDbType.Char;
                case "TIMESTAMP": return OracleDbType.TimeStamp;
                case "REFCURSOR": return OracleDbType.RefCursor;
                default: return OracleDbType.Varchar2;
            }
        }
        public AncestorResult Query<T>(IModel objectModel) where T : class,IModel, new()
        {
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();

            try
            {
                SqlString.Clear();
                // 2015-08-31
                //sqlString = QueryStringGenerator(objectModel, parameters);
                var tableName = objectModel.GetType().Name;
                SqlString.Append("SELECT " + tableName + ".*, ROWID FROM " + tableName);
                var sqlWhereCondition = ParseWhereCondition(objectModel, parameters);
                SqlString.Append(sqlWhereCondition);

                isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                returnResult.Message = DB.ErrorMessage;
                returnResult.DataList = dataTable.ToList<T>();
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }
        public AncestorResult QueryNoRowid<T>(IModel objectModel) where T : class,IModel, new()
        {
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();

            try
            {
                SqlString.Clear();
                // 2015-08-31
                //sqlString = QueryStringGenerator(objectModel, parameters);
                var tableName = objectModel.GetType().Name;
                SqlString.Append("SELECT * FROM " + tableName);
                var sqlWhereCondition = ParseWhereCondition(objectModel, parameters);
                SqlString.Append(sqlWhereCondition);

                isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                returnResult.Message = DB.ErrorMessage;
                returnResult.DataList = dataTable.ToList<T>();
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }

        public AncestorResult Query<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            string whereString = string.Empty;
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();
            SqlString = new StringBuilder();

            using (LambdaExpressionHelper helper = new LambdaExpressionHelper(DbSymbolize, DbLikeSymbolize))
            {

                try
                {
                    var rootExp = predicate.Body as Expression;
                    whereString = helper.Translate(rootExp);
                    var Parameters = helper.Parameters;
                    var tableName = new T().GetType().Name;

                    SqlString.Append("SELECT " + tableName + ".*, ROWID FROM " + tableName);
                    SqlString.Append(whereString);

                    var paras = from parameter in Parameters
                                select new OracleParameter(parameter.Name, (OracleDbType)GetDbType(parameter.Type),
                              parameter.Value, ParameterDirection.Input);
                    parameters.AddRange(paras);

                    isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                    returnResult.Message = DB.ErrorMessage;
                    returnResult.DataList = dataTable.ToList<T>();
                }
                catch (Exception exception)
                {
                    returnResult.Message = exception.ToString();
                    isSuccess = false;
                }
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }

        public AncestorResult QueryNoRowid<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            string whereString = string.Empty;
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();
            SqlString = new StringBuilder();

            using (LambdaExpressionHelper helper = new LambdaExpressionHelper(DbSymbolize, DbLikeSymbolize))
            {

                try
                {
                    var rootExp = predicate.Body as Expression;
                    whereString = helper.Translate(rootExp);
                    var Parameters = helper.Parameters;
                    var tableName = new T().GetType().Name;

                    SqlString.Append("SELECT * FROM " + tableName);
                    SqlString.Append(whereString);

                    var paras = from parameter in Parameters
                                select new OracleParameter(parameter.Name, (OracleDbType)GetDbType(parameter.Type),
                              parameter.Value, ParameterDirection.Input);
                    parameters.AddRange(paras);

                    isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                    returnResult.Message = DB.ErrorMessage;
                    returnResult.DataList = dataTable.ToList<T>();
                }
                catch (Exception exception)
                {
                    returnResult.Message = exception.ToString();
                    isSuccess = false;
                }
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }

        public AncestorResult Query(IModel objectModel)
        {
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();
            SqlString = new StringBuilder();

            try
            {
                // 2015-08-31
                //sqlString = QueryStringGenerator(objectModel, parameters);
                SqlString.Clear();
                var tableName = objectModel.GetType().Name;
                SqlString.Append("SELECT " + tableName + ".*, ROWID FROM " + tableName);
                var sqlWhereCondition = ParseWhereCondition(objectModel, parameters);
                SqlString.Append(sqlWhereCondition);
                sqlString = SqlString.ToString();

                isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                returnResult.Message = DB.ErrorMessage;
                returnResult.ReturnDataTable = dataTable;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }

        public AncestorResult QueryNoRowid(IModel objectModel)
        {
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();
            SqlString = new StringBuilder();

            try
            {
                // 2015-08-31
                //sqlString = QueryStringGenerator(objectModel, parameters);
                SqlString.Clear();
                var tableName = objectModel.GetType().Name;
                SqlString.Append("SELECT * FROM " + tableName);
                var sqlWhereCondition = ParseWhereCondition(objectModel, parameters);
                SqlString.Append(sqlWhereCondition);
                sqlString = SqlString.ToString();

                isSuccess = DB.Query(SqlString.ToString(), parameters, ref dataTable);
                returnResult.Message = DB.ErrorMessage;
                returnResult.ReturnDataTable = dataTable;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }

        public AncestorResult Query(string sqlString, object paramsObjects)
        {
            var isSuccess = false;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();

            try
            {
                //foreach (var prop in paramsObjects.GetType().GetProperties())
                //{
                //    var propertyType = prop.PropertyType;
                //    parameters.Add(
                //            new OracleParameter(":" + prop.Name, (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(paramsObjects, null), ParameterDirection.Input)
                //            );
                //}
                //2015-10-12 null 的參數
                if (paramsObjects != null)
                {
                    var paras = from prop in paramsObjects.GetType().GetProperties()
                                select
                                    new OracleParameter(DbSymbolize + prop.Name, (OracleDbType)GetDbType(prop.PropertyType.Name),
                                        prop.GetValue(paramsObjects, null), ParameterDirection.Input);

                    parameters.AddRange(paras);
                }

                isSuccess = DB.Query(sqlString, parameters, ref dataTable);
                returnResult.Message = DB.ErrorMessage;
                returnResult.ReturnDataTable = dataTable;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }

            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }

        public AncestorResult Insert(IModel objectModel)
        {
            SqlString = new StringBuilder();
            var sqlValueString = new StringBuilder();
            var effectRows = 0;
            var parameters = new List<OracleParameter>();
            var returnResult = new AncestorResult();
            var isSuccess = false;

            SqlString.Append("INSERT INTO " + objectModel.GetType().Name + " (");
            foreach (PropertyInfo prop in objectModel.GetType().GetProperties())
            {
                if (prop.GetValue(objectModel, null) != null)
                {
                    if (CheckBrowsable(objectModel, prop.Name))
                    {
                        SqlString.Append(prop.Name.ToUpper() + ",");
                        sqlValueString.Append(DbSymbolize + prop.Name.ToUpper() + ",");
                        var propertyType = prop.PropertyType;
                        if (prop.PropertyType.IsGenericType &&
                                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = prop.PropertyType.GetGenericArguments()[0];

                        parameters.Add(new OracleParameter(DbSymbolize + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(objectModel, null), ParameterDirection.Input));
                    }
                }
            }
            SqlString.Remove(SqlString.Length - 1, 1);
            sqlValueString.Remove(sqlValueString.Length - 1, 1);
            SqlString.Append(") ");
            SqlString.Append("values ");
            SqlString.Append("(");
            SqlString.Append(sqlValueString);
            SqlString.Append(")");
            try
            {
                isSuccess = DB.ExecuteNonQuery(SqlString.ToString(), parameters, ref effectRows);
                returnResult.EffectRows = effectRows;
                returnResult.Message = DB.ErrorMessage;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }

            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }

        public AncestorResult Update(IModel valueObject, IModel whereObject)
        {
            SqlString = new StringBuilder();
            var sb2 = new StringBuilder();
            var effectRows = 0;
            var parameters = new List<OracleParameter>();
            var returnResult = new AncestorResult();
            var isSuccess = false;
            var tableName = valueObject.GetType().Name;

            try
            {
                SqlString.Append("UPDATE " + tableName + " set ");
                // 2015-09-03 update set 欄位語法, 重構為 UpdateTranslate method.
                UpdateTranslate(valueObject, parameters);

                //foreach (PropertyInfo prop in valueObject.GetType().GetProperties())
                //{
                //    if (prop.GetValue(valueObject, null) != null)
                //    {
                //        if (CheckBrowsable(valueObject, prop.Name))
                //        {
                //            //SqlStringBuilder.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + ",");
                //            SqlString.Append(prop.Name.ToUpper() + " = " + DbSymbolize + prop.Name.ToUpper() + ",");

                //            var propertyType = prop.PropertyType;

                //            if (prop.PropertyType.IsGenericType &&
                //                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //                propertyType = prop.PropertyType.GetGenericArguments()[0];

                //            //如果obj value非null但長度為0, 代表需為NULL, 以DBnull.Value傳值
                //            parameters.Add(new OracleParameter(DbSymbolize + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(valueObject, null).ToString().Length > 0 ? prop.GetValue(valueObject, null) : DBNull.Value, ParameterDirection.Input));
                //        }

                //    }
                //}

                // 2015-08-31
                //if (whereObject != null)
                //{
                //    sb2.Append(" WHERE ");
                //    foreach (PropertyInfo prop in whereObject.GetType().GetProperties())
                //    {
                //        var propertyType = prop.PropertyType;
                //        if (prop.PropertyType.IsGenericType &&
                //                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //            propertyType = prop.PropertyType.GetGenericArguments()[0];

                //        if (prop.GetValue(whereObject, null) != null)
                //        {
                //            if (prop.Name.ToUpper() == "ROWID")
                //            {
                //                sb2.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + "1" + " and ");
                //                parameters.Add(new OracleParameter(":" + prop.Name.ToUpper() + "1", (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(whereObject, null).ToString().Length > 0 ? prop.GetValue(whereObject, null) : DBNull.Value, ParameterDirection.Input));
                //            }
                //            else
                //            {
                //                sb2.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + " and ");
                //                parameters.Add(new OracleParameter(":" + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(whereObject, null).ToString().Length > 0 ? prop.GetValue(whereObject, null) : DBNull.Value, ParameterDirection.Input));
                //            }
                //        }
                //    }
                //}

                //SqlString.Remove(SqlString.Length - 1, 1);
                //sb2.Remove(sb2.Length - 4, 4);
                //SqlStringBuilder.Append(sb2);
                var sqlWhereCondition = ParseWhereCondition(whereObject, parameters);
                SqlString.Append(sqlWhereCondition);

                if (string.IsNullOrEmpty(sqlWhereCondition))
                    throw new ArgumentNullException("All properties of model aren't allowed to be null for updating columns.");

                isSuccess = DB.ExecuteNonQuery(SqlString.ToString(), parameters, ref effectRows);
                returnResult.EffectRows = effectRows;
                returnResult.Message = DB.ErrorMessage;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }

            returnResult.IsSuccess = isSuccess;
            return returnResult;

        }

        public AncestorResult Update<T>(IModel valueObject, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            string whereString = string.Empty;
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var effectRows = 0;
            var tableName = valueObject.GetType().Name;
            SqlString = new StringBuilder();

            SqlString.Append("UPDATE " + tableName + " set ");
            // 2015-09-03 update set 欄位語法, 重構為 UpdateTranslate method.
            UpdateTranslate(valueObject, parameters);

            using (LambdaExpressionHelper helper = new LambdaExpressionHelper(DbSymbolize, DbLikeSymbolize))
            {
                try
                {
                    var rootExp = predicate.Body as Expression;
                    whereString = helper.Translate(rootExp);
                    var expParameters = helper.Parameters;

                    sqlString += SqlString.ToString();
                    sqlString += whereString;

                    var paras = from p in expParameters
                                select new OracleParameter(p.Name, (OracleDbType)GetDbType(p.Type),
                              p.Value, ParameterDirection.Input);
                    parameters.AddRange(paras);

                    isSuccess = DB.ExecuteNonQuery(sqlString, parameters, ref effectRows);
                    returnResult.Message = DB.ErrorMessage;
                    returnResult.EffectRows = effectRows;
                }
                catch (Exception exception)
                {
                    returnResult.Message = exception.ToString();
                    isSuccess = false;
                }
            }
            returnResult.IsSuccess = isSuccess;

            return returnResult;
        }
        public AncestorResult Delete(IModel whereObject)
        {
            SqlString = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();
            var effectRows = 0;
            var parameters = new List<OracleParameter>();
            var returnResult = new AncestorResult();
            var isSuccess = false;

            try
            {
                // 2015-08-31
                // SqlStringBuilder.Append("DELETE FROM " + whereObject.GetType().Name);

                //if (whereObject != null)
                //{
                //    sb2.Append(" WHERE ");
                //    foreach (PropertyInfo prop in whereObject.GetType().GetProperties())
                //    {
                //        if (prop.GetValue(whereObject, null) != null)
                //        {
                //            if (prop.Name.ToUpper() == "ROWID")
                //                sb2.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + "1" + " and ");
                //            else
                //                sb2.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + " and ");
                //            //檢查Nullable
                //            //若為Nullable,則型態設為prop.PropertyType.GetGenericArguments()[0]
                //            //否則仍為prop.PropertyType
                //            var propertyType = prop.PropertyType;
                //            if (prop.PropertyType.IsGenericType &&
                //                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //                propertyType = prop.PropertyType.GetGenericArguments()[0];

                //            if (prop.Name.ToUpper() == "ROWID")
                //                parameters.Add(new OracleParameter(":" + prop.Name.ToUpper() + "1", (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(whereObject, null).ToString().Length > 0 ? prop.GetValue(whereObject, null) : DBNull.Value, ParameterDirection.Input));
                //            else
                //                parameters.Add(new OracleParameter(":" + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(whereObject, null).ToString().Length > 0 ? prop.GetValue(whereObject, null) : DBNull.Value, ParameterDirection.Input));
                //        }
                //    }
                //}

                //sb2.Remove(sb2.Length - 4, 4);
                //SqlStringBuilder.Append(sb2);

                var tableName = whereObject.GetType().Name;
                SqlString.Append("DELETE FROM " + tableName);
                var sqlWhereCondition = ParseWhereCondition(whereObject, parameters);

                if (string.IsNullOrEmpty(sqlWhereCondition))
                    throw new ArgumentNullException("All properties of model aren't allowed to be null for deleting columns.");

                SqlString.Append(sqlWhereCondition);

                isSuccess = DB.ExecuteNonQuery(SqlString.ToString(), parameters, ref effectRows);
                returnResult.EffectRows = effectRows;
                returnResult.Message = DB.ErrorMessage;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }

            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }
        public AncestorResult Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            string whereString = string.Empty;
            var isSuccess = false;
            var sqlString = string.Empty;
            var returnResult = new AncestorResult();
            var parameters = new List<OracleParameter>();
            var dataTable = new DataTable();
            var effectRows = 0;

            using (LambdaExpressionHelper helper = new LambdaExpressionHelper(DbSymbolize, DbLikeSymbolize))
            {
                try
                {
                    var rootExp = predicate.Body as Expression;
                    whereString = helper.Translate(rootExp);
                    var Parameters = helper.Parameters;

                    var tableName = new T().GetType().Name;
                    sqlString = "DELETE FROM " + tableName;
                    // 2015-09-03
                    sqlString += whereString;

                    var paras = from parameter in Parameters
                                select new OracleParameter(parameter.Name, (OracleDbType)GetDbType(parameter.Type),
                              parameter.Value, ParameterDirection.Input);
                    parameters.AddRange(paras);

                    isSuccess = DB.ExecuteNonQuery(sqlString, parameters, ref effectRows);
                    returnResult.Message = DB.ErrorMessage;
                    returnResult.EffectRows = effectRows;
                }
                catch (Exception exception)
                {
                    returnResult.Message = exception.ToString();
                    isSuccess = false;
                }
            }
            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }
        public AncestorResult ExecuteNonQuery(string sqlString, object modelObject)
        {
            SqlString = new StringBuilder();
            var effectRows = 0;
            var parameters = new List<OracleParameter>();
            var returnResult = new AncestorResult();
            var isSuccess = false;

            if (modelObject != null)
            {
                foreach (PropertyInfo prop in modelObject.GetType().GetProperties())
                {
                    if (prop.GetValue(modelObject, null) != null)
                    {
                        //檢查Nullable
                        //若為Nullable,則型態設為prop.PropertyType.GetGenericArguments()[0]
                        //否則仍為prop.PropertyType
                        var propertyType = prop.PropertyType;
                        if (prop.PropertyType.IsGenericType &&
                                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = prop.PropertyType.GetGenericArguments()[0];
                        if (prop.Name.ToUpper() == "ROWID")
                            parameters.Add(new OracleParameter(DbSymbolize + prop.Name.ToUpper() + "1", (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(modelObject, null).ToString().Length > 0 ? prop.GetValue(modelObject, null) : DBNull.Value, ParameterDirection.Input));
                        else
                            parameters.Add(new OracleParameter(DbSymbolize + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(modelObject, null).ToString().Length > 0 ? prop.GetValue(modelObject, null) : DBNull.Value, ParameterDirection.Input));
                    }
                }
            }
            if (SqlString.ToString().IndexOf(":ROWID") > 0)
                SqlString = SqlString.Replace(":ROWID", ":ROWID1");

            try
            {
                isSuccess = DB.ExecuteNonQuery(SqlString.ToString(), parameters, ref effectRows);
                returnResult.EffectRows = effectRows;
                returnResult.Message = DB.ErrorMessage;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }

            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }
        public AncestorResult ExecuteStoredProcedure(string procedureName, bool bindbyName, List<DBParameter> dBParameter)
        {
            var parameters = new List<OracleParameter>();
            var returnResult = new AncestorResult();
            var isSuccess = false;

            try
            {
                foreach (DBParameter Parameter in dBParameter)
                {
                    if (Parameter.ParameterDirection == ParameterDirection.Input)
                    {
                        parameters.Add(new OracleParameter()
                        {
                            ParameterName = Parameter.Name,
                            OracleDbType = (OracleDbType) GetDbType(Parameter.Type),
                            Value = Parameter.Value.ToString().Length > 0 ? Parameter.Value : DBNull.Value,
                            Direction = ParameterDirection.Input,
                            Size = Parameter.Size
                        });
                    }
                    if (Parameter.ParameterDirection == ParameterDirection.Output)
                    {
                        parameters.Add(new OracleParameter()
                        {
                            ParameterName = Parameter.Name,
                            OracleDbType = (OracleDbType)GetDbType(Parameter.Type),
                            Direction = ParameterDirection.Output,
                            Size = Parameter.Size
                        });
                    }
                    if (Parameter.ParameterDirection == ParameterDirection.InputOutput)
                    {
                        parameters.Add(new OracleParameter()
                        {
                            ParameterName = Parameter.Name,
                            OracleDbType = (OracleDbType)GetDbType(Parameter.Type),
                            Value = Parameter.Value.ToString().Length > 0 ? Parameter.Value : DBNull.Value,
                            Direction = ParameterDirection.InputOutput,
                            Size = Parameter.Size
                        });
                    }
                    if (Parameter.ParameterDirection == ParameterDirection.ReturnValue)
                    {
                        parameters.Add(new OracleParameter()
                        {
                            ParameterName = Parameter.Name,
                            OracleDbType = (OracleDbType)GetDbType(Parameter.Type),
                            Direction = ParameterDirection.ReturnValue,
                            Size = Parameter.Size
                        });
                    }
                }
                isSuccess = DB.ExecuteStoredProcedure(procedureName, bindbyName, parameters, dBParameter);
                returnResult.Message = DB.ErrorMessage;
            }
            catch (Exception exception)
            {
                returnResult.Message = exception.ToString();
                isSuccess = false;
            }
            returnResult.IsSuccess = isSuccess;
            return returnResult;
        }
        public IDbAction GetActionFactory()
        {
            return ActionFactory.GetDBAction(DbObject);
        }
        private void UpdateTranslate(IModel valueObject, List<OracleParameter> parameters)
        {
            foreach (PropertyInfo prop in valueObject.GetType().GetProperties())
            {
                if (prop.GetValue(valueObject, null) != null)
                {
                    if (CheckBrowsable(valueObject, prop.Name))
                    {
                        //SqlStringBuilder.Append(prop.Name.ToUpper() + " = :" + prop.Name.ToUpper() + ",");
                        SqlString.Append(prop.Name.ToUpper() + " = " + DbSymbolize + prop.Name.ToUpper() + ",");

                        var propertyType = prop.PropertyType;

                        if (prop.PropertyType.IsGenericType &&
                                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = prop.PropertyType.GetGenericArguments()[0];

                        //如果obj value非null但長度為0, 代表需為NULL, 以DBnull.Value傳值
                        parameters.Add(new OracleParameter(DbSymbolize + prop.Name.ToUpper(), (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(valueObject, null).ToString().Length > 0 ? prop.GetValue(valueObject, null) : DBNull.Value, ParameterDirection.Input));
                    }

                }
            }
            SqlString.Remove(SqlString.Length - 1, 1);
        }
        private string QueryStringGenerator(IModel objectModel, ICollection<OracleParameter> parameters)
        {
            SqlString = new StringBuilder();
            SqlString.Append("Select " + objectModel.GetType().Name + ".*, ROWID from " + objectModel.GetType().Name);

            if (objectModel != null)
            {
                SqlString.Append(" Where ");
                foreach (var prop in objectModel.GetType().GetProperties())
                {
                    var propertyType = prop.PropertyType;
                    var parameterName = prop.Name.ToUpper();

                    if (prop.PropertyType.IsGenericType &&
                                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        propertyType = prop.PropertyType.GetGenericArguments()[0];

                    if (parameterName == "ROWID")
                    {
                        parameterName = parameterName + "1";
                        SqlString.Append(" ROWID = :" + parameterName);
                    }
                    else
                    {
                        SqlString.Append(parameterName + " = :" + parameterName);
                    }
                    parameters.Add(
                            new OracleParameter(DbSymbolize + parameterName, (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(objectModel, null), ParameterDirection.Input)
                            );
                    SqlString.Append(" and ");
                }
                SqlString.Remove(SqlString.Length - 5, 5);
            }

            return SqlString.ToString();
        }
        private string ParseWhereCondition(IModel objectModel, ICollection<OracleParameter> parameters)
        {
            StringBuilder sqlConditionWhere = new StringBuilder();

            if (objectModel != null)
            {
                sqlConditionWhere.Append(" WHERE ");
                foreach (var prop in objectModel.GetType().GetProperties())
                {
                    if (Object.Equals(prop.GetValue(objectModel, null), null))
                        continue;

                    var propertyType = prop.PropertyType;
                    var parameterName = prop.Name.ToUpper();

                    if (prop.PropertyType.IsGenericType &&
                                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        propertyType = prop.PropertyType.GetGenericArguments()[0];

                    if (parameterName == "ROWID")
                    {
                        parameterName = parameterName + "1";
                        sqlConditionWhere.Append(" ROWID = " + DbSymbolize + parameterName);
                    }
                    else
                    {
                        sqlConditionWhere.Append(parameterName + " = " + DbSymbolize + parameterName);
                    }
                    parameters.Add(
                            new OracleParameter(DbSymbolize + parameterName, (OracleDbType)GetDbType(propertyType.Name), prop.GetValue(objectModel, null), ParameterDirection.Input)
                            );
                    sqlConditionWhere.Append(" AND ");
                }
                if (parameters.Count > 0)
                    sqlConditionWhere.Remove(sqlConditionWhere.Length - 5, 5);
                else
                    sqlConditionWhere.Remove(sqlConditionWhere.Length - 7, 7);
            }

            return sqlConditionWhere.ToString();
        }
        /// <summary>
        /// 檢查物件內的[Browsable]屬性是true 或 false
        /// true代表可以存在於欄位, 可讓程式自動帶入SQL中
        /// false代表搜尋用欄位, 可自動略過不帶入SQL中
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private bool CheckBrowsable(object model, string columnName)
        {
            AttributeCollection attributes =
                TypeDescriptor.GetProperties(model)[columnName].Attributes;
            BrowsableAttribute myAttribute =
                (BrowsableAttribute)attributes[typeof(BrowsableAttribute)];

            return myAttribute.Browsable;
        }
    }
}
