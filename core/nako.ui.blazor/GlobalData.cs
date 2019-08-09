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

        private static DateTimeOffset unixRef = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTime UnixTimeToDateTime(ulong timestamp)
        {
            TimeSpan span = TimeSpan.FromSeconds(timestamp);
            var blocktime = (unixRef + span);
            return blocktime.UtcDateTime;
        }

        public static string ToUnit(long satoshi)
        {
            var res = (decimal) satoshi / 100000000;

            return res.ToString("###,###,##0.00######");
        }

        public static string DateTimeAgo(DateTime dateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
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
