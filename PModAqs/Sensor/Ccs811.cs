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

using MCP2221IO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PModAqs.Sensor
{
    internal class Ccs811 : ICcs811
    {
        private const int Resistor = 100000;

        private readonly ILogger<ICcs811> _logger;
        private readonly I2cAddress _i2cAddress;
        private IDevice _device;

        public Ccs811(ILogger<ICcs811> logger, I2cAddress i2cAddress, IDevice device)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _i2cAddress = i2cAddress ?? throw new ArgumentNullException(nameof(i2cAddress));
            _device = device ?? throw new ArgumentNullException(nameof(device));
        }

        // <inheritdoc/>
        public VersionInfo GetVersion()
        {
            List<byte> versionData = new List<byte>();

            versionData.AddRange(ReadRegister(Registers.HwId, 1));

            versionData.AddRange(ReadRegister(Registers.HwVersion, 1));

            versionData.AddRange(ReadRegister(Registers.FirmwareBootVersion, 2));

            versionData.AddRange(ReadRegister(Registers.FirmwareAppVersion, 2));

            return new VersionInfo(versionData);
        }

        // <inheritdoc/>
        public Mode GetMode()
        {
            return new Mode(ReadRegister(Registers.Mode, 1).First());
        }

        // <inheritdoc/>
        public RawData GetRawData()
        {
            return new RawData(ReadRegister(Registers.RawData, 2));
        }

        // <inheritdoc/>
        public SensorData GetData()
        {
            return new SensorData(ReadRegister(Registers.ResultData, 8).ToList());
        }

        // <inheritdoc/>
        public Status GetStatus()
        {
            return (Status)ReadRegister(Registers.Status, 1).First();
        }

        // <inheritdoc/>
        public Error GetError()
        {
            return (Error)ReadRegister(Registers.ErrorId, 1).First();
        }

        // <inheritdoc/>
        public void StartApplication()
        {
            _device.I2cWriteData(_i2cAddress, new List<byte>() { (byte)Registers.ApplicationStart } );
        }

        // <inheritdoc/>
        public void Reset()
        {
            _device.I2cWriteData(_i2cAddress, new List<byte>() { 0x11, 0xE5, 0x72, 0x8A });
        }

        // <inheritdoc/>
        public double GetTemperature()
        {
            throw new NotImplementedException();
        }

        // <inheritdoc/>
        public void SetMode(DriveMode mode)
        {
            _device.I2cWriteDataNoStop(_i2cAddress, new List<byte>() { (byte)Registers.Mode });
            
            var data = _device.I2cReadDataRepeatedStart(_i2cAddress, 1);

            data[0] |= (byte)mode;

            var writeData = new List<byte>() { (byte)Registers.Mode };
            writeData.AddRange(data);

            _device.I2cWriteData(_i2cAddress, writeData);
        }

        private IList<byte> ReadRegister(Registers register, ushort count)
        {
            _device.I2cWriteDataNoStop(_i2cAddress, new List<byte>() { (byte)register });
            return _device.I2cReadDataRepeatedStart(_i2cAddress, count);
        }

        #region Dispose
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug($"Disposing {nameof(Ccs811)}");

            if (!disposedValue)
            {
                if (disposing)
                {
                    _device = null;
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
        #endregion
    }
}
