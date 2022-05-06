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
        public void CancelI2cBusTransfer()
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    Status = ExecuteCommand<StatusSetParametersResponse>(new CancelI2cBusTransferCommand()).DeviceStatus;
                });
        }

        // <inheritdoc/>
        public void WriteI2cBusSpeed(int speed)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    Status = ExecuteCommand<StatusSetParametersResponse>(new UpdateI2cBusSpeedCommand(speed)).DeviceStatus;

                    if (Status.SpeedStatus != I2cSpeedStatus.Set)
                    {
                        CancelI2cBusTransfer();
                    }

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
        public void I2cWriteData(I2cAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2cWriteData<I2cWriteDataResponse>(CommandCodes.WriteI2cData, address, data);
                });
        }

        // <inheritdoc/>
        public void I2cWriteDataRepeatStart(I2cAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2cWriteData<I2cWriteDataRepeatStartResponse>(CommandCodes.WriteI2cDataRepeatedStart, address, data);
                });
        }

        // <inheritdoc/>
        public void I2cWriteDataNoStop(I2cAddress address, IList<byte> data)
        {
            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    I2cWriteData<I2cWriteDataNoStopResponse>(CommandCodes.WriteI2cDataNoStop, address, data);
                });
        }

        // <inheritdoc/>
        public IList<byte> I2cReadData(I2cAddress address, ushort length)
        {
            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return I2cReadData<I2cReadDataResponse>(CommandCodes.ReadI2cData, address, length);
                });
        }

        // <inheritdoc/>
        public IList<byte> I2cReadDataRepeatedStart(I2cAddress address, ushort length)
        {
            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    return I2cReadData<I2cReadDataRepeatedStarteResponse>(CommandCodes.ReadI2cDataRepeatedStart, address, length);
                });
        }

        // <inheritdoc/>
        public IList<I2cAddress> I2cScanBus(bool useTenBitAddressing)
        {
            List<I2cAddress> result = new List<I2cAddress>();

            uint upperAddress = useTenBitAddressing ? I2cAddress.TenBitRangeUpper : I2cAddress.SevenBitRangeUpper;

            _logger.LogDebug($"Setting I2C Speed: [0x{_speed:X}] [{_speed}]");

            WriteI2cBusSpeed(_speed);

            for (uint i = I2cAddress.SevenBitRangeLower + 1; i < upperAddress; i++)
            {
                I2cAddress address = new I2cAddress(i, useTenBitAddressing ? I2cAddressSize.TenBit : I2cAddressSize.SevenBit);

                try
                {
                    _logger.LogDebug($"Probing Device Address [0x{address.Value:X}]");

                    var response = I2cReadData(address, 1);

                    if (response.Count > 0)
                    {
                        _logger.LogDebug($"Read [{response.Count}] byte from Device Address [0x{address.Value:X}]");
                    }

                    result.Add(address);
                }
                catch (I2cOperationException ex)
                {
                    _logger.LogWarning(ex, $"Device Address [0x{address.Value:X2}] did not respond");

                    CancelI2cBusTransfer();
                }
            }

            return result;
        }

        private IList<byte> I2cReadData<T>(CommandCodes commandCode, I2cAddress address, ushort length) where T : IResponse, new()
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (length > IDevice.MaxI2cLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, $"Must be less than  0x{IDevice.MaxI2cLength:X4}");
            }

            return HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    List<byte> result = new List<byte>();

                    var response = ExecuteCommand<T>(new I2cReadDataCommand(commandCode, address, length), false);

                    if (response.ExecutionResult != 0)
                    {
                        throw new I2cOperationException(response.ExecutionResult, $"{nameof(I2cReadData)} The read of i2c data failed with execution result code [0x{response.ExecutionResult:x}]");
                    }

                    while (result.Count < length || length == 0)
                    {
                        var getResponse = ExecuteCommand<GetI2cDataResponse>(new GetI2cDataCommand(), false);

                        if (getResponse.ExecutionResult != 0)
                        {
                            throw new I2cOperationException(getResponse.ExecutionResult, $"{nameof(I2cReadData)} The read of i2c data failed with execution result code [0x{getResponse.ExecutionResult:x}]");
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

        private void I2cWriteData<T>(CommandCodes commandCode, I2cAddress address, IList<byte> data) where T : IResponse, new()
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count > IDevice.MaxI2cLength)
            {
                throw new ArgumentOutOfRangeException(nameof(data), data, $"Must be less than  0x{IDevice.MaxI2cLength:X4}");
            }

            HandleOperationExecution(
                nameof(Device),
                () =>
                {
                    //int blockCount = (data.Count + MaxBlockSize - 1) / MaxBlockSize;

                    //for (int i = 0; i < blockCount; i++)
                    //{
                    //    int blockSize = Math.Min(MaxBlockSize, Math.Abs(data.Count - (i * MaxBlockSize)));

                    //    ExecuteCommand<T>(new I2cWriteDataCommand(commandCode, address, data.Skip(MaxBlockSize * i).Take(blockSize).ToList()));
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
