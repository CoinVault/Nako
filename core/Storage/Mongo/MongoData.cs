// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoData.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Mongo
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations.Types;
    using Nako.Storage.Mongo.Types;
    using Nako.Storage.Types;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class MongoData : IStorage
    {
        private readonly ILogger<MongoStorageOperations> log;

        private readonly MongoClient mongoClient;

        private readonly IMongoDatabase mongoDatabase;

        private readonly SyncConnection syncConnection;

        private readonly NakoConfiguration configuration;

        public MongoData(ILogger<MongoStorageOperations> logger, SyncConnection connection, IOptions<NakoConfiguration> nakoConfiguration)
        {
            this.syncConnection = connection;
            this.log = logger;
            this.configuration = nakoConfiguration.Value;
            this.mongoClient = new MongoClient(this.configuration.ConnectionString);
            this.mongoDatabase = this.mongoClient.GetDatabase("Blockchain");
            this.MemoryTransactions = new ConcurrentDictionary<string, DecodedRawTransaction>();
        }

        public IMongoCollection<MapTransactionAddress> MapTransactionAddress
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapTransactionAddress>("MapTransactionAddress");
            }
        }

        public IMongoCollection<MapTransactionBlock> MapTransactionBlock
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapTransactionBlock>("MapTransactionBlock");
            }
        }

        public IMongoCollection<MapBlock> MapBlock
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapBlock>("MapBlock");
            }
        }

        public ConcurrentDictionary<string, DecodedRawTransaction> MemoryTransactions { get; set; }

        public IEnumerable<SyncBlockInfo> BlockGetIncompleteBlocks()
        {
            // note this field is not indexed
            var filter = Builders<MapBlock>.Filter.Eq(info => info.SyncComplete, false);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert);
        }

        public IEnumerable<SyncBlockInfo> BlockGetBlockCount(int count)
        {
            var filter = Builders<MapBlock>.Filter.Exists(info => info.BlockIndex);
            var sort = Builders<MapBlock>.Sort.Descending(info => info.BlockIndex);

            return this.MapBlock.Find(filter).Sort(sort).Limit(count).ToList().Select(this.Convert);
        }

        public IEnumerable<SyncBlockInfo> BlockGetCompleteBlockCount(int count)
        {
            var blocks = this.BlockGetBlockCount(2).ToList();
            return blocks.Where(b => b.SyncComplete);
        }

        public SyncBlockInfo BlockGetByIndex(long blockIndex)
        {
            var filter = Builders<MapBlock>.Filter.Eq(info => info.BlockIndex, blockIndex);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert).FirstOrDefault();
        }

        public SyncBlockInfo BlockGetByHash(string blockHash)
        {
            var filter = Builders<MapBlock>.Filter.Eq(info => info.BlockHash, blockHash);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert).FirstOrDefault();
        }

        public void InsertBlock(MapBlock info)
        {
            this.MapBlock.InsertOne(info);
        }

        public void CompleteBlock(string blockHash)
        {
            var filter = Builders<MapBlock>.Filter.Eq(blockInfo => blockInfo.BlockHash, blockHash);
            var update = Builders<MapBlock>.Update.Set(blockInfo => blockInfo.SyncComplete, true);
            this.MapBlock.UpdateOne(filter, update);
        }

        public void MarkOutput(string transaction, int index, string spendingTransactionId)
        {
            var filter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.Id, string.Format("{0}-{1}", transaction, index));
            var update = Builders<MapTransactionAddress>.Update.Set(blockInfo => blockInfo.SpendingTransactionId, spendingTransactionId);
            this.MapTransactionAddress.UpdateOne(filter, update);
        }

        public SyncTransactionInfo BlockTransactionGet(string transactionId)
        {
            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.TransactionId, transactionId);

            var trx = this.MapTransactionBlock.Find(filter).FirstOrDefault();
            if (trx == null)
            {
                return null;
            }

            var current = this.BlockGetBlockCount(1).First();

            var blk = this.BlockGetByIndex(trx.BlockIndex);
                
            return new SyncTransactionInfo
            {
                BlockIndex = trx.BlockIndex, 
                BlockHash = blk.BlockHash, 
                Timestamp = blk.BlockTime, 
                TransactionHash = trx.TransactionId, 
                Confirmations = current.BlockIndex - trx.BlockIndex
            };
        }

        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlock(string blockHash)
        {
            var blk = this.BlockGetByHash(blockHash);
            var current = this.BlockGetBlockCount(1).First();

            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, blk.BlockIndex);
            var trxs = this.MapTransactionBlock.Find(filter).ToList();

            return trxs.Select(s => new SyncTransactionInfo
                                        {
                                            BlockIndex = s.BlockIndex, 
                                            BlockHash = blk.BlockHash, 
                                            Timestamp = blk.BlockTime, 
                                            TransactionHash = s.TransactionId, 
                                            Confirmations = current.BlockIndex - s.BlockIndex
                                        });
        }

        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlockIndex(long blockIndex)
        {
            var blk = this.BlockGetByIndex(blockIndex);
            var current = this.BlockGetBlockCount(1).First();

            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, blk.BlockIndex);
            var trxs = this.MapTransactionBlock.Find(filter).ToList();

            return trxs.Select(s => new SyncTransactionInfo
            {
                BlockIndex = s.BlockIndex, 
                BlockHash = blk.BlockHash, 
                Timestamp = blk.BlockTime, 
                TransactionHash = s.TransactionId, 
                Confirmations = current.BlockIndex - s.BlockIndex
            });
        }

        public SyncTransactionItemOutput TransactionsGet(string transactionId, int index, SyncTransactionIndexType indexType)
        {
            throw new NotImplementedException();
        }

        public SyncTransactionItems TransactionItemsGet(string transactionId)
        {
            var client = CryptoClientFactory.Create(this.syncConnection.ServerDomain, this.syncConnection.RpcAccessPort, this.syncConnection.User, this.syncConnection.Password, this.syncConnection.Secure);

            var res = client.GetRawTransactionAsync(transactionId, 1).Result;

            return new SyncTransactionItems
                       {
                           Inputs = res.VIn.Select(v => new SyncTransactionItemInput{ PreviousTransactionHash = v.TxId, PreviousIndex = v.VOut, InputCoinBase = v.CoinBase}).ToList(), 
                           Outputs = res.VOut.Where(v => v.ScriptPubKey != null && v.ScriptPubKey.Addresses != null).Select(v=> new SyncTransactionItemOutput { Address = v.ScriptPubKey.Addresses.FirstOrDefault(), Index = v.N, Value = v.Value, OutputType = v.ScriptPubKey.Type }).ToList()
                       };
        }

        public SyncTransactionAddressBalance AddressGetBalance(string address, long confirmations)
        {
            var current = this.BlockGetBlockCount(1).First();

            var addrs = this.SelectAddressWithPool(current, address, false).ToList();
            return this.CreateAddresBalance(confirmations, addrs, false);
        }

        public SyncTransactionAddressBalance AddressGetBalanceUtxo(string address, long confirmations)
        {
            var current = this.BlockGetBlockCount(1).First();

            var addrs = this.SelectAddressWithPool(current, address, true).ToList();
            return this.CreateAddresBalance(confirmations, addrs, true);
        }

        public void DeleteBlock(string blockHash)
        {
            var block = this.BlockGetByHash(blockHash);

            // delete the outputs
            var addrFilter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.BlockIndex, block.BlockIndex);
            this.MapTransactionAddress.DeleteMany(addrFilter);

            // delete the transaction
            var transactionFilter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, block.BlockIndex);
            this.MapTransactionBlock.DeleteMany(transactionFilter);

            // delete the block itself.
            var blockFilter = Builders<MapBlock>.Filter.Eq(info => info.BlockHash, blockHash);
            this.MapBlock.DeleteOne(blockFilter);
        }

        public IEnumerable<DecodedRawTransaction> GetMemoryTransactions()
        {
            return this.MemoryTransactions.Values;
        }

        #region private

        private SyncTransactionAddressBalance CreateAddresBalance(long confirmations, List<SyncTransactionAddressItem> addrs, bool availableOnly)
        {
            var all = addrs.Where(s => s.Confirmations >= confirmations).Sum(s => s.Value);
            var confirming = addrs.Where(s => s.Confirmations < confirmations).Sum(s => s.Value);
            var used = addrs.Where(s => s.SpendingTransactionHash != null).Sum(s => s.Value);
            var available = all - used;

            return new SyncTransactionAddressBalance
                       {
                           Available = available, 
                           Received = availableOnly ? default(decimal?) : all, 
                           Sent = availableOnly ? default(decimal?) : used, 
                           Unconfirmed = confirming, 
                           Items = addrs
                       };
        }

        private SyncBlockInfo Convert(MapBlock block)
        {
            return new SyncBlockInfo { BlockIndex = block.BlockIndex, BlockSize = block.BlockSize, BlockHash = block.BlockHash, BlockTime = block.BlockTime, NextBlockHash = block.NextBlockHash, PreviousBlockHash = block.PreviousBlockHash, TransactionCount = block.TransactionCount, SyncComplete = block.SyncComplete };
        }

        private IEnumerable<SyncTransactionAddressItem> SelectAddressWithPool(SyncBlockInfo current, string address, bool availableOnly)
        {
            // this code will not work as we need to have the actual transactions to deduct their value in case a memory pool transaction is found.
            // this is not way to know if a mem pool transaction belongs to an address with heavily querying mongo, so the best solution (for now)
            // is to just fetch the entire history,  this could be limited to the available balance only.

            ////var confirmations = 3;
  
            //// var res = this.MapTransactionAddress.Aggregate()
            ////    .Match(new BsonDocument
            ////        {
            ////            new BsonElement("Addresses", new BsonArray(new[] { address })),
            ////            // in case we only want unspet values un comment this row
            ////            //new BsonElement("SpendingTransactionId", new BsonDocument("$eq", BsonNull.Value))
            ////        })
            ////    .Project(new BsonDocument
            ////        {
            ////            new BsonElement("confirmed", new BsonDocument(new BsonElement("$lte", new BsonArray(new[] {"$BlockIndex", (object)(current.BlockIndex - confirmations + 1)})))),
            ////            new BsonElement("val", new BsonString("$Value")),
            ////            new BsonElement("spent", new BsonDocument(new BsonElement("$ne", new BsonArray(new[] {"$SpendingTransactionId", (object)BsonNull.Value})))),
									
            ////        })
            ////    .Group(new BsonDocument
            ////        {
            ////            new BsonElement("_id", new BsonDocument{ {"Confirmed", new BsonString("$confirmed") }, { new BsonDocument("Spent", new BsonString("$spent")) }}),
            ////            new BsonElement("TotalAmount", new BsonDocument("$sum", new BsonString("$val"))),       
            ////            new BsonElement("Count", new BsonDocument("$sum", 1)),                  
            ////        });

            //// var results = res.ToList().Select(s => BsonSerializer.Deserialize<SelectBalanceResult>(s)).ToList();
            ////var enumerated = results.ToList();

            ////var received = enumerated.Where(w => w.id.Confirmed).Sum(s => s.TotalAmount);
            ////var sent = enumerated.Where(w => w.id.Spent).Sum(s => s.TotalAmount);
            ////var uncinfirmed = enumerated.Where(w => !w.id.Confirmed).Sum(s => s.TotalAmount);

            ////// the mongo aggregator may change the precision so we corrected here limiting to 8 digits.
            ////var balanceNew = new SyncTransactionAddressBalance
            ////{
            ////    Received = System.Convert.ToDecimal(received.ToString("#0.########")),
            ////    Sent = System.Convert.ToDecimal(sent.ToString("#0.########")),
            ////    Unconfirmed = System.Convert.ToDecimal(uncinfirmed.ToString("#0.########")),
            ////};
            
            var builder = Builders<MapTransactionAddress>.Filter;
            var filter = builder.Eq(info => info.Addresses, new List<string> { address });

            if (availableOnly)
            {
                // we only want spendable transactions    
                filter = filter & builder.Eq(info => info.SpendingTransactionId, null);
            }

            var stoper1 = Stopwatch.Start();

            var addrs = this.MapTransactionAddress.Find(filter).ToList();

            stoper1.Stop();

            this.log.LogInformation($"Select: Seconds = {stoper1.Elapsed.TotalSeconds} - UnspentOnly = {availableOnly} - Addr = {address} - Items = {addrs.Count()}");

            // this creates a copy of the collection (to avoid thread issues)
            var pool = this.MemoryTransactions.Values;

            if (pool.Any())
            {
                // mark trx in output as spent if they exist in the pool
                var addrsupdate = addrs;
                this.GetPoolOutputs(pool).ForEach(f =>
                {
                        var adr = addrsupdate.FirstOrDefault(a => a.TransactionId == f.Item1.TxId && a.Index == f.Item1.VOut);
                        if (adr != null)
                        {
                            adr.SpendingTransactionId = f.Item2;
                        }
                    });

                // if only spendable transactions are to be returned we need to remove 
                // any that have been marked as spent by a transaction in the pool
                if (availableOnly)
                {
                    addrs = addrs.Where(d => d.SpendingTransactionId == null).ToList();
                }

                // add all pool transactions to main output
                var paddr = this.PoolToMapTransactionAddress(pool, address).ToList();
                addrs = addrs.Concat(paddr).ToList();
            }

            // map to return type and calculate confirmations
            return addrs.Select(s => new SyncTransactionAddressItem
            {
                Address = address, 
                Index = s.Index, 
                TransactionHash = !s.CoinBase ? s.TransactionId : string.Empty, 
                BlockIndex = s.BlockIndex == -1 ? default(long?) : s.BlockIndex, 
                Value = System.Convert.ToDecimal(s.Value), 
                Confirmations = s.BlockIndex == -1 ? 0 : current.BlockIndex - s.BlockIndex + 1, 
                SpendingTransactionHash = s.SpendingTransactionId, 
                CoinBase = s.CoinBase ? s.TransactionId : string.Empty, 
                ScriptHex = s.ScriptHex
            });
        }

        private IEnumerable<Tuple<Vin, string>> GetPoolOutputs(IEnumerable<DecodedRawTransaction> pool)
        {
            return pool.SelectMany(s => s.VIn.Select(v => new Tuple<Vin, string>(v, s.TxId)));
        }

        private IEnumerable<MapTransactionAddress> PoolToMapTransactionAddress(IEnumerable<DecodedRawTransaction> pool, string address)
        {
            foreach (var transaction in pool)
            {
                var rawTransaction = transaction;
                var coinBase = rawTransaction.VIn.Any(v => v.CoinBase != null);

                var transactionOutputs = from output in rawTransaction.VOut
                                         where output.Value > 0
                                                 && output.ScriptPubKey != null
                                                 && output.ScriptPubKey.Addresses != null
                                                 && output.ScriptPubKey.Addresses.Any()
                                                 && output.ScriptPubKey.Addresses.Contains(address)
                                         select new MapTransactionAddress
                                         {
                                             Id = string.Format("{0}-{1}", rawTransaction.TxId, output.N), 
                                             TransactionId = rawTransaction.TxId, 
                                             Value = System.Convert.ToDouble(output.Value), 
                                             Index = output.N, 
                                             Addresses = output.ScriptPubKey.Addresses, 
                                             BlockIndex = -1, 
                                             CoinBase = coinBase
                                         };

                foreach (var output in transactionOutputs)
                {
                    yield return output;
                }
            }
        }

        public class SelectBalanceResult
        {
            public double TotalAmount { get; set; }

            public int Count { get; set; }

            public SelectStats id { get; set; }

            public class SelectStats
            {
                public bool Spent { get; set; }
                public bool Confirmed { get; set; }
            }
        }

        #endregion
    }
}
