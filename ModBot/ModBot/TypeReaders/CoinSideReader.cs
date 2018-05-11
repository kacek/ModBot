using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ModBot.Enums;

namespace ModBot.TypeReaders
{
    public class CoinSideReader : TypeReader
    {
        public override Task<TypeReaderResult> Read(ICommandContext context, string input, IServiceProvider services)
        {
            switch (input.ToLower())
            {
                case "tail":
                case "reszka":
                    return Task.FromResult(TypeReaderResult.FromSuccess(CoinSide.Tail));
                case "head":
                case "orzeł":
                case "orzel":
                    return Task.FromResult(TypeReaderResult.FromSuccess(CoinSide.Head));

                default:
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Nie rozpoznano strony monety!"));
            }
        }
    }
}