using System;
using System.Threading;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace AppServer
{
    public class AppServer : IDisposable
    {
        private static readonly Lazy<AppServer> lazy =
            new Lazy<AppServer>(() => new AppServer());

        public static AppServer Instance
        {
            get { return lazy.Value; }
        }

        public void Register(string address)
        {
            var groupNode = Singleton.Instance.GroupNode();
            var subNode = Singleton.Instance.SubNode();
            var zk = Singleton.Instance.ZkClient();
            var stat = IsExistsGroupNode(zk, groupNode);
            if (stat == null) CreateGroupNode(zk, groupNode);
            CreateSubNode(address, zk, groupNode, subNode);
        }

        private  void CreateSubNode(string address, ZooKeeper zk, string groupNode, string subNode)
        {
            while (true)
            {
                try
                {
                    zk.Create("/" + groupNode + "/" + subNode, address.GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.EphemeralSequential);
                    //注册完成后--退出
                    return;
                }
                //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                catch (KeeperException.SessionExpiredException)
                {
                    Dispose();
                    zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        private  Stat IsExistsGroupNode(ZooKeeper zk, string groupNode)
        {
            while (true)
            {
                try
                {
                    return zk.Exists("/" + groupNode,true);
                }
                //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                catch (KeeperException.SessionExpiredException)
                {                  
                    Dispose();
                    zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private  void CreateGroupNode(ZooKeeper zk, string groupNode)
        {
            while (true)
            {
                try
                {
                    zk.Create("/" + groupNode, "XX集群父节点".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    return;
                }
                //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                catch (KeeperException.SessionExpiredException)
                {
                    Dispose();
                    zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                //KeeperException.NodeExistsException节点存在--这是一种特殊情况
                catch (KeeperException.NodeExistsException)
                {
                    return;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public void Dispose()
        {
            Singleton.Instance.Dispose();
        }
    }
}