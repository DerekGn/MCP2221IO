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

using System.Text;

namespace PModAqs.Sensor
{
    internal class Mode
    {
        public Mode(byte mode)
        {
            Raw = mode;
            DriveMode = (DriveMode)(mode & 0x70);
            DataReady = (mode & 0x08) == 0x08;
            Threshold = (mode & 0x04) == 0x04;
        }

        public byte Raw { get; private set; }

        public DriveMode DriveMode { get; private set; }

        public bool DataReady { get; private set; }

        public bool Threshold { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Raw)}:\t\t[0x{Raw:X2}]");
            stringBuilder.AppendLine($"{nameof(DriveMode)}:\t{DriveMode}");
            stringBuilder.AppendLine($"{nameof(DataReady)}:\t{DataReady}");
            stringBuilder.Append($"{nameof(Threshold)}:\t{Threshold}");

            return stringBuilder.ToString();
        }
    }
}
