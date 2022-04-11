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

using CommandLine;
using HidSharp;
using MCP2221IO;
using MCP2221IO.Usb;
using MCP2221IOConsole.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace MCP2221IOConsole
{
    class Program
    {
        private static ServiceProvider _serviceProvider;
        private static IConfiguration _configuration;
        
        static int Main(string[] args)
        {
            _configuration = SetupConfiguration(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            _serviceProvider = BuildServiceProvider();

            return Parser.Default.ParseArguments<ChipCommands, GpCommands>(args)
                .MapResult(
                (ChipCommands command) => ExecuteChipCommands(command),
                (GpCommands command) => ExecuteGpCommands(command),
                errors => 1);
        }

        private static int ExecuteChipCommands(ChipCommands command)
        {
            return ExecuteCommand(command, (device) =>
            {
                if(command.Read)
                {
                    device.ReadChipSettings();

                    Console.WriteLine(device.ChipSettings.ToString());
                }

                return 1;
            });
        }

        private static int ExecuteGpCommands(GpCommands command)
        {
            return ExecuteCommand(command, (device) =>
            {

                return 1;
            });
        }

        private static int ExecuteCommand(BaseCommand baseCommand, Func<IDevice, int> action)
        {
            var logger = _serviceProvider.GetService<ILogger<Program>>();
            int result = -1;

            try
            {
                var hidDevice = DeviceList.Local.GetHidDeviceOrNull(baseCommand.Vid, baseCommand.Pid);

                if (hidDevice != null)
                {
                    using HidSharpHidDevice hidSharpHidDevice = new HidSharpHidDevice(_serviceProvider.GetService<ILogger<IHidDevice>>(), hidDevice);
                    using MCP2221IO.Device device = new MCP2221IO.Device(_serviceProvider.GetService<ILogger<IDevice>>(), hidSharpHidDevice);

                    device.Open();

                    result = action(device);
                }
                else
                {
                    logger.LogWarning($"Unable to find HID device VID: [0x{baseCommand.Vid:x}] PID: [0x{baseCommand.Vid:x}] SerialNumber: [{baseCommand.SerialNumber}]");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred");
            }

            return result;
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .AddSingleton(_configuration)
                .AddLogging();

            return serviceCollection.BuildServiceProvider();
        }

        private static IConfiguration SetupConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }
    }
}
