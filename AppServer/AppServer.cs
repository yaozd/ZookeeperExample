using System;
using System.Collections.Generic;
using System.Threading;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace AppServer
{
    /// <summary>
    /// 唯一性：
    /// 服务注册有特点--必须是在当前集群内-有且仅有一个节点
    /// 所有节点必须是唯一
    /// </summary>
    public class AppServer : IDisposable
    {
        private static readonly object LockObject = new object();
        private static readonly Lazy<AppServer> lazy =
            new Lazy<AppServer>(() => new AppServer());

        public static AppServer Instance
        {
            get { return lazy.Value; }
        }

        private ZooKeeper _zk = Singleton.Instance.ZkClient();
        /// <summary>
        /// 当前服务的注册节点
        /// </summary>
        private string  _currentSubNode ;

        public void Register(string address)
        {
            lock (LockObject)
            {
                var groupNode = Singleton.Instance.GroupNode();
                var subNode = Singleton.Instance.SubNode();
                //
                var statGroupNode = IsExistsGroupNode(groupNode);
                if (!statGroupNode) CreateGroupNode(groupNode);
                //
                var statSubNode = IsExistSubNode();             
                if (!statSubNode) CreateSubNode(address, groupNode, subNode);
                if (statSubNode)
                {
                    Display("节点{0}：已存在",address);
                }
            }

        }
        /// <summary>
        /// 判断groupNode节点是否存在
        /// </summary>
        /// <param name="groupNode"></param>
        /// <returns></returns>
        private bool IsExistsGroupNode(string groupNode)
        {
            var stat = IsExistsNode("/" + groupNode, false);
            if (stat == null) return false;
            return true;
        }
        /// <summary>
        /// 判断SubNode节点是否存在
        /// </summary>
        /// <returns></returns>
        private bool IsExistSubNode()
        {
            if (string.IsNullOrWhiteSpace(_currentSubNode)) return false;
            var stat = IsExistsNode(_currentSubNode, false);
            if (stat == null) return false;
            return true;
        }
        private Stat IsExistsNode(string path, bool watch)
        {
            while (true)
            {
                try
                {
                    return Execute(() => _zk.Exists(path, watch));
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
        private  void CreateSubNode(string address, string groupNode, string subNode)
        {
            Display("CreateSubNode:{0}", address);
            while (true)
            {
                try
                {
                    _currentSubNode = Execute(() =>_zk.Create("/" + groupNode + "/" + subNode, address.GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.EphemeralSequential) );
                    //注册完成后--退出
                    return;
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
                    Execute(() => _zk.Create("/" + groupNode, "XX集群父节点".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent));
                    return;
                }
                //节点已经存在             
                //KeeperException.NodeExistsException节点已经存在--这是一种特殊情况
                catch (KeeperException.NodeExistsException)
                {
                    return;
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
        private T Execute<T>(Func<T> action)
        {
            while (true)
            {
                try
                {
                    return action();
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
                catch (KeeperException.OperationTimeoutException)
                {
                    Dispose();
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (TimeoutException)
                {
                    Dispose();
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
            }           
        }
        private void Execute(Action action)
        {
            while (true)
            {
                try
                {
                    action();
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
                catch (KeeperException.OperationTimeoutException)
                {
                    Dispose();
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
                catch (TimeoutException)
                {
                    Dispose();
                    _zk = Singleton.Instance.ZkClient();
                    Thread.Sleep(1000);
                }
            }
        }
        private void Display(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
        public void Dispose()
        {
            Singleton.Instance.Dispose();
        }
    }
}