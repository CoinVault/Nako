using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nako.Ui.Blazor
{
    public class GlobalData
    {
        public GlobalData()
        {
            this.ApiUrl = "209.97.177.144:9040";
            this.BlocksCache = new LimitedDictionary<long, DataTypes.QueryBlock>();
        }

        public string ApiUrl { get; }

        /// <summary>
        /// TODO: fix this to have a max count in memory
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
