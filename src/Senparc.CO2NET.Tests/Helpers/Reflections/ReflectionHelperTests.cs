using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Tests.TestEntities;

namespace Senparc.CO2NET.Tests.Helpers
{
    [TestClass]
    public class ReflectionHelperTests
    {
        public ReflectionHelperTests() {
            //ע�᱾�ػ���ע���
            BaseTest.RegisterServiceCollection();
            BaseTest.RegisterServiceStart();
        }

        [TestMethod]
        public void CreateInstanceTest()
        {
            var obj = ReflectionHelper.CreateInstance<TestCustomObject>("Senparc.CO2NET.Tests.TestEntities.TestCustomObject", "Senparc.CO2NET.Tests");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(TestCustomObject));
            Assert.AreEqual(DateTime.Today, (obj as TestCustomObject).AddTime.Date);
        }

        [TestMethod]
        public void CreateInstanceTest2()
        {
            var obj = ReflectionHelper.CreateInstance<TestCustomObject>("Senparc.CO2NET.Tests", "Senparc.CO2NET.Tests.TestEntities", "TestCustomObject");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(TestCustomObject));
            Assert.AreEqual(DateTime.Today, (obj as TestCustomObject).AddTime.Date);
        }


        [TestMethod]
        public void CreateStaticMemberTest()
        {
            //��Ҫע�᱾�ػ���ע��
            var obj = ReflectionHelper.GetStaticMember("Senparc.CO2NET", "Senparc.CO2NET.Cache", "LocalObjectCacheStrategy", "Instance");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(LocalObjectCacheStrategy));
        }


        [TestMethod]
        public void CreateStaticMemberByTypeTest()
        {
            //��Ҫע�᱾�ػ���ע��

            var type = typeof(LocalObjectCacheStrategy);
            var obj = ReflectionHelper.GetStaticMember(type, "Instance");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(LocalObjectCacheStrategy));
        }
    }
}
