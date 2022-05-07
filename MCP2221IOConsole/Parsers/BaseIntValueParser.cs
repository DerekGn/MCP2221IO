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

using McMaster.Extensions.CommandLineUtils.Abstractions;
using System;
using System.Globalization;

namespace MCP2221IOConsole.Parsers
{
    internal abstract class BaseIntValueParser<T> : IValueParser
    {
        public Type TargetType => typeof(T);

        public object Parse(string argName, string value, CultureInfo culture)
        {
            return ParseInternal(argName, value, culture, ParseFunction);
        }

        protected abstract (bool, T) ParseFunction(string value, CultureInfo culture, NumberStyles numberStyles);

        private object ParseInternal(string argName, string value, CultureInfo culture,
            Func<string, CultureInfo, NumberStyles, (bool, T)> valueParser)
        {
            (bool, T) parseResult;
            
            if (value.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                var strippedHexValue = value.Replace("0x", string.Empty).Replace("0X", string.Empty);

                parseResult = valueParser(strippedHexValue, culture, NumberStyles.HexNumber);
            }
            else
            {
                parseResult = valueParser(value, culture, NumberStyles.Integer);
            }

            if (!parseResult.Item1)
            {
                throw new FormatException($"Invalid value specified for {argName}. '{value}' is not a valid number.");
            }

            return parseResult.Item2;
        }
    }
}
