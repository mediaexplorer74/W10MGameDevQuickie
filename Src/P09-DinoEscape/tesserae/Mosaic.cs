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
    public class Mosaic
    {
        public Dictionary<TmxTileset, Texture2D> spriteSheet;
        public Dictionary<int, Rectangle> tileRect;
        public Dictionary<int, TmxTileset> idSheet;
        public List<int[,]> layerID;     // layerID[x,y]
        
        public int tMapWidth, tMapHeight;
        
        // Temporary
        public TmxMap map;          // TMX data (try to remove this)
        public Canvas canvas;       // Viewport details
        
        public Game game;
        public RenderTarget2D renderTarget;
        public SpriteBatch batch;
        
        public Mosaic(Game gameInput, string mapName)
        {
            // Temporary code
            game = gameInput;
            map = new TmxMap(mapName);
            tMapWidth = map.Width;
            tMapHeight = map.Height;
            
            // Initialize graphics buffers
            canvas = new Canvas(game);
            renderTarget = new RenderTarget2D(game.GraphicsDevice,
                                              game.GraphicsDevice.Viewport.Width,
                                              game.GraphicsDevice.Viewport.Height);
            
            // Load spritesheets
            spriteSheet = new Dictionary<TmxTileset, Texture2D>();
            tileRect = new Dictionary<int, Rectangle>();
            idSheet = new Dictionary<int, TmxTileset>();
            
            foreach (TmxTileset ts in map.Tilesets)
            {
                var newSheet = GetSpriteSheet(ts.Image.Source);
                spriteSheet.Add(ts, newSheet);
                
                // Loop hoisting
                var wStart = ts.Margin;
                var wInc = ts.TileWidth + ts.Spacing;
                var wEnd = newSheet.Width;
                
                var hStart = ts.Margin;
                var hInc = ts.TileHeight + ts.Spacing;
                var hEnd = newSheet.Height;
                
                // Pre-compute tileset rectangles
                var id = ts.FirstGid;
                for (var h = hStart; h < hEnd; h += hInc)
                {
                    for (var w = wStart; w < wEnd; w += wInc)
                    {
                        var rect = new Rectangle(w, h,
                                                 ts.TileWidth, ts.TileHeight);
                        idSheet.Add(id, ts);
                        tileRect.Add(id, rect);
                        id += 1;
                    }
                }
                
                // Ignore properties for now
            }
            
            // Load id maps
            layerID = new List<int[,]>();


            // Phase 1 : TmxLayer ------------------------------
            try
            {
                foreach (TmxLayer layer in map.Layers)
                {
                    var idMap = new int[tMapWidth, tMapHeight];

                    try
                    {
                        foreach (TmxLayerTile t in layer.Tiles)
                        {
                            idMap[t.X, t.Y] = t.Gid;
                        }
                    }
                    catch (Exception ex2) 
                    {
                        Debug.WriteLine("[ex2] foreach (TmxLayerTile... bug: " + ex2.Message);
                        continue;
                    }
                    
                    layerID.Add(idMap);

                    // Ignore properties for now
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] layerID.Add bug: " + ex.Message);
            }
            //-------------------------------------------------------------------

            // Warning: Experimental Part !!! Delete phase 2 if big problems occur 
            // Phase 2 : TmxObject ----------------------
            /*
            try
            {
                foreach (TmxObjectGroup layer in map.Layers)
                {
                    var idMap = new int[tMapWidth, tMapHeight];

                    try
                    {
                        foreach (var t in layer.Objects)
                        {
                            idMap[(int)t.X, (int)t.Y] = t.Id;
                        }
                    }
                    catch (Exception ex2)
                    {
                        Debug.WriteLine("[ex2] foreach (TmxLayerTile... bug: " + ex2.Message);
                        continue;
                    }

                    layerID.Add(idMap);

                    // Ignore properties for now
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] layerID.Add bug: " + ex.Message);
            }
            */
            //----------------------------------------------------------------

        }//Mosaic


        //
        public void DrawCanvas(SpriteBatch batch)
        {
            // Loop hoisting (Determined from Canvas)
            var iStart = Math.Max(0, canvas.tStartX);
            var iEnd = Math.Min(tMapWidth, canvas.tEndX);
            
            var jStart = Math.Max(0, canvas.tStartY);
            var jEnd = Math.Min(tMapHeight, canvas.tEndY);
            
            // Initialize the renderTarget spriteBatch
            game.GraphicsDevice.SetRenderTarget(renderTarget);
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                              SamplerState.PointClamp, null, null);
            // Draw tiles inside canvas
            foreach (var idMap in layerID)
            {
                for (var i = iStart; i < iEnd; i++)
                {
                    for (var j = jStart; j < jEnd; j++)
                    {
                        var id = idMap[i,j];
                        
                        // Skip unmapped cells
                        if (id == 0) continue;
                        
                        // Pre-calculate? (not with tileScale in there...)
                        var position = new Vector2(
                                        map.TileWidth * canvas.tileScale * i,
                                        map.TileHeight * canvas.tileScale * j);
                        
                        batch.Draw(spriteSheet[idSheet[id]], position,
                                tileRect[id], Color.White, 0.0f, canvas.origin,
                                canvas.tileScale, SpriteEffects.None, 0);
                    }
                }
            }
            batch.End();
            
            // Close render target
            game.GraphicsDevice.SetRenderTarget(null);

        }
        
        public Texture2D GetSpriteSheet(string filepath)
        {
            Texture2D newSheet = default;
            Stream imgStream = default;

            try
            {
                imgStream = File.OpenRead(filepath);
            }
            catch (Exception ex)
            {
            }

            try
            {
                newSheet = Texture2D.FromStream(game.GraphicsDevice, imgStream);
            }
            catch (Exception ex)
            {
            }

            return newSheet;
        }
    }
}
