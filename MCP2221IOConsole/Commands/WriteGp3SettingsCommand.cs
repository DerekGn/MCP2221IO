
using McMaster.Extensions.CommandLineUtils;
using MCP2221IO.Gp;
using System;

namespace MCP2221IOConsole.Commands
{
    [Command("gp3", Description = "Write GP3 Power Up Settings")]
    internal class WriteGp3SettingsCommand : BaseWriteGpSetingsCommand
    {
        public WriteGp3SettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option("-d", Description = "The GP3 Power Up Designation")]
        public (bool HasValue, Gp3Designation Value) Designation { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                ApplySettings(device);

                if (!(SettingsApplied() && Designation.HasValue))
                {
                    console.Error.WriteLine("No update values specified");
                    app.ShowHelp();
                }
                else
                {
                    device.GpSettings.Gp3PowerUpSetting.Designation = Designation.Value;

                    device.WriteGpSettings();

                    console.WriteLine("GP3 Settings Updated");
                }

                return 0;
            });
        }
    }
}
