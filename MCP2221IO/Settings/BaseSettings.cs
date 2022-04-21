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

using System;

namespace MCP2221IO.Settings
{
    public abstract class BaseSettings
    {
        private const int MaxUsbMa = 500;

        internal int _powerRequestMa;

        /// <summary>
        /// The clock duty cycle
        /// </summary>
        public ClockDutyCycle ClockDutyCycle { get; set; }

        /// <summary>
        /// The current clock out divider value
        /// </summary>
        public ClockOutDivider ClockDivider { get; set; }

        /// <summary>
        /// DAC Reference Vrm voltage option
        /// </summary>
        public VrmRef DacRefVrm { get; set; }

        /// <summary>
        /// DAC reference option
        /// </summary>
        public DacRefOption DacRefOption { get; set; }

        /// <summary>
        /// Power up DAC Output Value
        /// </summary>
        public byte DacOutput { get; set; }

        /// <summary>
        /// ADC Reference Vrm voltage option
        /// </summary>
        public VrmRef AdcRefVrm { get; set; }

        /// <summary>
        /// ADC reference option
        /// </summary>
        public AdcRefOption AdcRefOption { get; set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// negative edge occurs.
        /// </summary>
        public bool InterruptNegativeEdge { get; set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// positive edge occurs.
        /// </summary>
        public bool InterruptPositiveEdge { get; set; }

        /// <summary>
        /// The requested mA value during the USB enumeration
        /// </summary>
        public int PowerRequestMa
        {
            get
            {
                return _powerRequestMa * 2;
            }
            protected set
            {
                if(value > MaxUsbMa)
                {
                    throw new ArgumentOutOfRangeException(nameof(PowerRequestMa), $"Value must be less than {MaxUsbMa}");
                }

                _powerRequestMa = value / 2;
            }
        }
    }
}
