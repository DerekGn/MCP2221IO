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

using System;

namespace PModAqs.Sensor
{
    internal interface ICcs811 : IDisposable
    {
        /// <summary>
        /// Get the sensor <see cref="VersionInfo"/>
        /// </summary>
        /// <returns>The sensor <see cref="VersionInfo"/></returns>
        VersionInfo GetVersion();
        /// <summary>
        /// Get the sensor <see cref="Mode"/>
        /// </summary>
        /// <returns>The sensor <see cref="Mode"/></returns>
        Mode GetMode();
        /// <summary>
        /// Get the sensor <see cref="SensorData"/>
        /// </summary>
        /// <returns>The sensor <see cref="SensorData"/></returns>
        SensorData GetData();
        /// <summary>
        /// Set the <see cref="DriveMode"/>
        /// </summary>
        /// <param name="mode">The <see cref="DriveMode"/> to set</param>
        void SetMode(DriveMode mode);
        /// <summary>
        /// Get the sensor <see cref="Status"/>
        /// </summary>
        /// <returns>The sensor <see cref="Status"/></returns>
        Status GetStatus();
        /// <summary>
        /// Get the sensor <see cref="Error"/>
        /// </summary>
        /// <returns>The sensor <see cref="Error"/></returns>
        Error GetError();
        /// <summary>
        /// Start the application
        /// </summary>
        void StartApplication();
        /// <summary>
        /// Reset the sensor
        /// </summary>
        void Reset();
        /// <summary>
        /// Read the internal temperature sensor
        /// </summary>
        /// <returns>The temperature value</returns>
        double GetTemperature();
    }
}