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
        /// <summary>
        /// zookeeper.discovery.register
        /// 客户端-读取服务
        /// //TODO 服务器端-注册服务
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                var address = "xxx=192.168.0.2";
                address = Singleton.Instance.Address();
                AppServer.Instance.Register(address);
                Thread.Sleep(10*1000);
                //AppServer.Instance.Dispose();
            }

            Console.ReadLine();
        }
    }
}
