using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Exceptions;
using Senparc.CO2NET.Tests.TestEntities;
using System;
using System.Collections.Generic;

namespace Senparc.CO2NET.Tests.Cache.CacheStrategyDomain
{


    [TestClass]
    public class CacheStrategyDomainWarehouseTests : BaseTest
    {
        [TestMethod]
        public void RegisterAndGetTest()
        {
            //��ԭĬ�ϻ���״̬
            CacheStrategyFactory.RegisterObjectCacheStrategy(() => LocalObjectCacheStrategy.Instance);

            //ע��
            CacheStrategyDomainWarehouse.RegisterCacheStrategyDomain(TestExtensionCacheStrategy.Instance);

            //��ȡ

            //��ȡ��ǰ������ԣ�Ĭ��Ϊ�ڴ滺�棩
            var objectCache = CacheStrategyFactory.GetObjectCacheStrategyInstance();
            var testCacheStrategy = CacheStrategyDomainWarehouse
                .GetDomainExtensionCacheStrategy(objectCache, new TestCacheDomain());

            Assert.IsInstanceOfType(testCacheStrategy, typeof(TestExtensionCacheStrategy));

            var baseCache = testCacheStrategy.BaseCacheStrategy();
            Assert.IsInstanceOfType(baseCache, objectCache.GetType());


            //д��
            var testStr = Guid.NewGuid().ToString();
            baseCache.Set("TestCache", testStr);

            //��ȡ
            var result = (testCacheStrategy as TestExtensionCacheStrategy).GetTestCache("TestCache");
            Assert.AreEqual(testStr + "|ABC", result);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void ClearRegisteredDomainExtensionCacheStrategiesTest()
        {
            //������򻺴�
            CacheStrategyDomainWarehouse.RegisterCacheStrategyDomain(TestExtensionCacheStrategy.Instance);
            var objectCache = CacheStrategyFactory.GetObjectCacheStrategyInstance();

            var testCacheStrategy = CacheStrategyDomainWarehouse
             .GetDomainExtensionCacheStrategy(objectCache, new TestCacheDomain());

            Assert.IsInstanceOfType(testCacheStrategy, typeof(TestExtensionCacheStrategy));

            //������򻺴�
            CacheStrategyDomainWarehouse.ClearRegisteredDomainExtensionCacheStrategies();
            try
            {
                testCacheStrategy = CacheStrategyDomainWarehouse
                                .GetDomainExtensionCacheStrategy(objectCache, new TestCacheDomain());
            }
            catch (UnregisteredDomainCacheStrategyException ex)
            {
                Console.WriteLine("�����쳣�׳�������ȷ��\r\n========\r\n");
                Console.WriteLine(ex);//δע��
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void AutoScanDomainCacheStrategyTest()
        {
            Config.IsDebug = true;
            {
                Console.WriteLine("ȫ���Զ�ɨ��");
                var addedTypes = CacheStrategyDomainWarehouse.AutoScanDomainCacheStrategy(true, null);
                addedTypes.ForEach(z => Console.WriteLine(z));
                Assert.IsTrue(addedTypes.Count > 0);
                Assert.IsTrue(addedTypes.Contains(typeof(TestExtensionCacheStrategy)));
                //�Զ�ɨ����򼯣�81����ע������ʱ��205.7718ms - 598.7549ms
            }
            {
                Console.WriteLine("���Զ�ɨ��");//
                var addedTypes = CacheStrategyDomainWarehouse.AutoScanDomainCacheStrategy(false, null);
                addedTypes.ForEach(z => Console.WriteLine(z));
                Assert.IsTrue(addedTypes.Count == 0);
                //ע������ʱ��0.0021ms
            }

            {
                Console.WriteLine("�ֶ�ָ��");
                Func<IList<IDomainExtensionCacheStrategy>> func = () =>
                {
                    var list = new List<IDomainExtensionCacheStrategy>();
                    list.Add(TestExtensionCacheStrategy.Instance);
                    return list;
                };

                var addedTypes = CacheStrategyDomainWarehouse.AutoScanDomainCacheStrategy(false, func);
                addedTypes.ForEach(z => Console.WriteLine(z));
                Assert.IsTrue(addedTypes.Count > 0);
                Assert.IsTrue(addedTypes.Contains(typeof(TestExtensionCacheStrategy)));
                //ע������ʱ��0.574ms
            }
        }
    }
}
