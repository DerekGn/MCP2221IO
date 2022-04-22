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

namespace MCP2221IOConsole.Commands.Flash
{
    [Command(Description = "Write Device Chip Settings")]
    internal class WriteChipSettingsCommand : BaseCommand
    {
        public WriteChipSettingsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.CdcSerialNumberEnable, "CDC serial number enumeration enable", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) CdcSerialNumberEnable { get; set; }

        [Option(Templates.ChipSecurity, Description = "Chip configuration security option")]
        public (bool HasValue, ChipSecurity Value) ChipSecurity { get; set; }

        [Option(Templates.AdcRef, "The ADC reference voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, AdcRefOption Value) AdcRef { get; set; }

        [Option(Templates.AdcVrm, "The ADC VRM voltage", CommandOptionType.SingleValue)]
        public (bool HasValue, VrmRef Value) AdcVrmRef { get; set; }

        [Option(null, "The Clock Divider setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockOutDivider Value) ClockDivider { get; set; }

        [Option(null, "The Clock Duty Cycle setting", CommandOptionType.SingleValue)]
        public (bool HasValue, ClockDutyCycle Value) DutyCycle { get; set; }

        [Range(0, 15)]
        [Option(Templates.DacOutput, Description = "The DAC output value")]
        public (bool HasValue, byte Value) DacOutput { get; set; }

        [Option(Templates.DacRef, Description = "The DAC reference value")]
        public (bool HasValue, DacRefOption Value) DacRef { get; set; }

        [Option(Templates.DacVrm, Description = "The DAC VRM voltage")]
        public (bool HasValue, VrmRef Value) DacRefVrm { get; set; }

        [Option(Templates.InterruptNegative, "Enable Interrupt detection will trigger on negative edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptNegativeEdge { get; set; }

        [Option(Templates.InterruptPositive, "Enable Interrupt detection will trigger on positive edges", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) InterruptPositiveEdge { get; set; }

        [Option(null, "USB Remote Wake-Up Capability", CommandOptionType.SingleValue)]
        public (bool HasValue, UsbRemoteWake Value) RemoteWake { get; set; }

        [Option(Templates.SelfPowered, "USB Self powered Capability", CommandOptionType.SingleValue)]
        public (bool HasValue, UsbSelfPowered Value) SelfPowered { get; set; }

        [Range(0, 500)]
        [Option(Templates.PowerRequest, "USB requested number of mA", CommandOptionType.SingleValue)]
        public (bool HasValue, int Value) PowerRequestMa { get; set; }

        [Option(Templates.NewPid,"The new USB PID value", CommandOptionType.SingleValue)]
        public (bool HasValue, ushort Value) NewPid { get; set; }

        [Option(Templates.NewVid, "The new USB VID value", CommandOptionType.SingleValue)]
        public (bool HasValue, ushort Value) NewVid { get; set; }

        [Option(Templates.Password, "", CommandOptionType.SingleValue)]
        public (bool HasValue, string Value) Password { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                bool modified = false;

                device.ReadChipSettings();

                if (AdcRef.HasValue)
                {
                    device.ChipSettings.AdcRefOption = AdcRef.Value;
                    modified = true;
                }

                if (AdcVrmRef.HasValue)
                {
                    device.ChipSettings.AdcRefVrm = AdcVrmRef.Value;
                    modified |= true;
                }

                if (CdcSerialNumberEnable.HasValue)
                {
                    device.ChipSettings.CdcSerialNumberEnable = CdcSerialNumberEnable.Value;
                    modified |= true;
                }

                if (ChipSecurity.HasValue)
                {
                    device.ChipSettings.ChipSecurity = ChipSecurity.Value;
                    modified |= true;
                }

                if (ClockDivider.HasValue)
                {
                    device.ChipSettings.ClockDivider = ClockDivider.Value;
                    modified |= true;
                }

                if (DutyCycle.HasValue)
                {
                    device.ChipSettings.ClockDutyCycle = DutyCycle.Value;
                    modified |= true;
                }

                if (DacOutput.HasValue)
                {
                    device.ChipSettings.DacOutput = DacOutput.Value;
                    modified |= true;
                }

                if (DacRef.HasValue)
                {
                    device.ChipSettings.DacRefOption = DacRef.Value;
                    modified |= true;
                }

                if (DacRefVrm.HasValue)
                {
                    device.ChipSettings.DacRefVrm = DacRefVrm.Value;
                    modified |= true;
                }

                if (InterruptNegativeEdge.HasValue)
                {
                    device.ChipSettings.InterruptNegativeEdge = InterruptNegativeEdge.Value;
                    modified |= true;
                }

                if (InterruptPositiveEdge.HasValue)
                {
                    device.ChipSettings.InterruptPositiveEdge = InterruptPositiveEdge.Value;
                    modified |= true;
                }

                if (PowerRequestMa.HasValue)
                {
                    device.ChipSettings.PowerRequestMa = PowerRequestMa.Value;
                    modified |= true;
                }

                if (NewPid.HasValue)
                {
                    device.ChipSettings.Pid = NewPid.Value;
                    modified |= true;
                }

                if (NewVid.HasValue)
                {
                    device.ChipSettings.Vid = NewVid.Value;
                    modified |= true;
                }

                if (RemoteWake.HasValue)
                {
                    device.ChipSettings.RemoteWake = RemoteWake.Value;
                    modified |= true;
                }

                if (SelfPowered.HasValue)
                {
                    device.ChipSettings.SelfPowered = SelfPowered.Value;
                    modified |= true;
                }

                if (modified)
                {

                    Password password = MCP2221IO.Settings.Password.DefaultPassword;

                    if(Password.HasValue)
                    {
                        password = new Password(Password.Value);
                    }
             
                    device.WriteChipSettings(password);

                    console.WriteLine("CHIP settings updated");
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
