using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Tests.TestEntities;

namespace Senparc.CO2NET.Tests.Helpers
{
    [TestClass]
    public class MutipleCacheTestHelperTests
    {
        [TestMethod]
        public void MutipleCacheTestHelperTest()
        {
            BaseTest.RegisterServiceStart();//�Զ�ע��Redis��Ҳ�����ֶ�ע��
            BaseTest.RegisterServiceCollection();

            var exCache = TestExtensionCacheStrategy.Instance;//������򻺴�ע��
            var exRedisCache = TestExtensionRedisCacheStrategy.Instance;//���Redis���򻺴�ע��

            MutipleCacheTestHelper.RunMutipleCache(() =>
            {
                try
                {
                    var currentCache = CacheStrategyFactory.GetObjectCacheStrategyInstance();
                    Console.WriteLine("��ǰ������ԣ�" + currentCache.GetType());

                    var testExCache = CacheStrategyFactory.GetExtensionCacheStrategyInstance(new TestCacheDomain());
                    var baseCache = testExCache.BaseCacheStrategy();

                    Console.WriteLine("��ǰ��չ������ԣ�"+ baseCache.GetType());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);//Local�Ѿ�ע�ᣬRedisδע��
                }

            }, CacheType.Local, CacheType.Redis);
        }
    }
}
