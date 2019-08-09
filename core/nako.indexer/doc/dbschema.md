### Nako indexer uses mongodb to store the blockchain data

The tables can be found here  
https://github.com/CoinVault/Nako/tree/master/core/nako.indexer/Storage/Mongo/Types

And the indexer builder here  
https://github.com/CoinVault/Nako/blob/master/core/nako.indexer/Storage/Mongo/MongoBuilder.cs

**There are currently 4 tables:**

`MapBlock` - Stores information about a block and creates an index based on the block hash and block height.  

`MapTransaction` - Stores the serialized raw trasnaction and index by the transaction hash (it's optional and transactions will be stored in this table if the config falg `StoreRawTransactions` is true otherwise trx will be pulled using RPC.  

`MapTransactionBlock` - Stores a link between a block height and a transaction.  

`MapTransactionAddress` - Stores spent and unspent TXOs related to an address, if the TXO is spent the `SpendingTransactionId` field has will be updated, to calculate the total balance of an address the entire UTXO set needs to be fetched and then sumall entreis where the spending hash is set to null.  

TODO:

`MapTransactionAddressComputed` -  A computed data of an adddess balance, this will be calculated on demand and be built per address, when an address has additional UTXOs only the diff form the block height of computation will be taken and recomputed, if a reorg happens the entry will be delted if the fork is bellow the first TXO.
