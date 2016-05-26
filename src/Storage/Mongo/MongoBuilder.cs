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
        private readonly MongoData mongoData;

        private readonly Tracer tracer;

        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoBuilder"/> class.
        /// </summary>
        public MongoBuilder(Tracer tracer, MongoData data, NakoConfiguration nakoConfiguration)
            : base(tracer)
        {
            this.tracer = tracer;
            this.mongoData = data;
            this.configuration = nakoConfiguration;
        }

        public override int Priority
        {
            get
            {
                return 20;
            }
        }

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

            return Task.FromResult(1);
        }
    }
}
