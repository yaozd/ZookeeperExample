using System;
using System.Collections.Generic;
using System.Threading;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace AppClient
{
    public class AppClient
    {
        //
        private readonly Stat _stat = new Stat();
        private volatile List<string> _serverList;
        //
        private static readonly Lazy<AppClient> lazy =
         new Lazy<AppClient>(() => new AppClient());
        public static AppClient Instance
        {
            get { return lazy.Value; }
        }
        private AppClient()
        {
            UpdateServerList();
        }

        public List<string> ServerList()
        {
            if(_serverList==null)UpdateServerList();
            return _serverList;
        }
        public void UpdateServerList()
        {
            var zk = Singleton.Instance.ZkClient();
            var groupNode = Singleton.Instance.GroupNode();
            List<string> newServerList = new List<string>();
            // 获取并监听groupNode的子节点变化
            // watch参数为true, 表示监听子节点变化事件. 
            // 每次都需要重新注册监听, 因为一次注册, 只能监听一次事件, 如果还想继续保持监听, 必须重新注册
            var subList = GetChildren(zk, groupNode);
            foreach (var subNode in subList)
            {
                // 获取每个子节点下关联的server地址
                byte[] data = GetSubNodeData(zk, groupNode, subNode);
                //跳过NoNodeException的异常情况
                if (data==null)continue;
                string str = System.Text.Encoding.Default.GetString(data);
                newServerList.Add(str);
            }
            // 替换server列表
            _serverList = newServerList;
            foreach (var server in _serverList)
            {
                Display("server list updated: " + server);
            }
        }

        private static IEnumerable<string> GetChildren(ZooKeeper zk, string groupNode)
        {
            while (true)
            {
                try
                {
                    return zk.GetChildren("/" + groupNode, true);
                }
                    //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        private byte[] GetSubNodeData(ZooKeeper zk, string groupNode, string subNode)
        {
            while (true)
            {
                try
                {
                    return zk.GetData("/" + groupNode + "/" + subNode, false, _stat);
                }
                //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                //跳过NoNodeException的异常情况-相当子节点不存在的情况
                catch (KeeperException.NoNodeException)
                {
                    return null;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private void Display(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
         
    }
}