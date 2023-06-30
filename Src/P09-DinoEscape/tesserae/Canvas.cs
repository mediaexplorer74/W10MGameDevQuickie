// Distributed as part of Tesserae, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Tesserae
{
    // Canvas tracks three coordinate systems:
    // 1. pWidth/Height: Canvas pixel count
    // 2. tWidth/Height: Canvas tile count
    public class Canvas
    {
        // User-defined (or derived) fields
        public int tMaxWidth;
        public int tMaxHeight;
        
        // Canvas size
        public int pWidth;          // Canvas pixel width
        public int pHeight;         // Canvas pixel height
        public float tileScale;     // Canvas-to-window scale
        
        // Testing: tHalo = 0 (should be at least 2)
        public int tWidth;      // # of (full) tiles per width
        public int tHeight;     // # of (full) tiles per height
        public int tHalo = 0;   // # of rendered tiles outside viewport
        
        // Camera position (focal tile)
        public int pX;      // Camera X position in pixels
        public int pY;      // Camera Y position in pixels
        public int tX;      // Tile.X containing camera
        public int tY;      // Tile.Y containing camera
        
        // Canvas loop hoisting
        public int tStartX;
        public int tEndX;
        public int tStartY;
        public int tEndY;
        
        // Mosaic parameters
        // (Note: Testing with default arguments)
        public int pTileWidth = 16;      // Tile width in pixels
        public int pTileHeight = 16;     // Tile height in pixels
        
        // Necessary?
        Game game;
        public Vector2 camera;
        public Vector2 origin;
        
        public Canvas(Game gameInput)
        {
            game = gameInput;
            RescaleCanvas();
            game.Window.ClientSizeChanged += UpdateViewport;
        }
        
        public void RescaleCanvas()
        {
            // Screen pixel count
            var pWindowWidth = game.GraphicsDevice.Viewport.Bounds.Width;
            var pWindowHeight = game.GraphicsDevice.Viewport.Bounds.Height;            
            
            // Testing
            // Define maximum tile width/height
            tMaxWidth = 30;
            tMaxHeight = 15;
            
            // Determine the minimum scaling
            var xScale = (float)pWindowWidth / (pTileWidth * tMaxWidth);
            var yScale = (float)pWindowHeight / (pTileHeight * tMaxHeight);
            tileScale = Math.Max(xScale, yScale);
            tWidth = (int)Math.Round(pWindowWidth / (pTileWidth * tileScale));
            tHeight = (int)Math.Round(pWindowHeight / (pTileHeight * tileScale));
            
            // Virtual height is prescribed (i.e. window-independent)
            pHeight = (int)Math.Round(pWindowHeight / tileScale);
            pWidth = (int)Math.Round(pWindowWidth / tileScale);
            
            //----
            // Testing (Normally pX,pY would be set externally)
            
            // Get centre pixel (or left/above centre)
            var pXc = (pWidth - 1) / 2;
            var pYc = (pHeight - 1) / 2;
            
            // Get tile index containing the pixel
            tX = pXc / pTileWidth;
            tY = pYc / pTileHeight;
            
            // Readjust pX, pY to the centre of the tile
            pX = tX * pTileWidth + pTileWidth / 2;
            pY = tY * pTileHeight + pTileHeight / 2;
            
//            Console.WriteLine("tX, tWidth: {0}, {1}", tX, tWidth);
            //--- End Testing
            
            // Visible tile range (tStart <= t < tEnd)
            tStartX = tX - (tWidth - 1) / 2 - tHalo;
            tEndX = tX + (tWidth - 1) / 2 + 1 + tHalo;
            tStartY = tY - (tHeight - 1) / 2 - tHalo;
            tEndY = tY + (tHeight - 1) / 2 + 1 + tHalo;
            
            camera = new Vector2((float)pX, (float)pY);
            origin = camera - new Vector2((float)(pWidth/2), (float)(pHeight/2));
            
            // Testing
//            Console.WriteLine("pXc, pYc: {0},{1}", pXc, pYc);
//            Console.WriteLine("Camera: {0}", camera.ToString());
//            Console.WriteLine("Origin: {0}", origin.ToString());
//            
//            Console.WriteLine("i: {0}..{1}", tStartX, tEndX);
//            Console.WriteLine("j: {0}..{1}", tStartY, tEndY);
//            Console.WriteLine("Pixel Width : {0}", pWindowWidth);
//            Console.WriteLine("Pixel Height: {0}", pWindowHeight);
//            Console.WriteLine("pX, pY      : {0}, {1}", pX, pY);
        }
        
        public void UpdateViewport(object sender, EventArgs e)
        {
            game.Window.ClientSizeChanged -= UpdateViewport;
            RescaleCanvas();
            // Some Diagnostics
            Debug.WriteLine("Viewport: {0}, {1}",
                              game.GraphicsDevice.Viewport.Width,
                              game.GraphicsDevice.Viewport.Height);
            Debug.WriteLine("Window: {0}",
                              game.Window.ClientBounds.ToString());
            Debug.WriteLine("Bounds: {0}",
                              game.GraphicsDevice.PresentationParameters.Bounds.ToString());
            game.Window.ClientSizeChanged += UpdateViewport;
        }
    }
}
