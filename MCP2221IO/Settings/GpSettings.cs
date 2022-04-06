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

using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    /// <summary>
    /// The GP settings read from flash
    /// </summary>
    public class GpSettings
    {
        public Gp0PowerUpSettings Gp0PowerUpSettings { get; private set; }

        public Gp1PowerUpSettings Gp1PowerUpSettings { get; private set; }

        public Gp2PowerUpSettings Gp2PowerUpSettings { get; private set; }

        public Gp3PowerUpSettings Gp3PowerUpSettings { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Gp0PowerUpSettings)}:\r\n{Gp0PowerUpSettings}");
            stringBuilder.AppendLine($"{nameof(Gp1PowerUpSettings)}:\r\n{Gp1PowerUpSettings}");
            stringBuilder.AppendLine($"{nameof(Gp2PowerUpSettings)}:\r\n{Gp2PowerUpSettings}");
            stringBuilder.AppendLine($"{nameof(Gp3PowerUpSettings)}:\r\n{Gp3PowerUpSettings}");

            return stringBuilder.ToString();
        }

        internal void Deserialise(Stream stream)
        {
            Gp0PowerUpSettings = new Gp0PowerUpSettings();
            Gp1PowerUpSettings = new Gp1PowerUpSettings();
            Gp2PowerUpSettings = new Gp2PowerUpSettings();
            Gp3PowerUpSettings = new Gp3PowerUpSettings();

            Gp0PowerUpSettings.Deserialise(stream);
            Gp1PowerUpSettings.Deserialise(stream);
            Gp2PowerUpSettings.Deserialise(stream);
            Gp3PowerUpSettings.Deserialise(stream);
        }
    }
}
