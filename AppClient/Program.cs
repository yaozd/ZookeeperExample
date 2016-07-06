using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient
{
    class Program
    {
        /// <summary>
        /// zookeeper.discovery.register
        /// //TODO 客户端-读取服务
        /// 服务器端-注册服务
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Zookeeper AppClient Test Go-{0:yyyy-M-dd:hh-mm-ss}", DateTime.Now);
            var serverList= AppClient.Instance.ServerList();
            foreach (var subNode in serverList)
            {
                Console.WriteLine("Main:" + subNode);
            }
            Console.ReadLine();
        }
    }
}
