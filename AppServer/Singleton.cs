using System;
using System.Threading;
using ZooKeeperNet;

namespace AppServer
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
            return _sgroup;
        }

        public string SubNode()
        {
            return _subnode;
        }
        public string Address()
        {
            return _address;
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
            //sessionTimeout--服务器端设置为5秒，这样当服务器宕机时，客户端能更快时间的发现
            return new ZooKeeper(_connectstring, new TimeSpan(0, 0, 0, 5), new WatcherForInitZk());
        }

        private string _connectstring
        {
            get { return "127.0.0.1:2181"; }
        }
        private  string _sgroup
        {
            get { return "sgroup"; }
        }
        private  string _subnode
        {
            get { return "sub"; }
        }
        private string _address
        {
            get { return "address-"+DateTime.Now.ToString("YYYY-MM-DD-hh-mm-ss"); }
        }
        public void Dispose()
        {
            if (_zk != null)
            {
                _zk.Dispose();
                _zk = null;
            }
        }
    }
}