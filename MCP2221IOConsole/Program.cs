using CommandLine;
using System.Collections.Generic;

namespace MCP2221IOConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options options)
        {
        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
        }
    }
}
