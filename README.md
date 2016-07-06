# ZookeeperExample
ZookeeperExample-服务的注册与发现的小例子：客户端-读取服务、服务器端-注册服务

* 特殊情况(开发时认真考虑的点)
  
    > 1.Zookeeper服务器宕机
    
    > 2.注册的服务器宕机
    
    > 3.出现重复的注册服务
    
    > 4.宕机后连接再重新建立

    > #5.Zookeeper默认单个IP的最大连接数为10，如果超过10个，则报异常：Too many connections from /127.0.0.1 -max is 10

* 异常情况
  
    > KeeperException.ConnectionLossException
    
    > KeeperException.SessionExpiredException
    
    > KeeperException.OperationTimeoutException
    
    > TimeoutException
    
    > Exception--未知异常

    > KeeperException.NoNodeException--节点不存在

    > KeeperException.NodeExistsException--节点已经存在

* 参考文档

    >zookeeper watch参照表谭志宇
    >http://www.cnblogs.com/chengxin1982/p/3997490.html
