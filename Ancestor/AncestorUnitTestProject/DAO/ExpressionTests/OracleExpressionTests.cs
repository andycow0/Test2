using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ancestor.DataAccess.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancestor.Core;
using Ancestor.DataAccess.Factory;
using AncestorUnitTestProject.Models;

namespace Ancestor.DataAccess.DAO.Tests
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/08/27
    /// Subject : Oracle Query Expression
    /// 
    /// History : 
    /// 2015/08/27 Andycow0 建立, 新增 TestCategory 的分類標籤
    /// 2015/08/31 Andycow0 由 OracleIModelTests 的內容，移至 QueryExpressionTest 
    /// </summary>
    [TestClass()]
    public class OracleExpressionTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Query")]
        public void ExpressionQueryTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHLABM",
                Password = "LABMAPPL",
                Node = "LNKA1"
            };
            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            var CHTNO = "123";
            var pdb = new RPRSPDB() {CHTNO = "123"};

            //var result = daoobject.Query<RPRSPDB>((x) => x.CHTNO == CHTNO);
            //var result = daoobject.Query<RPRSPDB>((x) => x.CHTNO.StartsWith("; insert into rlabsyslog(exeemp,exedat,exetm,jobty) values('Test','20150820','0524','TEST');"));
            //var result = daoobject.Query<RPRSPDB>((x) => x.IDNO == "123" && x.ONWMK == null);
            //var result = daoobject.Query<RPRSPDB>((x) => x.CHTNO.EndsWith( pdb.CHTNO));
            var result = daoobject.Query<RPRSPDB>(x=>x.CHTNO=="123").DataList as List<RPRSPDB>;
            var rprspdbChtnoList = (from a in result select a.CHTNO).ToList<string>();
            var lisResult = daoobject.Query<RPRSPDB>(x => rprspdbChtnoList.Contains(x.CHTNO));
            
            var startRCVDAT = "20140101";
            var endRCVDAT = "20151031";
            //var result =
            //    daoobject.Query<RLABINDX>(
            //        x =>
            //            x.CHTNO == CHTNO &&
            //            (x.RCVDAT.CompareTo(startRCVDAT) >= 0 && x.RCVDAT.CompareTo(endRCVDAT) <= 0));
            //var t = (List<RPRSPDB>)result.DataList;

            //foreach (RPRSPDB item in result.DataList)
            //{

            //}

            //Assert.Fail();
        }

        /// <summary>
        /// 以變數當作條件傳入Lambda做測試
        /// </summary>
        [TestMethod()]
        [TestCategory("Oracle.Expression.Query.VariableTest")]
        public void ExpressionQueryVariableTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHLABM",
                Password = "LABMAPPL",
                Node = "LNKA1"
            };
            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            var CHTNO = "123";

            var result = daoobject.Query<RPRSPDB>((x) => x.CHTNO == CHTNO);
            Assert.Fail();
        }

        /// <summary>
        /// 以物件的屬性當作條件傳入Lambda做測試
        /// </summary>
        [TestMethod()]
        [TestCategory("Oracle.Expression.Query.PropertiesTest")]
        public void ExpressionQueryPropertiesTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHLABM",
                Password = "LABMAPPL",
                Node = "LNKA1"
            };
            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            var CHTNO = "123";
            var pdb = new RPRSPDB() { CHTNO = "123" };

            var result = daoobject.Query<RPRSPDB>((x) => x.CHTNO.EndsWith( pdb.CHTNO));
            Assert.Fail();
        }

        /// <summary>
        /// 以List<string>當作In的條件  測試List<string>.Contains
        /// </summary>
        [TestMethod()]
        [TestCategory("Oracle.Expression.Query.InCauseTest")]
        public void ExpressionQueryInCauseTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHLABM",
                Password = "LABMAPPL",
                Node = "LNKA1"
            };
            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            var CHTNO = "123";
            var pdb = new RPRSPDB() { CHTNO = "123" };

            var result = daoobject.Query<RPRSPDB>(x => x.CHTNO == "123" || x.CHTNO == "500" || x.CHTNO == "88888888").DataList as List<RPRSPDB>;
            var rprspdbChtnoList = (from a in result select a.CHTNO).ToList<string>();
            var lisResult = daoobject.Query<RLABINDX>(x => rprspdbChtnoList.Contains(x.CHTNO));

            Assert.Fail();
        }

        /// <summary>
        /// 測試select ALL
        /// </summary>
        [TestMethod()]
        [TestCategory("Oracle.Expression.QueryAll")]
        public void ExpressionQueryAll()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "CGHOPR",
                Password = "OPRAPPL",
                Node = "LNKA2"
            };
            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            var result = daoobject.Query<ROPRPAINVISIT>(new ROPRPAINVISIT()).DataList as List<ROPRPAINVISIT>;
            
            Assert.Fail();
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Query")]
        public void ExpressionQueryTest2()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };
            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var target = daoObject.Query<RPRSPDB>((x) => x.CHTNO == "500");

            bool result = false;

            if (target.IsSuccess && target.DataList.Count == 1)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Update")]
        public void ANDYCOW0_Update()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };
            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var andycow0 = new ANDYCOW0()
            {
                //FIRST = "66",
                SECOND = 7,
                THIRD = DateTime.Now//.AddDays(-1)
            };

            var target = daoObject.Update<ANDYCOW0>(andycow0, (x) => x.FIRST.Contains("6"));

            bool result = false;

            if (target.IsSuccess && target.EffectRows > 0)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestCategory("Oracle.Expression.TDD.Update")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\data.csv", "data#csv", DataAccessMethod.Sequential), DeploymentItem("data.csv"), TestMethod]
        public void ANDYCOW0_TDD_Update()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };

            var first = testContextInstance.DataRow[0].ToString();
            var second = Int32.Parse(testContextInstance.DataRow[1].ToString());
            var third = DateTime.ParseExact(testContextInstance.DataRow[2].ToString(), "yyyyMMddHHmmss", null);

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var andycow0 = new ANDYCOW0()
            {
                //FIRST = "11",
                SECOND = 300,
                THIRD = DateTime.Now.AddDays(-1)
            };

            var target = daoObject.Update<ANDYCOW0>(andycow0, (x) => x.FIRST == "22" && x.SECOND > 300);

            bool result = false;

            if (target.IsSuccess && target.EffectRows > 0)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Delete")]
        public void ANDYCOW0_Delete_Contains()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var first = "500";

            var target = daoObject.Delete<ANDYCOW0>((x) => x.FIRST.StartsWith(first));

            bool result = false;

            if (target.IsSuccess && target.EffectRows > 0)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Delete")]
        public void ANDYCOW0_Delete()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var target = daoObject.Delete<ANDYCOW0>((x) => x.SECOND > 200);

            bool result = false;

            if (target.IsSuccess && target.EffectRows > 0)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Oracle.Expression.Query")]
        public void WizTonE_VHPNPULB_Test()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "jiatest4",
                IP = "10.31.11.118"
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var target = daoObject.QueryNoRowid<VHPNPULB>(x=>x.IDNO == "A125120059");

            bool result = false;

            if (target.IsSuccess && target.DataList.Count > 0)
                result = true;

            Assert.AreEqual(true, result);
        }
    }

}
