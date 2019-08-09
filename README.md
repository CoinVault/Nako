# Nako block explorer
A server application to index Blockchain transactions by addresses for Bitcoin and Altcoins.  
Nako exposes a REST api to query the blockchain data.

Nako api can be searched by segwit addresses and Cold-Staking (hot and cold key) script types.

CoinVault uses Nako as a block Explorer, Nako is lightweight and uses mongodb to index transactions by addresses.

### Technologies
- dotnet core (and blazor)
- NBitcoin and Stratis.Bitcoin
- Running a full Bitcoin/Altcoin node either daemon or qt 
- Running a MongoDB instance as indexing storage
- OWIN selfhost REST api easily documented using swagger

We user [docker](https://www.docker.com/) (with docker-compose)

#### DB schema
Can be found here:  
https://github.com/CoinVault/Nako/blob/master/core/nako.indexer/doc/dbschema.md

#### Api
Swagger http://[server-url]:[port]/swagger/

##### examples
GET /api/query/address/{address}  
GET /api/query/address/{address}/confirmations/{confirmations}/unspent/transactions  
GET /api/query/address/{address}/unspent/transactions  
GET /api/query/address/{address}/unspent  
GET /api/query/block/Latest/{transactions}  
GET /api/query/block/{blockHash}/{transactions}  
GET /api/query/block/Index/{blockIndex}/{transactions}  
GET /api/query/transaction/{transactionId}  
GET /api/stats  
GET /api/stats/peers  

#### Nako UI
Checkout an experimental blazor ui  
https://github.com/CoinVault/Nako/tree/master/core/nako.ui.blazor
