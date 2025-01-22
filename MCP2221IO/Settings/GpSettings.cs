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

using MCP2221IO.Gp;
using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    /// <summary>
    /// The GP settings read from flash
    /// </summary>
    public class GpSettings
    {
        public GpSetting<Gp0Designation>? Gp0PowerUpSetting { get; internal set; }

        public GpSetting<Gp1Designation>? Gp1PowerUpSetting { get; internal set; }

        public GpSetting<Gp2Designation>? Gp2PowerUpSetting { get; internal set; }

        public GpSetting<Gp3Designation>? Gp3PowerUpSetting { get; internal set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Gp0PowerUpSetting)}=>{Gp0PowerUpSetting}");
            stringBuilder.AppendLine($"{nameof(Gp1PowerUpSetting)}=>{Gp1PowerUpSetting}");
            stringBuilder.AppendLine($"{nameof(Gp2PowerUpSetting)}=>{Gp2PowerUpSetting}");
            stringBuilder.AppendLine($"{nameof(Gp3PowerUpSetting)}=>{Gp3PowerUpSetting}");

            return stringBuilder.ToString();
        }

        internal void Deserialize(Stream stream)
        {
            Gp0PowerUpSetting = new GpSetting<Gp0Designation>();
            Gp1PowerUpSetting = new GpSetting<Gp1Designation>();
            Gp2PowerUpSetting = new GpSetting<Gp2Designation>();
            Gp3PowerUpSetting = new GpSetting<Gp3Designation>();

            Gp0PowerUpSetting.Deserialize(stream);
            Gp1PowerUpSetting.Deserialize(stream);
            Gp2PowerUpSetting.Deserialize(stream);
            Gp3PowerUpSetting.Deserialize(stream);
        }

        internal void Serialize(Stream stream)
        {
            Gp0PowerUpSetting!.Serialize(stream);
            Gp1PowerUpSetting!.Serialize(stream);
            Gp2PowerUpSetting!.Serialize(stream);
            Gp3PowerUpSetting!.Serialize(stream);
        }
    }
}
