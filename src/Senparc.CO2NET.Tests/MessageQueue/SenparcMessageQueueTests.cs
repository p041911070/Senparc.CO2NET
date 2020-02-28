using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.MessageQueue;
using Senparc.CO2NET.Threads;

namespace Senparc.CO2NET.Tests.MessageQueue
{
    [TestClass]
    public class SenparcMessageQueueTests
    {
        [TestMethod]
        public void SenparcMessageQueueTest()
        {


            var smq = new SenparcMessageQueue();
            var keyPrefix = "TestMQ_";
            var count = smq.GetCount();

            for (int i = 0; i < 3; i++)
            {
                var key = keyPrefix + i;
                //����Add
                smq.Add(key, () =>
                  {
                      Console.WriteLine("ִ�ж��У�" + SystemTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                  });


                Console.WriteLine("��Ӷ����" + key);
                Console.WriteLine("��ǰ������" + smq.GetCount());
                Console.WriteLine("CurrentKey��" + smq.GetCurrentKey());
                Assert.AreEqual(count + 1, smq.GetCount());
                count = smq.GetCount();

                //����GetItem
                var item = smq.GetItem(key);
                Console.WriteLine("item.AddTime��" + item.AddTime);
                Assert.AreEqual(key, item.Key);

            }

            //����Remove
            smq.Add("ToRemove", () =>
            {
                Console.WriteLine("���������һ����˵��û������ɹ�");
            });
            smq.Remove("ToRemove",out SenparcMessageQueueItem value);

            //�����߳�
            ThreadUtility.Register();

            while (smq.GetCount() > 0)
            {
                //�ȴ����д�����
            }

            Console.WriteLine("���д�����ϣ���ǰ����������" + smq.GetCount());
        }

        [TestMethod]
        public void TestAll()
        {
            var mq = new SenparcMessageQueue();
            var count = mq.GetCount();
            var key = SystemTime.Now.Ticks.ToString();

            //Test Add()
            var item = mq.Add(key, () => Console.WriteLine("����SenparcMessageQueueд��Key=A"));
            Assert.AreEqual(count + 1, mq.GetCount());
            //var hashCode = item.GetHashCode();

            //Test GetCurrentKey()
            var currentKey = mq.GetCurrentKey();
            Assert.AreEqual(key, currentKey);

            //Test GetItem
            var currentItem = mq.GetItem(currentKey);
            Assert.AreEqual(currentItem.Key, item.Key);
            Assert.AreEqual(currentItem.AddTime, item.AddTime);

            //Test Remove
            mq.Remove(key,out SenparcMessageQueueItem value);
            Assert.AreEqual(count, mq.GetCount());
        }
    }
}
