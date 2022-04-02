﻿/*
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

namespace MCP2221IO.Gpio
{
    /// <summary>
    ///  Gpio1 designation 
    /// </summary>
    public enum Gpio1Designation
    {
        /// <summary>
        /// GPIO operation.
        /// </summary>
        GpioOperation = 0,
        /// <summary>
        /// Dedicated function operation (clock output).
        /// </summary>
        DedicatedFunction = 1,
        /// <summary>
        /// Alternate Function 0 (ADC1).
        /// </summary>
        AlternateFunction0 = 2,
        /// <summary>
        /// Alternate Function 1 (LED_UTx).
        /// </summary>
        AlternateFunction1 = 3,
        /// <summary>
        /// Alternate Function 2 (Interrupt detection).
        /// </summary>
        AlternateFunction2 = 4

    }
}