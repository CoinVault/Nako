# Nako
A server application to index Blockchain transactions by addresses for Bitcoin and Altcoins.  
Nako exposes a REST api to query the blockchain data.

## Features
- Language C#
- Auther: Dan Gershony

## Technologies
- Running a full Bitcoin/Altcoin node either daemon or qt 
- Running a MongoDB instance as indexing storage
- OWIN selfhost REST api easily documented using swagger

### Api
Swagger http://[server-url]:[port]/swagger/

#### examples
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
