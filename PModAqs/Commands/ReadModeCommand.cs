
using McMaster.Extensions.CommandLineUtils;
using System;

namespace PModAqs.Commands
{
    [Command(Description = "Read the sensor mode")]
    internal class ReadModeCommand : BaseCommand
    {
        public ReadModeCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((sensor) =>
            {
                Console.WriteLine($"Mode: {sensor.GetMode()}");

                return 0;
            });
        }
    }
}
