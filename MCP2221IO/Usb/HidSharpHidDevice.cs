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

using HidSharp;
using Microsoft.Extensions.Logging;
using System;

namespace MCP2221IO.Usb
{
    public class HidSharpHidDevice : IHidDevice
    {
        private readonly ILogger<IHidDevice> _logger;
        private HidDevice _hidDevice;
        private HidStream _hidStream;
        private bool disposedValue;

        public HidSharpHidDevice(ILogger<IHidDevice> logger, HidDevice hidDevice)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hidDevice = hidDevice ?? throw new ArgumentNullException(nameof(hidDevice));
        }

        public void Open()
        {
            _hidStream = _hidDevice.Open();
        }

        public void Write(byte[] outBytes)
        {
            _hidStream.Write(outBytes);
        }

        public byte[] WriteRead(byte[] outBytes)
        {
            _logger.LogDebug($"Output HID Packet: [{BitConverter.ToString(outBytes).Replace("-", ",0x")}]");

            _hidStream.Write(outBytes);

            var inBytes = _hidStream.Read();

            _logger.LogDebug($"Input HID Packet: [{BitConverter.ToString(inBytes).Replace("-", ",0x")}]");

            return inBytes;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_hidStream != null)
                    {
                        _hidStream.Dispose();
                    }

                    _hidDevice = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
