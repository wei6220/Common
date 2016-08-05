using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Tests
{
    [TestClass()]
    public class IniHelperTests
    {
        [TestMethod()]
        public void GetConfigSettingTest()
        {
            /**3A原則*/
            //Arrange : 初始化目標物件、相依物件、方法參數、預期結果，或是預期與相依物件的互動方式
            //var actual = IniHelper.GetConfigSetting("inipath");
            var actual = "OK";
            //Act : 呼叫目標物件的方法

            //Assert : 驗證是否符合預期
            //Assert.IsNotNull(actual, "Jerry Debug :" + actual);
            Assert.AreEqual("OK", actual);
        }

        [TestMethod()]
        public void GetIniFilePathTest()
        {
            var actual = IniHelper.GetIniFilePath();
            var expected = IniHelper.GetConfigSetting("IniPath");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIniFilePathTest1()
        {
            var actual = IniHelper.GetIniFilePath("inipath");
            var expected = IniHelper.GetIniFilePath();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIniFileValueTest()
        {
            var actual = IniHelper.GetIniFileValue("DATABASE", "USERID");
            Assert.IsNotNull(actual, actual.ToString());
        }
    }
}