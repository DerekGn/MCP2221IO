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
using MCP2221IO.Exceptions;
using MCP2221IO.Gpio;
using MCP2221IO.Responses;
using MCP2221IO.Settings;
using MCP2221IO.Usb;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MCP2221IO
{
    /// <summary>
    /// A MCP2221 device
    /// </summary>
    public class Device : IDevice
    {
        internal const ushort MaxI2CLength = 0xFFFF;
        internal bool _gpioPortsRead = false;

        private readonly ILogger<IDevice> _logger;
        private string _factorySerialNumber;
        private IHidDevice _hidDevice;
        private int _speed = 100000;

        public Device(ILogger<IDevice> logger, IHidDevice hidDevice)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hidDevice = hidDevice ?? throw new ArgumentNullException(nameof(hidDevice));
        }

        // <inheritdoc/>
        public DeviceStatus Status { get; internal set; }

        // <inheritdoc/>
        public ChipSettings ChipSettings { get; internal set; }

        // <inheritdoc/>
        public GpSettings GpSettings { get; internal set; }

        // <inheritdoc/>
        public SramSettings SramSettings { get; internal set; }

        // <inheritdoc/>
        public GpioPort GpioPort0 { get; internal set; }

        // <inheritdoc/>
        public GpioPort GpioPort1 { get; internal set; }

        // <inheritdoc/>
        public GpioPort GpioPort2 { get; internal set; }

        // <inheritdoc/>
        public GpioPort GpioPort3 { get; internal set; }

        // <inheritdoc/>
        public string UsbManufacturerDescriptor
        {
            get => HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return ExecuteCommand<ReadFlashStringResponse>(new ReadUsbManufacturerDescriptorCommand()).Value;
                });
            set =>
                HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ExecuteCommand<WriteFlashDataResponse>(new WriteUsbManufacturerDescriptorCommand(value));
                });
        }

        // <inheritdoc/>
        public string UsbProductDescriptor
        {
            get => HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return ExecuteCommand<ReadFlashStringResponse>(new ReadUsbProductDescriptorCommand()).Value;
                });
            set => HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ExecuteCommand<WriteFlashDataResponse>(new WriteUsbProductDescriptorCommand(value));
                });
        }

        // <inheritdoc/>
        public string UsbSerialNumberDescriptor
        {
            get => HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return ExecuteCommand<ReadFlashStringResponse>(new ReadUsbSerialNumberDescriptorCommand()).Value;
                });
            set => HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ExecuteCommand<WriteFlashDataResponse>(new WriteUsbSerialNumberCommand(value));
                });
        }

        // <inheritdoc/>
        public string FactorySerialNumber => GetFactorySerialNumber();

        // <inheritdoc/>
        public void UnlockFlash(Password password)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ExecuteCommand<UnlockFlashResponse>(new UnlockFlashCommand(password));
                });
        }

        // <inheritdoc/>
        public void ReadDeviceStatus()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    Status = ExecuteCommand<StatusSetParametersResponse>(new ReadStatusSetParametersCommand()).DeviceStatus;
                });
        }

        // <inheritdoc/>
        public void ReadChipSettings()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ChipSettings = ExecuteCommand<ReadChipSettingsResponse>(new ReadChipSettingsCommand()).ChipSettings;
                });
        }

        // <inheritdoc/>
        public void ReadGpSettings()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    GpSettings = ExecuteCommand<ReadGpSettingsResponse>(new ReadGpSettingsCommand()).GpSettings;
                });
        }

        // <inheritdoc/>
        public void ReadSramSettings()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    SramSettings = ExecuteCommand<ReadSramSettingsResponse>(new ReadSramSettingsCommand()).SramSettings;
                });
        }

        // <inheritdoc/>
        public void ReadGpioPorts()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    var response = ExecuteCommand<ReadGpioPortsResponse>(new ReadGpioPortsCommand());

                    GpioPort0 = response.GpioPort0;
                    GpioPort1 = response.GpioPort1;
                    GpioPort2 = response.GpioPort2;
                    GpioPort3 = response.GpioPort3;

                    _gpioPortsRead = true;
                });
        }

        // <inheritdoc/>
        public void WriteChipSettings(Password password)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    if (ChipSettings == null)
                    {
                        throw new ReadRequiredException($"{nameof(ChipSettings)} must be read from the device");
                    }

                    ExecuteCommand<WriteFlashDataResponse>(new WriteChipSettingsCommand(ChipSettings, password));
                });
        }

        // <inheritdoc/>
        public void WriteGpSettings()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    if (GpSettings == null)
                    {
                        throw new ReadRequiredException($"{nameof(GpSettings)} must be read from the device");
                    }

                    ExecuteCommand<WriteFlashDataResponse>(new WriteGpSettingsCommand(GpSettings));
                });
        }

        // <inheritdoc/>
        public void WriteSramSettings(bool clearInterrupts)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    if (SramSettings == null)
                    {
                        throw new ReadRequiredException($"{nameof(SramSettings)} must be read from the device");
                    }

                    ExecuteCommand<WriteSramSettingsResponse>(new WriteSramSettingsCommand(SramSettings, clearInterrupts));
                });
        }

        // <inheritdoc/>
        public void WriteGpioPorts()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    if (!_gpioPortsRead)
                    {
                        throw new ReadRequiredException($"Gpio ports must be read from the device");
                    }

                    ExecuteCommand<WriteGpioPortsResponse>(new WriteGpioPortsCommand(GpioPort0, GpioPort1, GpioPort2, GpioPort3));
                });
        }

        // <inheritdoc/>
        public void Reset()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    ExecuteCommand(new ResetCommand());
                });
        }

        // <inheritdoc/>
        public void CancelI2CBusTransfer()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    Status = ExecuteCommand<StatusSetParametersResponse>(new CancelI2CBusTransferCommand()).DeviceStatus;
                });
        }

        // <inheritdoc/>
        public void WriteI2CBusSpeed(int speed)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    Status = ExecuteCommand<StatusSetParametersResponse>(new UpdateI2CBusSpeedCommand(speed)).DeviceStatus;

                    _speed = speed;
                });
        }

        // <inheritdoc/>
        public void Open()
        {
            HandleOperationExecution(nameof(Device), () => _hidDevice.Open());
        }

        // <inheritdoc/>
        public void Close()
        {
            Dispose();
        }

        // <inheritdoc/>
        public void I2CWriteData(I2CAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2CWriteData<I2CWriteDataResponse>(CommandCodes.WriteI2CData, address, data);
                });
        }

        // <inheritdoc/>
        public void I2CWriteDataRepeatStart(I2CAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2CWriteData<I2CWriteDataRepeatStartResponse>(CommandCodes.WriteI2CDataRepeatedStart, address, data);
                });
        }

        // <inheritdoc/>
        public void I2CWriteDataNoStop(I2CAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2CWriteData<I2CWriteDataNoStopResponse>(CommandCodes.WriteI2CDataNoStop, address, data);
                });
        }

        // <inheritdoc/>
        public IList<byte> I2CReadData(I2CAddress address, ushort length)
        {
            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return I2CReadData<I2CReadDataResponse>(CommandCodes.ReadI2CData, address, length);
                });
        }

        // <inheritdoc/>
        public IList<byte> I2CReadDataRepeatedStart(I2CAddress address, ushort length)
        {
            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return I2CReadData<I2CReadDataRepeatedStarteResponse>(CommandCodes.ReadI2CDataRepeatedStart, address, length);
                });
        }

        // <inheritdoc/>
        public IList<I2CAddress> I2CScanBus(bool useTenBitAddressing)
        {
            List<I2CAddress> result = new List<I2CAddress>();

            uint upperAddress = useTenBitAddressing ? I2CAddress.TenBitRangeUpper : I2CAddress.SevenBitRangeUpper;

            _logger.LogDebug($"Setting I2C Speed: [0x{_speed:X}] [{_speed}]");

            WriteI2CBusSpeed(_speed);

            I2CAddress address = new I2CAddress(0x5B, useTenBitAddressing ? I2CAddressSize.TenBit : I2CAddressSize.SevenBit);

            var response = I2CReadData(address, 1);

            //for (uint i = I2CAddress.SevenBitRangeLower + 1; i < upperAddress; i++)
            //{
            //    I2CAddress address = new I2CAddress(i, useTenBitAddressing ? I2CAddressSize.TenBit : I2CAddressSize.SevenBit);

            //    try
            //    {
            //        var response = I2CReadData(address, 1);
            //    }
            //    catch (I2cOperationException ex)
            //    {
            //        _logger.LogWarning(ex, $"Device Address [0x{address.Value:X2}] did not respond");
            //    }
            //}

            return result;
        }

        private IList<byte> I2CReadData<T>(CommandCodes commandCode, I2CAddress address, ushort length) where T : IResponse, new()
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (length > MaxI2CLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, $"Must be less than  0x{MaxI2CLength:X4}");
            }

            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    List<byte> result = new List<byte>();

                    var response = ExecuteCommand<T>(new I2CReadDataCommand(commandCode, address, length), false);

                    if (response.ExecutionResult != 0)
                    {
                        throw new I2cOperationException(response.ExecutionResult, $"{nameof(I2CReadData)} The read of i2c data failed with execution result code [0x{response.ExecutionResult:x}]");
                    }

                    while (result.Count < length || length == 0)
                    {
                        var getResponse = ExecuteCommand<GetI2CDataResponse>(new GetI2CDataCommand());

                        if (response.ExecutionResult != 0)
                        {
                            throw new I2cOperationException(response.ExecutionResult, $"{nameof(I2CReadData)} The read of i2c data failed with execution result code [0x{response.ExecutionResult:x}]");
                        }

                        if (getResponse.Data.Count > 0)
                        {
                            result.AddRange(getResponse.Data);
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    return result;
                });
        }

        private void I2CWriteData<T>(CommandCodes commandCode, I2CAddress address, IList<byte> data) where T : IResponse, new()
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count > MaxI2CLength)
            {
                throw new ArgumentOutOfRangeException(nameof(data), data, $"Must be less than  0x{MaxI2CLength:X4}");
            }

            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    //int blockCount = (data.Count + MaxBlockSize - 1) / MaxBlockSize;

                    //for (int i = 0; i < blockCount; i++)
                    //{
                    //    int blockSize = Math.Min(MaxBlockSize, Math.Abs(data.Count - (i * MaxBlockSize)));

                    //    ExecuteCommand<T>(new I2CWriteDataCommand(commandCode, address, data.Skip(MaxBlockSize * i).Take(blockSize).ToList()));
                    //}
                });
        }

        private string GetFactorySerialNumber()
        {
            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    if (String.IsNullOrWhiteSpace(_factorySerialNumber))
                    {
                        var response = ExecuteCommand<FactorySerialNumberResponse>(new ReadFactorySerialNumberCommand());

                        _factorySerialNumber = response.SerialNumber;
                    }

                    return _factorySerialNumber;
                });
        }

        private void ExecuteCommand(ICommand command)
        {
            var memoryStream = new MemoryStream(new byte[64], true);

            command.Serialize(memoryStream);

            _hidDevice.Write(memoryStream.ToArray());
        }

        private T ExecuteCommand<T>(ICommand command, bool checkResult = true) where T : IResponse, new()
        {
            var outStream = new MemoryStream(new byte[64], true);
            var inStream = new MemoryStream();

            command.Serialize(outStream);

            inStream.Write(_hidDevice.WriteRead(outStream.ToArray()));

            var result = new T();

            result.Deserialize(inStream);

            if (checkResult && result.ExecutionResult != 0)
            {
                throw new CommandExecutionFailedException($"Unexpected command execution status Expected: [0x00] Actual [0x{result.ExecutionResult:x}]");
            }

            return result;
        }

        [DebuggerStepThrough]
        private void HandleOperationExecution(string className, Action operation, [CallerMemberName] string memberName = "")
        {
            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                operation();
                sw.Stop();

                _logger.LogDebug($"Executed [{className}].[{memberName}] in [{sw.Elapsed.TotalMilliseconds}] ms");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred executing [{className}].[{memberName}] Reason: [{ex.Message}]");
                
                throw;
            }
        }

        [DebuggerStepThrough]
        private T HandleOperationExecution<T>(string className, Func<T> operation, [CallerMemberName] string memberName = "")
        {
            Stopwatch sw = new Stopwatch();
            T result = default(T);

            try
            {
                sw.Start();
                result = operation();
                sw.Stop();

                _logger.LogDebug($"Executed [{className}].[{memberName}] in [{sw.Elapsed.TotalMilliseconds}] ms");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred executing [{className}].[{memberName}] Reason: [{ex.Message}]");
                
                throw;
            }

            return result;
        }

        #region Dispose
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _hidDevice = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}
