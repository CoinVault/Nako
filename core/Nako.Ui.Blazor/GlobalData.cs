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
            this.BlocksCache = new Dictionary<long, DataTypes.QueryBlock>();
        }

        public string Search { get; set; }

        /// <summary>
        /// TODO: fix this to have a max count in memory
        /// </summary>
        public Dictionary<long, DataTypes.QueryBlock> BlocksCache;
    }
}
