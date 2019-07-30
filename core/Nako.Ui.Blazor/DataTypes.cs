using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nako.Ui.Blazor
{
    // TODO: Make a common datatypes project with the api
    public class DataTypes
    {
        public class QueryBlocks
        {
            public QueryBlock[] Blocks { get; set; }
        }

        public class QueryBlock
        {
            public string CoinTag { get; set; }
            public string BlockHash { get; set; }
            public long BlockIndex { get; set; }
            public long BlockSize { get; set; }
            public long BlockTime { get; set; }
            public string NextBlockHash { get; set; }
            public string PreviousBlockHash { get; set; }
            public bool Synced { get; set; }
            public int TransactionCount { get; set; }
            public long Confirmations { get; set; }
            public string Bits { get; set; }
            public string Merkleroot { get; set; }
            public long Nonce { get; set; }
            public long Version { get; set; }
            public string PosBlockSignature { get; set; }
            public string PosModifierv2 { get; set; }
            public string PosFlags { get; set; }
            public string PosHashProof { get; set; }
            public string PosBlockTrust { get; set; }
            public string PosChainTrust { get; set; }
            public IEnumerable<string> Transactions { get; set; }
        }

        public class PeerInfo
        {
            public uint Id { get; set; }
            public string Addr { get; set; }
            public string AddrLocal { get; set; }
            public string Services { get; set; }
            public long LastSend { get; set; }
            public long LastRecv { get; set; }
            public long BytesSent { get; set; }
            public long BytesRecv { get; set; }
            public int ConnTime { get; set; }
            public double PingTime { get; set; }
            public double Version { get; set; }
            public string SubVer { get; set; }
            public bool Inbound { get; set; }
            public long StartingHeight { get; set; }
            public int BanScore { get; set; }
            public long SyncedHeaders { get; set; }
            public long SyncedBlocks { get; set; }
            public IList<long> InFlight { get; set; }
            public bool WhiteListed { get; set; }
        }
        public class Statistics
        {
            public string CoinTag { get; set; }
            public string Progress { get; set; }
            public int TransactionsInPool { get; set; }
            public long SyncBlockIndex { get; set; }
            public ClientInfo ClientInfo { get; set; }
            public string BlocksPerMinute { get; set; }
        }

        public class ClientInfo
        {
            public string Version { get; set; }
            public string ProtocolVersion { get; set; }
            public string WalletVersion { get; set; }
            public decimal Balance { get; set; }
            public long Blocks { get; set; }
            public double TimeOffset { get; set; }
            public long Connections { get; set; }
            public string Proxy { get; set; }
            //public double Difficulty { get; set; }
            public bool Testnet { get; set; }
            public long KeyPoolEldest { get; set; }
            public long KeyPoolSize { get; set; }
            public decimal PayTxFee { get; set; }
            public decimal RelayTxFee { get; set; }
            public string Errors { get; set; }
        }

    }
}
