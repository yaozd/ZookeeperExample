using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppServer
{
    class Program
    {
        //原子计数
        public static int Count;
        /// <summary>
        /// zookeeper.discovery.register
        /// 客户端-读取服务
        /// //TODO 服务器端-注册服务
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Zookeeper AppServer Test Go");
            var address = "xxx=192.168.0.2";
            address = Singleton.Instance.Address();
            AppServer.Instance.Register(address,true);
            //AppServer.Instance.Dispose();
            //while (true)
            //{
            //    var address = "xxx=192.168.0.2";
            //    address = Singleton.Instance.Address();
            //    AppServer.Instance.Register(address);
            //    var current = Interlocked.Increment(ref Count);
            //    Console.WriteLine("{0}-Register-{1}", current, DateTime.Now);
            //    Thread.Sleep(5*1000);
            //    AppServer.Instance.Dispose();
            //    Thread.Sleep(1000);
            //    Console.WriteLine("{0}-Dispose-{1}", current, DateTime.Now);

            //}

            Console.ReadLine();
        }
    }
}
