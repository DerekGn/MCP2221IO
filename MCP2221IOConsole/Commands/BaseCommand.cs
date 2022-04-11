/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

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
