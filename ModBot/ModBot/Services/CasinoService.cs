using System;
using ModBot.Enums;

namespace ModBot.Services
{
    public class CasinoService
    {
        private static Random _rng = new Random();

        public CoinSide DrawCoinSide()
            => ((_rng.Next(0, 50) % 2) == 1) ? CoinSide.Tail : CoinSide.Head;
    }
}