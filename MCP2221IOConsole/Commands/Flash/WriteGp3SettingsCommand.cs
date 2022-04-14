
using McMaster.Extensions.CommandLineUtils;
using MCP2221IO.Gp;
using System;

namespace MCP2221IOConsole.Commands.Flash
{
    [Command(Description = "Write GP3 Power Up Settings")]
    internal class WriteGp3SettingsCommand : BaseWriteGpSetingsCommand
    {
        public WriteGp3SettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option("-d", "The GP3 Power Up Designation", CommandOptionType.SingleValue)]
        public (bool HasValue, Gp3Designation Value) Designation { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                ApplySettings(device);

                if (!(IsInput.HasValue || Value.HasValue || Designation.HasValue))
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
