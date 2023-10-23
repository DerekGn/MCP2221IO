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
using MCP2221IO;
using MCP2221IO.Settings;
using System;

namespace MCP2221IOConsole.Commands.Flash
{
    internal abstract class BaseWriteGpSettingsCommand : BaseCommand
    {
        protected BaseWriteGpSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.GpIsInput, "The GP pin is set as input if set", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) IsInput { get; set; }

        [Option(Templates.GpValue, "The GP output value at power up when pin is set as output", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) Value { get; set; }

        internal void UpdateGpSetting<T>(CommandLineApplication app, IConsole console, IDevice device, GpSetting<T> gpio, (bool HasValue, T Value) designation) where T : Enum
        {
            if (!(IsInput.HasValue || Value.HasValue || designation.HasValue))
            {
                console.Error.WriteLine("No update values specified");
                app.ShowHelp();
            }
            else
            {
                if (IsInput.HasValue)
                {
                    gpio.IsInput = IsInput.Value;
                }

                if (Value.HasValue)
                {
                    gpio.Value = Value.Value;
                }

                if (designation.HasValue)
                {
                    gpio.Designation = designation.Value;
                }

                device.WriteGpSettings();

                console.WriteLine("GP settings updated");
            }
        }
    }
}
