using System;
using ZooKeeperNet;

namespace AppClient
{
    /// <summary>
    /// Watcher-AppClient
    /// </summary>
    public class WatcherForInitZk : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("WatcherForInitZk State:{0}", @event.State);
            Console.WriteLine("WatcherForInitZk Path:{0}", @event.Path);
            var groupNode = Singleton.Instance.GroupNode();
            //当zookeeper宕机， 与zookeeper断开后，又再次重新连接的事件
            if (@event.State == KeeperState.SyncConnected && string.IsNullOrEmpty(@event.Path))
            {
                //TODO updateServerList()
                Console.WriteLine("2-2");
                AppClient.Instance.UpdateServerList();
                Console.WriteLine(DateTime.Now);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
            //当zookeeper--节点发生变化时
            if (@event.Type == EventType.NodeChildrenChanged && ("/" + groupNode).Equals(@event.Path))
            {
                //TODO updateServerList()
                Console.WriteLine("1-1");
                AppClient.Instance.UpdateServerList();
                Console.WriteLine(DateTime.Now);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (@event.State == KeeperState.Disconnected||@event.State==KeeperState.Expired)
            {
                //TODO 客户端与ZK服务器断开连接的情况
                //清空--服务的服务器列表
                Console.WriteLine("客户端与ZK服务器断开连接的情况-清空--服务的服务器列表");
            }           
        }
    }
}