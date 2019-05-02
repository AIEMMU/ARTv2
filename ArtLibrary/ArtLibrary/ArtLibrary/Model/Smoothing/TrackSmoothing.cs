/*Automated Rodent Tracker - A program to automatically track rodents
Copyright(C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArtLibrary.Extensions;
using ArtLibrary.ModelInterface.Smoothing;

namespace ArtLibrary.Model.Smoothing
{
    internal class TrackSmoothing : ModelObjectBase, ITrackSmoothing
    {
        public PointF[] SmoothTrack(PointF[] originalTrack, double smoothingFactor = 0.68)
        {
            int length = originalTrack.Length;
            PointF[] smoothedMotion = new PointF[length];
            smoothedMotion[0] = originalTrack[0];
            smoothedMotion[length - 1] = originalTrack[length - 1];

            for (int i = 1; i < length - 1; i++)
            {
                PointF prevPoint = originalTrack[i - 1];
                PointF nextPoint = originalTrack[i + 1];

                PointF midPoint = prevPoint.MidPoint(nextPoint);

                PointF currentPoint = originalTrack[i];
                Vector dir = new Vector(midPoint.X - currentPoint.X, midPoint.Y - currentPoint.Y);
                dir *= smoothingFactor;
                smoothedMotion[i] = new PointF((float)(currentPoint.X + dir.X), (float)(currentPoint.Y + dir.Y));
            }

            return smoothedMotion;
        }

        public double GetTrackLength(PointF[] track)
        {
            double dist = 0;
            int length = track.Length;

            for (int i = 1; i < length; i++)
            {
                PointF p1 = track[i - 1];
                PointF p2 = track[i];
                dist += p1.Distance(p2);
            }

            return dist;
        }
    }
}
