/*
Manual Whisker Annotator - A program to manually annotate whiskers and analyse them
Copyright (C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ARWT.View.Results;
using ARWT.ViewModel;

namespace ARWT.Model.Analysis
{
    public static class BrettFFT
    {
        public static uint NearestPowerOfTwo(uint number)
        {
            uint num = number > 0 ? number - 1 : 0;

            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            num++;

            return num;
        }

        public static uint PreviousPowerOfTwo(uint number)
        {
            uint num = number > 0 ? number - 1 : 0;

            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            num++;

            return num/2;
        }

        public static double[] ZeroAverageSignal(double[] data)
        {
            double signalMean = GetMeanSignal(data);

            double[] result = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = data[i] - signalMean;
            }

            return result;
        }

        public static double GetMeanSignal(double[] data)
        {
            double result = data.Sum();

            result /= data.Length;

            return result;
        }

        public static Dictionary<double, double> BrettDFT(double[] data, out double bestFrequency, double lowFrequencyThreshold = 0, double highFrequencyThreshold = 20, double frequencyStep = 0.01)
        {
            double[] zeroMeanedData = ZeroAverageSignal(data);
            Dictionary<double, double> result = new Dictionary<double, double>();
            double highestFrequencySoFar = 0;
            double highestValueSoFar = 0;
            for (double frequency = lowFrequencyThreshold; frequency <= highFrequencyThreshold; frequency += frequencyStep)
            {
                Complex value = ComputeDftForFrequency(zeroMeanedData, frequency);
                double power = Math.Pow(value.Real, 2) + Math.Pow(value.Imaginary, 2);

                if (power > highestValueSoFar)
                {
                    highestValueSoFar = power;
                    highestFrequencySoFar = frequency;
                }

                result.Add(frequency, power);
            }

            bestFrequency = highestFrequencySoFar;
            return result;
        }

        public static Dictionary<double, double> BrettDFT(double[] data, out double[] zeroedSignal, out double bestFrequency, double frameRate, double lowFrequencyThreshold = 0, double highFrequencyThreshold = 20, double frequencyStep = 0.01)
        {
            double elapsedTime = data.Length/frameRate;
            zeroedSignal = ZeroAverageSignal(data);
            Dictionary<double, double> result = new Dictionary<double, double>();
            double highestFrequencySoFar = 0;
            double highestValueSoFar = 0;
            for (double frequency = lowFrequencyThreshold; frequency <= highFrequencyThreshold; frequency += frequencyStep)
            {
                Complex value = ComputeDftForFrequency(zeroedSignal, frequency);
                double power = Math.Pow(value.Real, 2) + Math.Pow(value.Imaginary, 2);

                if (power > highestValueSoFar)
                {
                    highestValueSoFar = power;
                    highestFrequencySoFar = frequency;
                }

                result.Add(frequency, power);
            }

            bestFrequency = highestFrequencySoFar/elapsedTime;
            
            return result;
        }

        public static Complex ComputeDftForFrequency(double[] data, double frequency)
        {
            double realCounter = 0;
            double imagCounter = 0;
            double[] reals = new double[data.Length];
            double[] comps = new double[data.Length];
            //double piValue = 2*Math.PI/data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                double realResult = data[i]*Math.Cos((2*Math.PI*i*frequency)/data.Length);
                double compResult = data[i]*Math.Sin((2*Math.PI*i*frequency)/data.Length);

                realCounter += realResult;
                imagCounter += compResult;

                reals[i] = realResult;
                comps[i] = compResult;
            }
            Complex result = new Complex(realCounter, imagCounter);
            return result;
        }
    }
}
