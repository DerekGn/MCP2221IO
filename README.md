[![Build Status](https://derekgn.visualstudio.com/GitHub/_apis/build/status/DerekGn.MCP2221IO?branchName=main)](https://derekgn.visualstudio.com/GitHub/_build/latest?definitionId=12&branchName=main)

[![NuGet Badge](https://buildstats.info/nuget/MCP2221IO)](https://www.nuget.org/packages/MCP2221IO/)

# MCP2221IO

A .Net Core library to interact with Usb [MCP2221](https://www.microchip.com/wwwproducts/en/MCP2221)/[MCP2221A](https://www.microchip.com/wwwproducts/en/MCP2221A) devices. For example the [ADM00559](https://www.microchip.com/en-us/development-tool/ADM00559) device.

## Installing MCP2221IO

Install the MCP2221IO package via nuget package manager console:

```ps
Install-Package MCP2221IO
```

Or from the dot net command line console

```cmd
dotnet add package MCP2221IO
```

## Supported Functions

The following is the list of functions supported by the API.

- [x] Status/Set Parameters
  - [x] Cancel I2C Transfer
  - [x] Set I2C Speed
- [x] Read Flash Data
  - [x] Read Chip Settings
  - [x] Read GP Settings
  - [x] Read USB Manufacturer Descriptor String
  - [x] Read USB Product Descriptor String
  - [x] Read USB Serial Number Descriptor String
  - [x] Read Chip Factory Serial Number
- [x] Write Flash Data
  - [x] Write Chip Settings
  - [x] Write GP Settings
  - [x] Write USB Manufacturer Descriptor String
  - [x] Write USB Product Descriptor String
  - [x] Write USB Serial Number Descriptor String
  - [x] Write Chip Factory Serial Number
- [x] Send Access Flash Password
- [x] I2C Operations (7 Bit And 10 Bit address)
  - [x] Scan Bus
  - [x] Write Data
  - [x] Write Data Repeated-Start
  - [x] Write Data No Stop
  - [x] Read Data
  - [x] Read Data Repeated-Start
- [x] Set GPIO (Output And Direction)
  - [x] GPO, GP1, GP2, GP3
- [x] Get GPIO (Output And Direction)
  - [x] GPO, GP1, GP2, GP3
- [x] Set SRAM Settings
  - [x] Clock Output Divider Value
  - [x] DAC Voltage Reference
  - [x] Set DAC Output Value
  - [x] ADC Voltage Reference
  - [x] Set Up the Interrupt Detection Mechanism and Clear the Detection Flag
  - [x] GP0, GP1, GP2, GP3 Settings (Output And Designation)
- [x] Get SRAM Settings
  - [x] CDC Serial Number Enumeration Enable
  - [x] Chip Configuration Security Option
  - [x] Clock Output Divider Value
  - [x] DAC Reference Voltage Option
  - [x] DAC Reference Option
  - [x] Power-up DAC Value
  - [x] Interrupt Detection – Negative Edge
  - [x] Interrupt Detection – Positive Edge
  - [x] ADC Reference Voltage
  - [x] ADC Reference Option
  - [x] USB VID
  - [x] USB PID
  - [x] USB Power Attributes
  - [x] Usb Request Number Of mA
  - [x] Password
- [x] Reset
- [x] SmBus
  - [x] SmBusBlockRead
  - [x] SmBusBlockWrite
  - [x] SmBusQuickCommand
  - [x] SmBusReadByte
  - [x] SmBusReadByteCommand
  - [x] SmBusReadIntCommand
  - [x] SmBusReadLongCommand
  - [x] SmBusReadShortCommand
  - [x] SmBusWriteByte
  - [x] SmBusWriteByteCommand
  - [x] SmBusWriteIntCommand
  - [x] SmBusWriteLongCommand
  - [x] SmBusWriteShortCommand

## Example code

The MCP2221A Api contains a mix of Read/Write properties and Read Before Write properties. The intent is to allow the code consuming the MCP 2221A Api to control the read write IO to the USB device.

### Resolve HID Device Instance

```csharp
var hidDevice = DeviceList.Local.GetHidDeviceOrNull(Vid, Pid, null, Serial);

if (hidDevice != null)
{
    using HidSharpHidDevice hidSharpHidDevice = new HidSharpHidDevice((ILogger<IHidDevice>)_serviceProvider.GetService(typeof(ILogger<IHidDevice>)), hidDevice);
    using MCP2221IO.Device device = new MCP2221IO.Device((ILogger<IDevice>)_serviceProvider.GetService(typeof(ILogger<IDevice>)), hidSharpHidDevice);

    device.Open();

    result = action(device);
}
else
{
    Console.Error.WriteLine($"Unable to find HID device VID: [0x{Vid:X}] PID: [0x{Vid:X}] SerialNumber: [{Serial}]");
}
```

### Read Write Properties

Read write properties can be simply used as any standard property.

```csharp
Console.WriteLine($"Usb Manufacture Descriptor: [{device.UsbManufacturerDescriptor}]");

device.UsbManufacturerDescriptor = "Updated";
```

### Read Before Write Methods

To reduce the IO overhead to and from the USB device multiple settings can be set and then applied in a single operation.

```csharp
// Read the chip settings
device.ReadChipSettings();

// Update  multiple settings
device.ChipSettings.AdcRefOption = AdcRef.Value;

Password password = MCP2221IO.Settings.Password.DefaultPassword;

// Write the CHIP Settings
device.WriteChipSettings(password);

Console.WriteLine("CHIP settings updated");
```

### Synchronous Methods

There a number of synchronous methods that are used to change state of the device or read write data for example I2C and SmBus operations.

```csharp
// Cancel the I2C bus transfer
device.CancelI2cBusTransfer();

// Set the I2C bus speed
device.SetI2cBusSpeed(int speed);
```

## MCP2221 Console Application

A console application is available from nuget that allows command line access to the functions of a connected MCP2221 HID usb device. Type MCP2221Console -?|-h|--help to see the list of commands for the device.

## Installing MCP2221IO.Console

Install the MCP2221IO.Console package via nuget package manager console:

```ps
Install-Package MCP2221IO.Console
```

Or from the dot net command line console

```cmd
dotnet add package MCP2221IO.Console
```

## Example Console Application

The source code contains an example application for the PModAqs sensor which is based on the CCS811 see [here](https://digilent.com/reference/pmod/pmodaqs/start) for further information.

The ADM00559 should be configured for 3.3v operation via the jumper selectable power supply. The ADM00559 should be connected to the PMosAqs device in the following way:

ADM00559 J3 | PMosAqs
---------|----------
 Pin 2 (Vdd) | Pin 6 (Vcc)
 Pin 3 (Gnd) | Pin 5 (Gnd)
 Pin 4 (Sda) | Pin 4 (Sda)
 Pin 5 (Scl) | Pin 3 (Scl)
