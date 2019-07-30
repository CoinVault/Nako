using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nako.Ui.Blazor
{
    public class GlobalData
    {
        public string Search { get; set; }

        public Dictionary<long, DataTypes.QueryBlock> BlocksCache = new Dictionary<long, DataTypes.QueryBlock>();
    }
}
