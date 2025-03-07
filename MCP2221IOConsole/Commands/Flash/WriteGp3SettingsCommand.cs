﻿
using McMaster.Extensions.CommandLineUtils;
using MCP2221IO.Gp;
using System;

namespace MCP2221IOConsole.Commands.Flash
{
    [Command(Description = "Write GP3 power up settings")]
    internal class WriteGp3SettingsCommand : BaseWriteGpSettingsCommand
    {
        public WriteGp3SettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.GpDesignation, "The GP3 power up designation", CommandOptionType.SingleValue)]
        public (bool HasValue, Gp3Designation Value) Designation { get; set; }

        protected override int OnExecute(CommandLineApplication application, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                device.ReadGpSettings();

                UpdateGpSetting(application, console, device, device.GpSettings!.Gp3PowerUpSetting!, Designation);

                return 0;
            });
        }
    }
}
