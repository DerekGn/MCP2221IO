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
    /// Internal Voltage Reference VRM Selection
    /// </summary>
    public enum VrmRef
    {
        /// <summary>
        /// Reference voltage is 4.096V (only if VDD is above this voltage)
        /// </summary>
        Vrm4096V = 0b11,
        /// <summary>
        /// Reference voltage is 2.048V
        /// </summary>
        Vrm2048V = 0b10,
        /// <summary>
        /// Reference voltage is 1.024V
        /// </summary>
        Vrm1024V = 0b01,
        /// <summary>
        /// Reference voltage is off
        /// </summary>
        VrmOff = 0
    }
}