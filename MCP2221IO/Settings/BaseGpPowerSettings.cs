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

using System;
using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    /// <summary>
    /// A Gpio setting type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GpSetting<T> where T : Enum
    {
        /// <summary>
        /// The current output value on the Gpio port
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// The Gpio port direction
        /// </summary>
        public bool IsInput { get; set; }

        /// <summary>
        /// The current Gp settings port designation
        /// </summary>
        public T? Designation { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                $"{nameof(Value)}: {Value,5} " +
                $"{nameof(IsInput)}: {IsInput,5} " +
                $"{nameof(Designation)}: {Designation,17} ");

            return stringBuilder.ToString();
        }

        // <inheritdoc/>
        internal void Deserialize(Stream stream)
        {
            int temp = stream.ReadByte();

            Value = (temp & 0x10) == 0x10;
            IsInput = (temp & 0x08) == 0x08;
            Designation = (T)(object)(temp & 0x07);
        }

        internal void Serialize(Stream stream)
        {
            int update = Value ? 0x10 : 0x00;
            update |= IsInput ? 0x08 : 0x00;
            update |= (int)(object)Designation! & 0b111;

            stream.WriteByte((byte)update);
        }
    }
}

