using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ancestor.DataAccess.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancestor.Core;
using Ancestor.DataAccess.Factory;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using AncestorUnitTestProject.Models;
using System.Data.SqlServerCe;
//using LIS_DLL.Model;

namespace Ancestor.DataAccess.DAO.Tests
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/08/27
    /// Subject : Oracle Query.IModel.Test
    /// 
    /// History : 
    /// 2015/08/27 Andycow0 新增 TestCategory 的分類標籤
    /// 2015/09/01 Andycow0 create TDD methods for this class, and testing by .csv type file.
    /// 2015/09/02 Andycow0 move models to Models folder.
    /// </summary>
    [TestClass()]
    public class OracleIModelTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod()]
        [TestCategory("Oracle.IModel.Query")]
        public void QueryTest()
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

            RPRSPDB rprspdb = new RPRSPDB() { CHTNO = "1234" };

            var queryResult = daoobject.Query(rprspdb);

            bool result = false;

            if (queryResult.ReturnDataTable.Rows.Count > 0)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Oracle.IModel.Query")]
        public void QueryTest2()
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

            RPRSPDB rprspdb = new RPRSPDB() { CHTNO = "1234", SEX = "M" };

            var queryResult = daoobject.Query(rprspdb);

            bool result = false;

            if (queryResult.ReturnDataTable.Rows.Count > 0)
                result = true;

            Assert.AreEqual(false, result);

        }
        [TestMethod()]
        [TestCategory("Oracle.IModel.Query")]
        public void QueryTest3()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",
                Password = "test",
                Node = "JIATEST4"
            };

            DAOFactory target = new DAOFactory(dbObj);
            var daoobject = target.GetDataAccessObjectFactory();

            RPRSPDB rprspdb = new RPRSPDB() { CHTNO = "1234", SEX = "M" };

            var queryResult = daoobject.Query(rprspdb);

            bool result = false;

            if (queryResult.ReturnDataTable.Rows.Count > 0)
                result = true;

            Assert.AreEqual(false, result);

        }

        [TestMethod()]
        [TestCategory("Oracle.IModel.Insert")]
        public void ROPRPAINVISIT_Insert()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cghopr",
                Password = "oprappl",
                Node = "lnka2"
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoobject = factory.GetDataAccessObjectFactory();

            //DateTime d = new DateTime(2015, 8, 31, 9, 30, 0);

            ROPRPAINVISIT roprpainvisit = new ROPRPAINVISIT()
            {
                CHTNO = "123",
                CSN = "11111",
                OPERNO = "1234567890",
                VSDAT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                EB1 = 11,
                EB2 = 1
            };

            var target = daoobject.Insert(roprpainvisit);

            bool result = false;

            if (target.IsSuccess)
                result = true;

            Assert.AreEqual(true, result);

        }

        [TestMethod()]
        [TestCategory("Oracle.IModel.Update")]
        public void Andycow0_Update()
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
            var daoobject = factory.GetDataAccessObjectFactory();

            var andycow0 = new ANDYCOW0()
            {
                FIRST = "22",
                SECOND = 2
            };

            var w_andycow0 = new ANDYCOW0()
            {
                FIRST = "11",
                SECOND = 1
            };

            var target = daoobject.Update(andycow0, w_andycow0);

            bool result = false;

            if (target.IsSuccess == true && target.EffectRows == 1)
                result = true;

            Assert.AreEqual(true, result);

        }

        [TestMethod()]
        [TestCategory("Oracle.IModel.Delete")]
        public void ROPRPAINVISIT_Delete()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cghopr",
                Password = "oprappl",
                Node = "lnka2"
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoobject = factory.GetDataAccessObjectFactory();

            //ROPRPAINVISIT roprpainvisit = new ROPRPAINVISIT()
            //{
            //    //CHTNO = "123",
            //    //CSN = "11111",
            //    //OPERNO = "1234567890",
            //    //VSDAT = DateTime.Now.ToString("yyyyMMddHHmmss"),
            //    EB1 = 15,
            //    EB2 = 3,
            //    EB1UINIT = "mg"
            //};

            ROPRPAINVISIT w_roprpainvisit = new ROPRPAINVISIT()
            {
                CHTNO = "123",
                CSN = "11111",
                OPERNO = "1234567890",
                //VSDAT = "20150901102411"
            };

            var target = daoobject.Delete(w_roprpainvisit);

            bool result = false;

            if (target.IsSuccess == true && target.EffectRows > 0)
                result = true;

            Assert.AreEqual(true, result);

        }

        [TestCategory("Oracle.IModel.TDD")]
        //[DeploymentItem("AncestorTest.csv")]
        //[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
        //            "|DataDirectory|\\AncestorTest.csv",
        //            "ROPRPAINVISIT#csv", DataAccessMethod.Random)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\data.csv", "data#csv", DataAccessMethod.Sequential), DeploymentItem("data.csv"), TestMethod]
        //[TestMethod()]
        public void Andycow0Insert()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",//"cghopr",
                Password = "test",//"oprappl",
                //Node = "lnka2"
                Node = "jiatest4",
                IP = "10.31.11.118"//
            };

            var first = testContextInstance.DataRow[0].ToString();
            var second = Int32.Parse(testContextInstance.DataRow[1].ToString());
            var third = DateTime.ParseExact(testContextInstance.DataRow[2].ToString(), "yyyyMMddHHmmss", null);
            //var fourth = testContextInstance.DataRow[3].ToString();

            ANDYCOW0 andycow0 = new ANDYCOW0()
            {
                FIRST = first,
                SECOND = second,
                THIRD = third
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var target = daoObject.Insert(andycow0);

            bool result = false;

            if (target.IsSuccess)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestCategory("Oracle.IModel.TDD")]
        //[DeploymentItem("AncestorTest.csv")]
        //[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
        //            "|DataDirectory|\\AncestorTest.csv",
        //            "ROPRPAINVISIT#csv", DataAccessMethod.Random)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\Resources\\data.csv", "data#csv", DataAccessMethod.Sequential), DeploymentItem("data.csv"), TestMethod]
        //[TestMethod()]
        public void Andycow0Delete()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                ID = "cgh",//"cghopr",
                Password = "test",//"oprappl",
                //Node = "lnka2"
                Node = "jiatest4",
                IP = "10.31.11.118"//
            };

            var first = testContextInstance.DataRow[0].ToString();
            var second = Int32.Parse(testContextInstance.DataRow[1].ToString());
            var third = DateTime.ParseExact(testContextInstance.DataRow[2].ToString(), "yyyyMMddHHmmss", null);
            //var fourth = testContextInstance.DataRow[3].ToString();

            ANDYCOW0 andycow0 = new ANDYCOW0()
            {
                FIRST = first,
                SECOND = second,
                THIRD = third
            };

            DAOFactory factory = new DAOFactory(dbObj);
            var daoObject = factory.GetDataAccessObjectFactory();

            var target = daoObject.Delete(andycow0);

            bool result = false;

            if (target.IsSuccess)
                result = true;

            Assert.AreEqual(true, result);
        }

        [TestCategory("Oracle.IModel.Query")]
        [DataSource(@"Provider=Microsoft.SqlServerCe.Client.4.0;Data Source=D:\CSharp\Base\Ancestor\AncestorUnitTestProject\ROPRPAINVISIT.sdf;Password=0000", "ROPRPAINVISIT")]
        [TestMethod()]
        public void Query2()
        {
            string chtno = testContextInstance.DataRow["CHTNO"].ToString();
            Assert.Fail();
        }
    }
}
