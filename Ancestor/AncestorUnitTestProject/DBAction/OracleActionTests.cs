using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ancestor.DataAccess.DBAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancestor.Core;
using Ancestor.DataAccess.Factory;
using System.Data;
using Oracle.DataAccess.Client;
namespace Ancestor.DataAccess.DBAction.Tests
{
    [TestClass()]
    public class OracleActionTests
    {
        [TestMethod()]
        [TestCategory("Oracle.ExecuteStoredProcedure")]
        public void ExecuteStoredProcedureTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHOPD",
                Password = "OPDAPPL",
                Node = "LNKA1"
            };

            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            DBParameter dbP = new DBParameter()
            {
                Name = "IDNO",
                Size = 100,
                Value = "K122013118",
                Type = "string",
                ParameterDirection = ParameterDirection.Input
            };

            List<DBParameter> dbs = new List<DBParameter>();
            dbs.Add(dbP);

            var acs = daoobject.ExecuteStoredProcedure("KOPD200.OPD_VVIP", true, dbs);



            Assert.Fail();
        }
    }
}
