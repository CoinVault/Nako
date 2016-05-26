// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoBuilder.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Mongo
{
    #region Using Directives

    using System.Threading.Tasks;

    using MongoDB.Driver;

    using Nako.Config;
    using Nako.Storage.Mongo.Types;
    using Nako.Sync.SyncTasks;

    #endregion

    /// <summary>
    /// The mongo builder.
    /// </summary>
    public class MongoBuilder : TaskStarter
    {
        /// <summary>
        /// The running.
        /// </summary>
        private static bool running;

        /// <summary>
        /// The mongo data.
        /// </summary>
        private readonly MongoData mongoData;

        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoBuilder"/> class.
        /// </summary>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="nakoConfiguration">
        /// The Configuration.
        /// </param>
        public MongoBuilder(Tracer tracer, MongoData data, NakoConfiguration nakoConfiguration)
            : base(tracer)
        {
            this.tracer = tracer;
            this.mongoData = data;
            this.configuration = nakoConfiguration;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public override int Priority
        {
            get
            {
                return 20;
            }
        }

        /// <summary>
        /// The on execute.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnExecute()
        {
            this.tracer.Trace("MongoBuilder", "Creating mappings");

            if (!MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MapBlock)))
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MapBlock>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdMember(c => c.BlockHash);
                    });
            }

            if (!MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MapTransactionAddress)))
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MapTransactionAddress>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdMember(c => c.Id);
                    });
            }

            if (!MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MapTransactionBlock)))
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MapTransactionBlock>(cm =>
                    {
                        cm.AutoMap();
                        cm.MapIdMember(c => c.TransactionId);
                    });
            }

            // indexes
            this.tracer.Trace("MongoBuilder", "Creating indexes");

            var blkIndex = Builders<MapBlock>.IndexKeys.Ascending(blk => blk.BlockIndex);
            this.mongoData.MapBlock.Indexes.CreateOne(blkIndex);

            var addrIndex = Builders<MapTransactionAddress>.IndexKeys.Ascending(addr => addr.Addresses);
            this.mongoData.MapTransactionAddress.Indexes.CreateOne(addrIndex);
            var addrBlockIndex = Builders<MapTransactionAddress>.IndexKeys.Ascending(addr => addr.BlockIndex);
            this.mongoData.MapTransactionAddress.Indexes.CreateOne(addrBlockIndex);

            var trxBlkIndex = Builders<MapTransactionBlock>.IndexKeys.Ascending(trxBlk => trxBlk.BlockIndex);
            this.mongoData.MapTransactionBlock.Indexes.CreateOne(trxBlkIndex);

            ////if (this.configuration.CoinTag == "DOGE")
            ////{
            ////    // ugly hack to complete DOGE double db update

            ////    // Conversion fields
            ////    this.tracer.Trace("MongoBuilder", "Converting fields");

            ////    if (running)
            ////    {
            ////        return Task.FromResult(1);
            ////    }

            ////    running = true;

            ////    new Task(() =>
            ////        {
            ////            long blockIndex = 200000;

            ////            while (true)
            ////            {
            ////                var filterb = Builders<MapBlock>.Filter.Eq(info => info.BlockIndex, blockIndex);
            ////                var block = this.mongoData.MapBlock.Find(filterb).ToList().FirstOrDefault();

            ////                if (block == null)
            ////                {
            ////                    break;
            ////                }

            ////                var filter = Builders<MapTransactionAddress>.Filter.Eq(info => info.BlockIndex, blockIndex);
            ////                var collection = this.mongoData.MapTransactionAddress.Find(filter).ToList();

            ////                if (!collection.Any())
            ////                {
            ////                    blockIndex++;
            ////                    continue;
            ////                }

            ////                this.tracer.Trace("MongoBuilder", string.Format("Fetching Block {0} - Items {1}", blockIndex, collection.Count()));

            ////                collection.ForEach(x =>
            ////                    {
            ////                        this.tracer.Trace("MongoBuilder", string.Format("Block {0} - Updating field {1}", blockIndex, x.Id));

            ////                        var filterInner = Builders<MapTransactionAddress>.Filter.Eq(info => info.Id, x.Id);
            ////                        var update = Builders<MapTransactionAddress>.Update.Set(info => info.Value, x.Value);
            ////                        this.mongoData.MapTransactionAddress.UpdateOne(filterInner, update);
            ////                    });

            ////                blockIndex++;
            ////            }

            ////            this.tracer.Trace("MongoBuilder", string.Format("Complete Converting"));
            ////        }).Start();
            ////}
            return Task.FromResult(1);
        }
    }
}
