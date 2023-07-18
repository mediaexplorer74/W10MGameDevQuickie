// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// A utility class that overlays a designer-friendly grid on top of the 
    /// application frame, for use similar to the performance counters in
    /// App.xaml.cs. The color and opacity are configurable. The grid contains 
    /// a number of squares that are 24x24, offset with 12px gutters, and all
    /// 24px away from the edge of the device.
    /// </summary>
    public static class MetroGridHelper
    {
        private static bool visible;
        private static double opacity = 0.25;
        private static Color color = Colors.Red;
        private static List<Rectangle> squares;
        private static Grid grid;

        /// <summary>
        /// Gets or sets a value indicating whether the designer grid is 
        /// visible on top of the application's frame.
        /// </summary>
        public static bool IsVisible
        {
            get { return visible; }
            set
            {
                visible = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets the color to use for the grid's squares.
        /// </summary>
        public static Color Color
        {
            get { return color; }
            set
            {
                color = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the opacity for the grid's squares.
        /// </summary>
        public static double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Updates the grid (if it already has been created) or initializes it
        /// otherwise.
        /// </summary>
        private static void UpdateGrid()
        {
            if (squares != null)
            {
                SolidColorBrush brush = new SolidColorBrush(color);
                foreach (Rectangle square in squares)
                {
                    square.Fill = brush;
                }
                if (grid != null)
                {
                    grid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                    grid.Opacity = opacity;
                }
            }
            else
            {
                BuildGrid();
            }
        }

        /// <summary>
        /// Builds the grid.
        /// </summary>
        private static void BuildGrid()
        {
            squares = new List<Rectangle>();

            Frame frame = Application.Current.RootVisual as Frame;
            if (frame == null || VisualTreeHelper.GetChildrenCount(frame) == 0)
            {
                Deployment.Current.Dispatcher.BeginInvoke(BuildGrid);
                return;
            }

            DependencyObject child = VisualTreeHelper.GetChild(frame, 0);
            Border childAsBorder = child as Border;
            Grid childAsGrid = child as Grid;
            if (childAsBorder != null)
            {
                // Not a pretty way to control the root visual, but I did not
                // want to implement using a popup.
                UIElement content = childAsBorder.Child;
                if (content == null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(BuildGrid);
                    return;
                }
                childAsBorder.Child = null;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    Grid newGrid = new Grid();
                    childAsBorder.Child = newGrid;
                    newGrid.Children.Add(content);
                    PrepareGrid(frame, newGrid);
                });
            }
            else if (childAsGrid != null)
            {
                PrepareGrid(frame, childAsGrid);
            }
            else
            {
                Debug.WriteLine("Dear developer:");
                Debug.WriteLine("Unfortunately the design overlay feature requires that the root frame visual");
                Debug.WriteLine("be a Border or a Grid. So the overlay grid just isn't going to happen.");
            }
        }

        /// <summary>
        /// Does the actual work of preparing the grid once the parent frame is
        /// in the visual tree and we have a Grid instance to work with for
        /// placing the chilren.
        /// </summary>
        /// <param name="frame">The phone application frame.</param>
        /// <param name="parent">The parent grid to insert the sub-grid into.</param>
        private static void PrepareGrid(Frame frame, Grid parent)
        {
            SolidColorBrush brush = new SolidColorBrush(color);

            grid = new Grid {IsHitTestVisible = false};

            // To support both orientations, unfortunately more visuals need to
            // be used. An alternate implementation would be to react to the
            // orientation change event and re-draw/remove squares.
            double width = frame.ActualWidth;
            double height = frame.ActualHeight;
            double max = Math.Max(width, height);

            for (int x = 24; x < /*width*/ max; x += 37)
            {
                for (int y = 24; y < /*height*/ max; y += 37)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 25,
                        Height = 25,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(x, y, 0, 0),
                        IsHitTestVisible = false,
                        Fill = brush,
                    };
                    grid.Children.Add(rect);
                    squares.Add(rect);
                }
            }

            grid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            grid.Opacity = opacity;

            // For performance reasons a single surface should ideally be used
            // for the grid.
            grid.CacheMode = new BitmapCache();

            // Places the grid into the visual tree. It is never removed once
            // being added.
            parent.Children.Add(grid);
        }
    }
}
