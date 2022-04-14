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
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MCP2221IOConsole.Commands
{
    [Command(Description = "Unlock the Device With Password")]
    internal class WriteAccessPasswordCommand : BaseCommand
    {
        public WriteAccessPasswordCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Required]
        [Option(Description = "The 8 byte password value")]
        public string Password { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                int result = 0;

                if (Parse(Password, out var parsed))
                {
                    device.UnlockFlash(parsed);
                    
                    console.WriteLine("Flash unlocked");
                }
                else
                {
                    console.Error.Write($"Value for {nameof(Password)} [{Password}] is invalid");
                    result = -1;
                }

                return result;
            });
        }

        private bool Parse(string password, out ulong value)
        {
            bool result;

            if (password.StartsWith("0x"))
            {
                result = ulong.TryParse(password.Replace("0x", string.Empty), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
            }
            else
            {
                result = ulong.TryParse(password, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            }

            return result;
        }
    }
}
