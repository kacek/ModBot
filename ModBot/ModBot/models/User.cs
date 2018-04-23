using System;
using System.Collections.Generic;
using System.Text;

namespace ModBot.models
{
    class User
    {
        UInt64 UID;
        UInt64 Wallet;

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
    }
}
