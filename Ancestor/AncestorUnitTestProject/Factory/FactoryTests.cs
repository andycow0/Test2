using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ancestor.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ancestor.Core;

namespace Ancestor.DataAccess.Tests
{
    /// <summary>
    /// Author  : Andycow0 
    /// Date    : 2015/08/27
    /// Subject : Connection Factory
    /// 
    /// History : 
    /// 2015/08/27 Andycow0 建立, 新增 TestCategory 的分類標籤
    /// 
    /// </summary>
    [TestClass()]
    public class FactoryTests
    {
        [TestMethod()]
        [TestCategory("Connection Factory")]
        public void GetConnectionFactoryTest()
        {
            DBObject dbObj = new DBObject()
            {
                DataBaseType = DBObject.DataBase.Oracle,
                Schema = "Test"
            };

            IConnection connResource = new ConnectionFactory(dbObj).GetConnectionFactory();

            bool result = true;
            Assert.AreEqual(false, result);
        }
    }
}
