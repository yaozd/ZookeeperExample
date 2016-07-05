using System;
using System.Threading;
using ZooKeeperNet;

namespace AppServer
{
    /// <summary>
    /// Watcher-AppServer
    /// </summary>
    public class WatcherForInitZk : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            if (@event.State == KeeperState.Disconnected || @event.State == KeeperState.Expired)
            {
                //TODO 客户端与ZK服务器断开连接的情况
                //清空--服务的服务器列表
                Console.WriteLine("AppServer-State-Disconnected-or-Expired");
                //不管的将本地服务写入到ZK服务器端，无限循环直到注册成功
                //2.重新注册服务
                var address = Singleton.Instance.Address();
                AppServer.Instance.Register(address+ @event.State);
            }
        }
    }
}