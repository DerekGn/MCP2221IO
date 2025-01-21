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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MCP2221IO.Settings
{
    public class Password
    {
        public Password(string value)
        {
            if(!Parse(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public Password(IList<byte> bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);

            if (bytes.Count > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Must be 8 bytes long");
            }

            Bytes = bytes.ToList().AsReadOnly();
            Value = BitConverter.ToString(bytes.ToArray()).Replace("-", "");
        }

        public string? Value { get; private set; }

        public IReadOnlyList<byte>? Bytes { get; private set; }

        public override string ToString()
        {
            return Value!;
        }

        /// <summary>
        /// The default <see cref="Password"/>
        /// </summary>
        public static Password DefaultPassword => new Password(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

        private bool Parse(string password)
        {
            bool result;

            password = password.Replace("0x", string.Empty);

            if (!ulong.TryParse(password, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong value))
            {
                result = ulong.TryParse(password, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            }
            else
            {
                result = true;
            }

            Bytes = BitConverter.GetBytes(value);
            Value = password;

            return result;
        }
    }
}
