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

using System.IO;

namespace MCP2221IO.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        /// Read an <see cref="UInt16"/> from the <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <returns>The <see cref="ushort"/> value</returns>
        public static ushort ReadUShort(this Stream stream)
        {
            ushort value = (byte)stream.ReadByte();
            value += (ushort)(stream.ReadByte() << 8);

            return value;
        }
        /// <summary>
        /// Write a <see cref="UInt16"/> value to the <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The stream to write</param>
        /// <param name="value">The value to write</param>
        public static void WriteUShort(this Stream stream, ushort value)
        {
            stream.WriteByte((byte)(value & 0x00FF));
            stream.WriteByte((byte)((value & 0xFF00) >> 8));
        }
    }
}
