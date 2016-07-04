using System;
using ZooKeeperNet;

namespace AppClient
{
    public class Singleton
    {
        private static readonly Lazy<Singleton> lazy =
          new Lazy<Singleton>(() => new Singleton());

        private ZooKeeper _zk;

        public static Singleton Instance
        {
            get { return lazy.Value; }
        }

        private Singleton()
        {
            InitZookeeperClients();
        }

        public ZooKeeper ZkClient()
        {
            if (_zk == null) SetZooKeeperClient();
            return _zk;
        }

        public string GroupNode()
        {
            return Sgroup;
        }

        private void InitZookeeperClients()
        {
            _zk = LoadClient();
        }
        private void SetZooKeeperClient()
        {
            _zk = LoadClient();
        }
        private ZooKeeper LoadClient()
        {
            return  new ZooKeeper(Connectstring, new TimeSpan(0, 0, 0, 50000), new WatcherForInitZk());
        }

        private string Connectstring
        {
            get { return "127.0.0.1:2181"; }
        }
        private static string Sgroup
        {
            get { return "sgroup"; }
        }
    }
}