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

namespace MCP2221IO.Settings
{
    /// <summary>
    /// Clock-Out Divider Output
    /// </summary>
    public enum ClockOutDivider
    {
        /// <summary>
        /// 375 kHz clock output
        /// </summary>
        Clock375kHz = 0b111,
        /// <summary>
        /// 750 kHz clock output
        /// </summary>
        Clock750kHz = 0b110,
        /// <summary>
        /// 1.5 MHz clock output
        /// </summary>
        Clock1_5MHz = 0b101,
        /// <summary>
        /// 3 MHz clock output
        /// </summary>
        Clock3MHz = 0b100,
        /// <summary>
        /// 6 MHz clock output
        /// </summary>
        Clock6MHz = 0b011,
        /// <summary>
        /// 12 MHz clock output (factory default)
        /// </summary>
        Clock12MHz = 0b010,
        /// <summary>
        /// 24 MHz clock output
        /// </summary>
        Clock24Mhz = 0b001,
        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 0
    }
}