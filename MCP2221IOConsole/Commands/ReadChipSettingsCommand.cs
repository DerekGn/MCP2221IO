using McMaster.Extensions.CommandLineUtils;
using System;

namespace MCP2221IOConsole.Commands
{
    [Command("read", Description = "Read Device Chip Settings")]
    class ReadChipSettingsCommand : BaseCommand
    {
        public ReadChipSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                console.WriteLine($"{nameof(device.UsbManufacturerDescriptor)}:\t[{device.UsbManufacturerDescriptor}]");
                console.WriteLine($"{nameof(device.UsbSerialNumberDescriptor)}:\t[{device.UsbSerialNumberDescriptor}]");
                console.WriteLine($"{nameof(device.UsbProductDescriptor)}:\t\t[{device.UsbProductDescriptor}]");
                return 0;
            });
        }
    }
}
