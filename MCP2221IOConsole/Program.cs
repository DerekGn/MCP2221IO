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

using McMaster.Extensions.CommandLineUtils;
using MCP2221IOConsole.Commands;
using MCP2221IOConsole.Commands.Flash;
using MCP2221IOConsole.Commands.Gpio;
using MCP2221IOConsole.Commands.I2c;
using MCP2221IOConsole.Commands.SmBus;
using MCP2221IOConsole.Commands.Sram;
using MCP2221IOConsole.Commands.Status;
using MCP2221IOConsole.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace MCP2221IOConsole
{
    [Command(Description = "A console application for accessing a MCP2221 device")]
    [Subcommand(typeof(FlashCommand))]
    [Subcommand(typeof(GpioCommand))]
    [Subcommand(typeof(I2cCommand))]
    [Subcommand(typeof(ResetCommand))]
    [Subcommand(typeof(SramCommand))]
    [Subcommand(typeof(SmBusCommand))]
    [Subcommand(typeof(StatusCommand))]
    [Subcommand(typeof(UnlockCommand))]
    class Program
    {
        private static IConfiguration _configuration;

        public static int Main(string[] args)
        {
            int result = -1;

            try
            {
                _configuration = SetupConfiguration(args);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(_configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

                var serviceProvider = BuildServiceProvider();

                var app = new CommandLineApplication<Program>();

                app.ValueParsers.AddOrReplace(new CustomUShortValueParser());
                app.ValueParsers.AddOrReplace(new CustomUIntValueParser());
                app.ValueParsers.AddOrReplace(new ByteListValueParser());

                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(serviceProvider);

                result = app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred");
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

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify a subcommand.");
            app.ShowHelp();
            return 1;
        }
    }
}