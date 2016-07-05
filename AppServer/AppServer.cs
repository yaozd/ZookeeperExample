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

        private ZooKeeper _zk = Singleton.Instance.ZkClient();
        public void Register(string address)
        {
            var groupNode = Singleton.Instance.GroupNode();
            var subNode = Singleton.Instance.SubNode();
            var zk = Singleton.Instance.ZkClient();
            var stat = IsExistsGroupNode( groupNode);
            if (stat == null) CreateGroupNode(groupNode);
            CreateSubNode(address,groupNode, subNode);
        }

        private  void CreateSubNode(string address, string groupNode, string subNode)
        {
            while (true)
            {
                try
                {
                    _zk.Create("/" + groupNode + "/" + subNode, address.GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.EphemeralSequential);
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
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    //TODO 记录错误日志，但不抛出异常
                    //System.TimeoutException
                    //The request     / sgroup / sub    xsss - aaaExpired            world anyone     timed out while waiting for a response from the server.
                    throw;
                }
            }

        }

        private  Stat IsExistsGroupNode( string groupNode)
        {
            while (true)
            {
                try
                {
                    return _zk.Exists("/" + groupNode,true);
                }
                //ConnectionLossException-Sleep 1秒
                catch (KeeperException.ConnectionLossException)
                {
                    Thread.Sleep(1000);
                }
                catch (KeeperException.SessionExpiredException)
                {                  
                    Dispose();
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    //TODO 记录错误日志，但不抛出异常
                    //System.TimeoutException
                    //The request     / sgroup / sub    xsss - aaaExpired            world anyone     timed out while waiting for a response from the server.
                    throw;
                }
            }
        }

        private  void CreateGroupNode( string groupNode)
        {
            while (true)
            {
                try
                {
                    _zk.Create("/" + groupNode, "XX集群父节点".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
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
                    _zk = Singleton.Instance.ZkClient();
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