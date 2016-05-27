
## Config instructions 

Wait for the blockchain to download compleatly.  
- make sure txindex=1 is set.
- disable wallet if feature is supported
- its recommended to use ssl on the client

Start mongodb server.  

Start Nako.  
Nako will then sync using the rpc endpoints the progress will be displayed on the console app  
Progress can also be displayed on the api endpoint http://[serverurl]:[nako-port]/api/stats  
Swagger documentation http://[serverurl]:[nako-port]/swagger

##### Bitcoin/Altcoin daemon (bitcoind) or qt
```
port=[client-port]  
server=1  

rpcuser=[rpc-user]  
rpcpassword=[rpc-password]  
rpctimeout=30  
rpcport=[client-rpc-port]  
rpcthreads=200  

txindex=1  
rpcssl=1  
```

##### MongoDB

mongod.exe --dbpath C:\\[mongo-path]\mongo --port [mongodb-port]  


##### Nako (in the nako.config file)
```xml
<add key="ConnectionString" value="mongodb://[serverurl]:[mongodb-port]" />  
<add key="CoinTag" value="BTC" />  
<add key="RpcDomain" value="127.0.0.1" />  
<add key="RpcSecure" value="true" />  
<add key="RpcAccessPort" value="[client-rpc-port]" />  
<add key="RpcUser" value="[rpc-user]" />  
<add key="RpcPassword" value="[rpc-password]" />  
<add key="StartBlockIndex" value="1" />
<add key="SyncApiPort" value="[nako-port]" />
```

Nako.exe C:\\[nako-path]\nako.config
