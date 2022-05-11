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
using MCP2221IO.Settings;
using System;
using System.ComponentModel.DataAnnotations;

namespace MCP2221IOConsole.Commands.Sram
{
    [Command(Name = "write-sram-settings", Description = "Write MCP2221 SRAM settings")]
    internal class SramWriteSettingsCommand : BaseCommand
    {
        public SramWriteSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.AdcRef, "The ADC reference voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, AdcRefOption Value) AdcRef { get; set; }

        [Option(Templates.AdcVrm, "The ADC VRM voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, VrmRef Value) AdcVrmRef { get; set; }

        [Option(null, "The Clock Divider setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockOutDivider Value) ClockDivider { get; set; }

        [Option(null, "The Clock Duty Cycle setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockDutyCycle Value) DutyCycle { get; set; }

        [Range(0, 15)]
        [Option(Templates.DacOutput, "The DAC output value", CommandOptionType.SingleValue)]
        public (bool HasValue, byte Value) DacOutput { get; set; }

        [Option(Templates.DacRef, "The DAC reference value", CommandOptionType.SingleValue)]
        public (bool HasValue, DacRefOption Value) DacRef { get; set; }

        [Option(Templates.DacVrm, "The DAC VRM voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, VrmRef Value) DacRefVrm { get; set; }

        [Option(Templates.InterruptNegative, "Enable Interrupt detection will trigger on negative edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptNegativeEdge { get; set; }

        [Option(Templates.InterruptPositive, "Enable Interrupt detection will trigger on positive edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptPositiveEdge { get; set; }

        [Option(Templates.ClearInterrupt, "Clear Interrupt", CommandOptionType.SingleValue)]
        public bool ClearInterrupt { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                bool modified = false;

                device.ReadSramSettings();

                if (AdcRef.HasValue)
                {
                    device.SramSettings.AdcRefOption = AdcRef.Value;
                    modified = true;
                }

                if(AdcVrmRef.HasValue)
                {
                    device.SramSettings.AdcRefVrm = AdcVrmRef.Value;
                    modified |= true;
                }

                if (ClockDivider.HasValue)
                {
                    device.SramSettings.ClockDivider = ClockDivider.Value;
                    modified |= true;
                }

                if (DutyCycle.HasValue)
                {
                    device.SramSettings.ClockDutyCycle = DutyCycle.Value;
                    modified |= true;
                }

                if (DacOutput.HasValue)
                {
                    device.SramSettings.DacOutput = DacOutput.Value;
                    modified |= true;
                }

                if (DacRef.HasValue)
                {
                    device.SramSettings.DacRefOption = DacRef.Value;
                    modified |= true;
                }

                if (DacRefVrm.HasValue)
                {
                    device.SramSettings.DacRefVrm = DacRefVrm.Value;
                    modified |= true;
                }

                if (InterruptNegativeEdge.HasValue)
                {
                    device.SramSettings.InterruptNegativeEdge = InterruptNegativeEdge.Value;
                    modified |= true;
                }

                if (InterruptPositiveEdge.HasValue)
                {
                    device.SramSettings.InterruptPositiveEdge = InterruptPositiveEdge.Value;
                    modified |= true;
                }

                modified |= ClearInterrupt;

                if (modified)
                {
                    device.WriteSramSettings(ClearInterrupt);
                    console.WriteLine("SRAM settings updated");
                }
                else
                {
                    console.Error.WriteLine("No update values specified");
                    app.ShowHelp();
                }

                return 0;
            });
        }
    }
}