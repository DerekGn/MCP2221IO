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

using MCP2221IO.Commands;
using MCP2221IO.Gpio;
using MCP2221IO.Responses;
using MCP2221IO.Usb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCP2221IO
{
    public class Device : IDevice
    {
        private const int MaxBlockSize = 60;

        private string _factorySerialNumber;
        private IUsbDevice _usbDevice;

        public Device(IUsbDevice usbDevice)
        {
            _usbDevice = usbDevice ?? throw new ArgumentNullException(nameof(usbDevice));
        }

        // <inheritdoc/>
        public DeviceStatus Status => ExecuteCommand<StatusSetParametersResponse>(new StatusSetParametersCommand()).DeviceStatus;
        // <inheritdoc/>
        public ChipSettings ChipSettings => ExecuteCommand<ChipSettingsResponse>(new ReadChipSettingsCommand()).ChipSettings;
        // <inheritdoc/>
        public GpioPorts GpioPorts => ExecuteCommand<GpioPortsResponse>(new ReadGpioPortsCommand()).GpioPorts;
        // <inheritdoc/>
        public string UsbManufacturerDescriptor
        { 
            get => ExecuteCommand<UsbManufacturerDescriptorResponse>(new ReadUsbManufacturerDescriptorCommand()).Value;
            set => ExecuteCommand<WriteFlashDataResponse>(new WriteUsbManufacturerDescriptorCommand(value));
        }
        // <inheritdoc/>
        public string UsbProductDescriptor
        { 
            get => ExecuteCommand<UsbProductDescriptorResponse>(new ReadUsbProductDescriptorCommand()).Value;
            set => ExecuteCommand<WriteFlashDataResponse>(new WriteUsbProductDescriptorCommand(value));
        }
        // <inheritdoc/>
        public string UsbSerialNumberDescriptor
        {
            get => ExecuteCommand<UsbSerialNumberDescriptorResponse>(new ReadUsbSerialNumberDescriptorCommand()).Value;
            set => ExecuteCommand<WriteFlashDataResponse>(new WriteUsbSerialNumberCommand(value));
        }
        // <inheritdoc/>
        public string FactorySerialNumber => GetFactorySerialNumber();
        // <inheritdoc/>
        public void UnlockFlash(ulong password)
        {
            ExecuteCommand<UnlockFlashResponse>(new UnlockFlashCommand(password));
        }
        // <inheritdoc/>
        public void I2CWriteData(byte address, IList<byte> data)
        {
            I2CWriteData<I2CWriteDataResponse>(CommandCodes.WriteI2CData, address, data);
        }

        // <inheritdoc/>
        public void I2CWriteDataRepeatStart(byte address, IList<byte> data)
        {
            I2CWriteData<I2CWriteDataRepeatStartResponse>(CommandCodes.WriteI2CDataRepeatStart, address, data);
        }
        // <inheritdoc/>
        public void I2CWriteDataNoStop(byte address, IList<byte> data)
        {
            I2CWriteData<I2CWriteDataNoStopResponse>(CommandCodes.WriteI2CDataNoStop, address, data);
        }
        // <inheritdoc/>
        public IList<byte> I2CReadData(byte address, ushort length)
        {
            throw new NotImplementedException();
        }
        // <inheritdoc/>
        public IList<byte> I2CReadDataRepeatedStart(byte address, ushort length)
        {
            throw new NotImplementedException();
        }

        private void I2CWriteData<T>(CommandCodes commandCode, byte address, IList<byte> data) where T : IResponse, new()
        {
            int blockCount = (data.Count + MaxBlockSize - 1) / MaxBlockSize;

            for (int i = 0; i < blockCount; i++)
            {
                int blockSize = Math.Min(MaxBlockSize, data.Count - i);

                ExecuteCommand<T>(new I2CWriteDataCommand(commandCode, address, data.Skip(MaxBlockSize * i).Take(blockSize).ToList()));
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            var memoryStream = new MemoryStream(new byte[64], true);

            command.Serialise(memoryStream);

            _usbDevice.Write(memoryStream);
        }

        private T ExecuteCommand<T>(ICommand command) where T : IResponse, new()
        {
            var outStream = new MemoryStream(new byte[64], true);
            var inStream = new MemoryStream();

            command.Serialise(outStream);

            _usbDevice.WriteRead(outStream, inStream);

            var result = new T();

            result.Deserialise(inStream);

            return result;
        }

        private string GetFactorySerialNumber()
        {
            if (String.IsNullOrWhiteSpace(_factorySerialNumber))
            {
                var response = ExecuteCommand<FactorySerialNumberResponse>(new ReadFactorySerialNumberCommand());

                _factorySerialNumber = response.SerialNumber;
            }

            return _factorySerialNumber;
        }
        #region Dispose
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _usbDevice = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Device()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}
