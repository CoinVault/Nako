using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Nako.Ui.Blazor
{
    public class GlobalData
    {
        public GlobalData(HttpClient httpClient)
        {
            // For now api support by default on the same ip host (we use the default port for Nako api 9000)
            this.ApiUrl =  $"{httpClient.BaseAddress.Host}:{9000}";

            this.BlocksCache = new LimitedDictionary<long, DataTypes.QueryBlock>();
        }

        public DataTypes.CoinInfo Info { get; set; }

        public string ApiUrl { get; }

        /// <summary>
        /// TODO: Add expiry time to the collection.
        /// </summary>
        public LimitedDictionary<long, DataTypes.QueryBlock> BlocksCache;
    }

    public class LimitedDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dictionaty;
        private Queue<TKey> queue;
        private int MaxItems;

        public LimitedDictionary()
        {
            this.dictionaty = new Dictionary<TKey, TValue>();
            this.queue = new Queue<TKey>();
            this.MaxItems = 50;
        }

        public void Add(TKey key, TValue value)
        {
            if(!this.dictionaty.ContainsKey(key))
            {
                this.dictionaty.Add(key, value);
                this.queue.Enqueue(key);

                if (this.dictionaty.Count > this.MaxItems)
                {
                    var item = this.queue.Dequeue();
                    this.dictionaty.Remove(item);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionaty.TryGetValue(key, out value);
        }
    }
}
