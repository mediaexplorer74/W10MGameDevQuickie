﻿using Microsoft.Xna.Framework;
using System;
//using System;
//using System.Windows;

namespace Win8.Core.Gestures
{
    internal static class MathHelpers
    {
        /// <summary>
        /// Return the angle of the hypotenuse of a triangle with
        /// sides defined by deltaX and deltaY.
        /// </summary>
        /// <param name="deltaX">Change in X.</param>
        /// <param name="deltaY">Change in Y.</param>
        /// <returns>The angle (in degrees).</returns>
        public static double GetAngle(double deltaX, double deltaY)
        {
            double angle = Math.Atan2(deltaY, deltaX);
            if (angle < 0)
            {
                angle = 2 * Math.PI + angle;
            }

            return (angle * 360) / (2 * Math.PI);
        }

        /// <summary>
        /// Return the distance between two points
        /// </summary>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static double GetDistance(Point p0, Point p1)
        {
            double dx = p0.X - p1.X;
            double dy = p0.Y - p1.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Helper extension method for turning XNA's Vector2 type into a Point
        /// </summary>
        /// <param name="v">The Vector2.</param>
        /// <returns>The point.</returns>
        public static Point ToPoint(this Microsoft.Xna.Framework.Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        } 
    }
}
