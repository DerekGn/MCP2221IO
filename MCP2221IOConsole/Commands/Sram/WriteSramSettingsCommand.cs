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
#warning TODO
using McMaster.Extensions.CommandLineUtils;
using MCP2221IO.Settings;
using System;

namespace MCP2221IOConsole.Commands.Sram
{
    [Command(Description = "Write Device SRAM Settings")]
    internal class WriteSramSettingsCommand : BaseCommand
    {
        public WriteSramSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option("-ar", "The ADC reference voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, AdcRefOption Value) AdcRef { get; set; }

        [Option("-av", "The ADC reference Vrm voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, VrmRef Value) AdcRefVrm { get; set; }

        [Option("-cd", "The clock divider setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockOutDivider Value) ClockDivider { get; set; }

        [Option("-dc", "The clock duty cycle setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockDutyCycle Value) DutyCycle { get; set; }

        [Option("-do", "The Dac output value", CommandOptionType.SingleValue)]
        public (bool HasValue, byte Value) DacOutput { get; set; }

        [Option("-dr", "The Dac reference value", CommandOptionType.SingleValue)]
        public (bool HasValue, DacRefOption Value) DacRef { get; set; }

        [Option("-dv", "The Dac reference Vrm voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, VrmRef Value) DacRefVrm { get; set; }

        [Option("-in", "Enable Interrupt detection will trigger on negative edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptNegativeEdge { get; set; }

        [Option("-ip", "Enable Interrupt detection will trigger on positive edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptPositiveEdge { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                device.ReadSramSettings();

                if (AdcRef.HasValue)
                {
                    device.SramSettings.AdcRefOption = AdcRef.Value;
                }

                if(AdcRefVrm.HasValue)
                {
                    device.SramSettings.AdcRefVrm = AdcRefVrm.Value;
                }

                if (ClockDivider.HasValue)
                {
                    device.SramSettings.ClockDivider = ClockDivider.Value;
                }

                if (DutyCycle.HasValue)
                {
                    device.SramSettings.ClockDutyCycle = DutyCycle.Value;
                }

                if (DacOutput.HasValue)
                {
                    device.SramSettings.DacOutput = DacOutput.Value;
                }

                if (DacRef.HasValue)
                {
                    device.SramSettings.DacRefOption = DacRef.Value;
                }

                if (DacRefVrm.HasValue)
                {
                    device.SramSettings.DacRefVrm = DacRefVrm.Value;
                }

                if (InterruptNegativeEdge.HasValue)
                {
                    device.SramSettings.InterruptNegativeEdge = InterruptNegativeEdge.Value;
                }

                if (InterruptPositiveEdge.HasValue)
                {
                    device.SramSettings.InterruptPositiveEdge = InterruptPositiveEdge.Value;
                }

                return 0;
            });
        }
    }
}