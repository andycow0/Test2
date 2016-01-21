using Ancestor.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ancestor.DataAccess.DBAction;

namespace Ancestor.DataAccess.DAO
{
    public interface IDataAccessObject
    {
        IDbAction GetActionFactory();
        AncestorResult Query<T>(IModel objectModel) where T : class, IModel, new();
        AncestorResult Query<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        AncestorResult Query(IModel objectModel);
        AncestorResult Query(string sqlString, object paramsObjects);
        AncestorResult QueryNoRowid<T>(IModel objectModel) where T : class, IModel, new();
        AncestorResult QueryNoRowid<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        AncestorResult QueryNoRowid(IModel objectModel);
        AncestorResult Insert(IModel objectModel);
        AncestorResult Update(IModel valueObject, IModel whereObject);
        AncestorResult Update<T>(IModel valueObject, Expression<Func<T, bool>> predicate) where T : class, new();
        AncestorResult Delete(IModel whereObject);
        AncestorResult Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        AncestorResult ExecuteNonQuery(string sqlString, object modelObject);
        AncestorResult ExecuteStoredProcedure(string procedureName, bool bindbyName, List<DBParameter> dBParameter);
    }
}
