# ZookeeperExample
ZookeeperExample-服务的注册与发现的小例子：客户端-读取服务、服务器端-注册服务

* 特殊情况(开发时认真考虑的点)
  
    > 1.Zookeeper服务器宕机
    
    > 2.注册的服务器宕机
    
    > 3.出现重复的注册服务
    
    > 4.宕机后连接再重新建立
    

* 异常情况
  
    > KeeperException.ConnectionLossException
    
    > KeeperException.SessionExpiredException
    
    > KeeperException.OperationTimeoutException
    
    > TimeoutException
    
    > Exception--未知异常
