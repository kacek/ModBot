using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ModBot.models
{
    public class User
    {
        public const uint DEFAULT_WALLET_CNT = 200;

        public UInt64 UID {get; set; }
        public UInt64 Wallet { get; set; }

        public void setUID (UInt64 id)
        {
            UID = id;
        }

        public UInt64 getUID()
        {
            return UID;
        }

        public void setWallet(UInt64 wallet)
        {
            Wallet = wallet;
        }

        public UInt64 getWallet()
        {
            return Wallet;
        }

        public bool ChangeWalletCnt(long amount)
        {
            var min = UInt64.MinValue;
            var max = UInt64.MaxValue;

            BigInteger nV = Wallet;
            nV += amount;

            if (nV > max || nV < min)
                return false;

            Wallet = (UInt64)nV;
            return true;
        }
    }
}
