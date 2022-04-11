using HidSharp;
using McMaster.Extensions.CommandLineUtils;
using MCP2221IO;
using MCP2221IO.Usb;
using Microsoft.Extensions.Logging;
using System;

namespace MCP2221IOConsole.Commands
{
    internal abstract class BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;

        protected BaseCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [Option(Description = "The VID of the MCP2221")]
        public int Vid { get; set; } = 0x04D8;
        [Option(Description = "The PID of the MCP2221")]
        public int Pid { get; set; } = 0x00DD;
        [Option(Description = "The serial number of the MCP2221 instance")]
        public string SerialNumber { get; set; }

        protected int ExecuteCommand(Func<IDevice, int> action)
        {
            int result = -1;

            try
            {
                var hidDevice = DeviceList.Local.GetHidDeviceOrNull(Vid, Pid);

                if (hidDevice != null)
                {
                    using HidSharpHidDevice hidSharpHidDevice = new HidSharpHidDevice((ILogger<IHidDevice>)_serviceProvider.GetService(typeof(ILogger<IHidDevice>)), hidDevice);
                    using MCP2221IO.Device device = new MCP2221IO.Device((ILogger<IDevice>)_serviceProvider.GetService(typeof(ILogger<IDevice>)), hidSharpHidDevice);

                    device.Open();

                    result = action(device);
                }
                else
                {
                    Console.Error.WriteLine($"Unable to find HID device VID: [0x{Vid:x}] PID: [0x{Vid:x}] SerialNumber: [{SerialNumber}]");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unhandled exception occurred: {ex.ToString()}");
            }

            return result;
        }

        protected virtual int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.Error.WriteLine("You must specify a command. See --help for more details.");
            app.ShowHelp();

            return 0;
        }
    }
}
