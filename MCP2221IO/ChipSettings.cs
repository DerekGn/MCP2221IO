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

namespace MCP2221IO
{
    public class ChipSettings
    {
        /// <summary>
        /// The current clock out divider value
        /// </summary>
        public ClockOutDivider ClockDivider { get; private set; }

        /// <summary>
        /// DAC Reference voltage option
        /// </summary>
        public DacRefVoltage DacRefVoltage { get; private set; }

        /// <summary>
        /// DAC reference option
        /// </summary>
        public DacRefOption DacRefOption { get; private set; }

        /// <summary>
        /// Initial DAC Output Value
        /// </summary>
        public byte DacOutput { get; private set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// negative edge occurs.
        /// </summary>
        public bool InterruptNegativeEdge { get; private set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// positive edge occurs.
        /// </summary>
        public bool InterruptPositiveEdge { get; private set; }

        /// <summary>
        /// ADC Reference voltage option
        /// </summary>
        public AdcRefVoltage AdcRefVoltage { get; private set; }

        /// <summary>
        /// ADC reference option
        /// </summary>
        public AdcRefOption AdcRefOption { get; private set; }

        /// <summary>
        /// The Usb vendor id
        /// </summary>
        public ushort Vid { get; private set; }

        /// <summary>
        /// The Usb product id
        /// </summary>
        public ushort Pid { get; private set; }

        /// <summary>
        /// This value will be used by the MCP2221’s USB 
        /// Configuration Descriptor(power attributes value) 
        /// during the USB enumeration.
        /// </summary>
        public UsbPowerAttributes PowerAttributes { get; private set; }

        /// <summary>
        /// The requested mA value during the USB enumeration
        /// </summary>
        public int PowerRequestMa { get; private set; }
    }
}