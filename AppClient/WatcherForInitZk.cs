using System;
using ZooKeeperNet;

namespace AppClient
{
    public class WatcherForInitZk : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("WatcherForInitZk State:{0}", @event.State);
            Console.WriteLine("WatcherForInitZk Path:{0}", @event.Path);
            var groupNode = Singleton.Instance.GroupNode();
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