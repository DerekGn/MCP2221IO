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

namespace MCP2221IOConsole.Commands
{
    internal static class Templates
    {
        public const string CdcSerialNumberEnable = "-cdc|--cdc-serial-number-enable";
        public const string InterruptNegative = "-in|--interrupt-negative";
        public const string InterruptPositive = "-ip|--interrupt-positive";
        public const string ClearInterrupt = "-ci|--clear-interrupt";
        public const string PowerRequest = "-pr|--power-request";
        public const string SerialNumber = "-s|--serial-number";
        public const string ChipSecurity = "-cs|--chip-security";
        public const string SelfPowered = "-sp|--self-powered";
        public const string DacVrm = "-dv|--dac-vrm-reference";
        public const string AdcVrm = "-av|--adc-vrm-reference";
        public const string AdcRef = "-ar|--adc-reference";
        public const string DacRef = "-dr|--dac-reference";
        public const string DacOutput = "-do|--dac-output";
        public const string NewPid = "-np|--new-pid";
        public const string NewVid = "-nv|--new-vid";
        public const string Pid = "-p|--pid";
        public const string Vid = "-v|--vid";

        public const string UsbProduct = "-up|--usb-product";

        public const string UsbSerialNumber = "-us|--usb-serial-number";

        public const string Password = "-pwd|--password";

        public const string GpDesignation = "-gpd|--gp-designation";
        public const string GpIsInput = "-gpi|--gp-is-input";
        public const string GpValue = "-gpv|--gp-value";

        public const string GpioPorts = "-gpiop|--gpio-ports";
        public const string GpioValue = "-gpiov|--gpio-value";
        public const string GpioIsInput = "-gpioi|--gpio-is-input";

        public const string SramGpValue = "-sgv|--sram-gp-value";
        public const string SramGpIsInput = "-sgi|--sram-gp-is-input";
        public const string SramGpDesignation = "-sgd|--sram-gp-designation";
        public const string UsbManufacturer = "-um|--usb-manufacturer";

        public const string Speed = "-is|--i2c-speed";

        public const string I2cAddress = "-ia| --i2c-address";
        public const string I2cLength = "-il| --i2c-length";
        public const string I2cData = "-id| --i2c-data";

        public const string TenBitAddressing = "-tb|--ten-bit-addressing";

        public const string Command = "-c|--command";
        public const string Count = "-ct|--count";
        public const string Write = "-w|--write";
        public const string Data = "-d|--data";
        public const string Pec = "-pe|--pec";
    }
}
