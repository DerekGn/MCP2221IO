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

using McMaster.Extensions.CommandLineUtils;
using MCP2221IO.Gpio;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MCP2221IOConsole.Commands.Gpio
{
    [Command(Description = "Write Device GPIO Settings")]
    internal class WriteGpioSettingsCommand : BaseCommand
    {
        public WriteGpioSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Range(0, 3)]
        [Option(Description = "The GPIO Port")]
        public int[] Ports { get; set; }

        [Option(Description = "The GP pin is set as input if set")]
        public (bool HasValue, bool Value) IsInput { get; set; }

        [Option(Description = "The output value of the GPIO Port")]
        public (bool HasValue, bool Value) Output { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                int result = -1;

                if (Ports != null && (IsInput.HasValue || Output.HasValue))
                {
                    bool updated = false;

                    var ports = Ports.Distinct();

                    device.ReadGpioPorts();

                    foreach (var port in ports)
                    {
                        if (port == 0)
                        {
                            updated |= UpdatePort(0, device.GpioPort0, console);
                        }
                        else if (port == 1)
                        {
                            updated |= UpdatePort(1, device.GpioPort1, console);
                        }
                        else if (port == 2)
                        {
                            updated |= UpdatePort(2, device.GpioPort2, console);
                        }
                        else if (port == 3)
                        {
                            updated |= UpdatePort(3, device.GpioPort3, console);
                        }
                    }

                    if (updated)
                    {
                        device.WriteGpioPorts();

                        console.WriteLine($"Ports Updated");
                    }
                    else
                    {
                        console.WriteLine($"No Ports Updated");
                    }
                    result = 0;
                }
                else
                {
                    console.Error.WriteLine("No update values specified");
                    app.ShowHelp();
                }

                return result;
            });
        }

        private bool UpdatePort(int index, GpioPort gpioPort, IConsole console)
        {
            if(gpioPort.Enabled)
            {
                if (IsInput.HasValue)
                {
                    gpioPort.IsInput = IsInput.Value;
                }

                if (Output.HasValue)
                {
                    gpioPort.Value = Output.Value;
                }
            }
            else
            {
                console.Error.WriteLine($"Port [{index}] Not Configured For GPIO");
            }

            return gpioPort.Enabled;
        }
    }
}
