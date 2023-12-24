using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Q U A D  B A T C H   ( like SPRITEBATCH with more drawing options [like distortions and individual colors at each vertex] )

namespace GameManager
{
    //FONTS : to create a font the surrounding colors must be green(0,255,0) with alpha space as 1 and green seperations spaces as 1 
    public enum Alignment { None, LeftAlign, RightAlign, CenterAlign }
    public struct FontData {
        public float x1, y1, x2, y2;
        public float w, h, iw;
    }
    public class QuadBatch
    {
        // PUBLIC
        public BlendState blendState;
        public SamplerState samplerState;
        public DepthStencilState depthStencilState; // depth is default to off
        public Texture2D tex = null, font_tex = null;
        public Effect default_shader = null;
        public Effect fx = null;                    // substitute pixel-shader effect for example
        public float text_h_space, text_v_space;    // font spacing
        public Vector2 origin_default;
        public float depth, font_depth;
        public int screenWidth, screenHeight;
        // PRIVATE
        private GraphicsDevice device;
        private VertexPositionColorTexture[] vertices;
        private VertexPositionColorTexture[] fontverts;
        private short[] indices;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private bool beginCalled;
        private bool font_tex_loaded = false;
        private int vert_count, font_vert_count;
        private FontData[] fd;
        private float default_font_size = 0.5f;
        private Matrix world, view, proj, font_world, font_view, font_proj, ViewProj, WorldViewProj, font_WorldViewProj;        

        // CONSTRUCTOR
        public QuadBatch(ContentManager content, GraphicsDevice graphics, string Default_Effect_File, string FontTextureFile, int? ScreenWidth, int? ScreenHeight)
        {
            Rectangle pixel = new Rectangle(1, 1, 1, 1);
            VertexDeclaration vertexDeclaration;
            default_shader = content.Load<Effect>(Default_Effect_File);
            device = graphics;
            if (ScreenWidth.HasValue) screenWidth = ScreenWidth.Value; else screenWidth = device.Viewport.Width;
            if (ScreenHeight.HasValue) screenHeight = ScreenHeight.Value; else screenHeight = device.Viewport.Height;
            beginCalled = false;
            vertices  = new VertexPositionColorTexture[8192]; // 2048 sprites * 4 vertices per sprite IS 8192
            fontverts = new VertexPositionColorTexture[8192]; // 2048 sprites * 4 vertices per sprite IS 8192
            vertexDeclaration = VertexPositionColorTexture.VertexDeclaration;
            indices = new short[12288]; // 2048 sprites * 6 indices per sprite IS 12288
            int c = 0;
            for (int i = 0; i < 8192; i += 8)
            {
                indices[c] = (short)(i + 0); c++; indices[c] = (short)(i + 1); c++; indices[c] = (short)(i + 2); c++;
                indices[c] = (short)(i + 2); c++; indices[c] = (short)(i + 3); c++; indices[c] = (short)(i + 0); c++;
                indices[c] = (short)(i + 4); c++; indices[c] = (short)(i + 5); c++; indices[c] = (short)(i + 6); c++;
                indices[c] = (short)(i + 6); c++; indices[c] = (short)(i + 7); c++; indices[c] = (short)(i + 4); c++;
            }
            vertexBuffer = new DynamicVertexBuffer(graphics, typeof(VertexPositionColorTexture), 8192, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(graphics, typeof(short), 12288, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
            fd = new FontData[130]; 
            origin_default = new Vector2(0.5f, 0.5f);
            Use2DCamera();
            PrepareFont(content.Load<Texture2D>(FontTextureFile));
            blendState = BlendState.AlphaBlend;
            samplerState = SamplerState.LinearClamp;
            depthStencilState = DepthStencilState.None;
            if (device.RasterizerState.CullMode != CullMode.None) device.RasterizerState = RasterizerState.CullNone;
        }
        public void SetEffect(Effect F_ect) { fx = F_ect; }
        public void SetWorld(Matrix World) { world = World; WorldViewProj = world * ViewProj; }
        public void SetFontWorld(Matrix World) { font_world = World; font_WorldViewProj = font_world * font_view * font_proj; }

        // U S E  2 D  C A M E R A
        public void Use2DCamera()
        {
            world = Matrix.Identity;
            proj = Matrix.CreateOrthographicOffCenter(0.0f, screenWidth, screenHeight, 0.0f, -2000f, 2000f);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0); proj = halfPixelOffset * proj; // <-- fixes half-pixel offset problem
            view = Matrix.Identity; depth = 0.0f;
            font_world = world; font_view = view; font_proj = proj;
            ViewProj = view * proj; WorldViewProj = world * ViewProj; font_WorldViewProj = WorldViewProj;
        }


        //  M E O  M O T I O N  C H A R A C T E R  D R A W   ( used in   M E O   P L A Y E R )
        //  D R A W  T R A N S F O R M E D  V E R T I C E S  ( assumes verts already transformed )     
        public void DrawTransformedVertices(Rectangle sourceRect, Vector2 scene_origin, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) 
            { 
               Debug.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); 
                return; 
            }
            float u1, v1, u2, v2;
            p1 = (p1 + scene_origin); 
            p2 = (p2 + scene_origin); 
            p3 = (p3 + scene_origin); 
            p4 = (p4 + scene_origin);
            
            u1 = sourceRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = sourceRect.Y / (float)tex.Height;
            u2 = (sourceRect.X + sourceRect.Width) / (float)tex.Width; 
            v2 = (sourceRect.Y + sourceRect.Height) / (float)tex.Height;
            
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                Single temp = v2; v2 = v1; v1 = temp;
            }
            
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                Single temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(p1, depth);
            vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); // upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(p2, depth);
            vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); // upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(p3, depth);
            vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); // lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(p4, depth);
            vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); // lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;

            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); 
                beginCalled = true; 
                vert_count = 0;
            }
        } // DrawTransformedVertices (for MeoMotion)


        //non-uniform scaling and rotation with single color:        
        public void Draw(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, Color color, SpriteEffects flip = SpriteEffects.None) {
            Draw(sourceRect, pos, null, scale, rot, color, flip);
        }
        public void DrawDepth(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, Color color, float z_depth, SpriteEffects flip = SpriteEffects.None) {
            depth = z_depth;
            Draw(sourceRect, pos, null, scale, rot, color, flip);
        }
        public void DrawDepth(Rectangle sourceRect, Vector2 pos, Vector2 origin, Vector2 scale, float rot, Color color, float z_depth, SpriteEffects flip = SpriteEffects.None) {
            depth = z_depth;
            Draw(sourceRect, pos, origin, scale, rot, color, color, color, color, flip);
        }
        //just rotates and scales:         
        public void DrawColor(Rectangle sourceRect, Vector2 pos, float scale, float rot, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None) {
            Draw(sourceRect, pos, null, new Vector2(scale, scale), rot, c1, c2, c3, c4, flip);
        }
        public void DrawColorDepth(Rectangle sourceRect, Vector2 pos, float scale, float rot, Color c1, Color c2, Color c3, Color c4, float z_depth, SpriteEffects flip = SpriteEffects.None) {
            depth = z_depth;
            Draw(sourceRect, pos, null, new Vector2(scale, scale), rot, c1, c2, c3, c4, flip);
        }
        public void DrawColor(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None) {
            Draw(sourceRect, pos, null, scale, rot, c1, c2, c3, c4, flip);
        }
        //assume origin is middle:        
        public void DrawColorDistort(Rectangle sourceRect, Vector2 pos, float scale, float rot, int x1off, int y1off, int x2off, int y2off, int x3off, int y3off, int x4off, int y4off, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None) {
            Draw(sourceRect, pos, null, new Vector2(scale, scale), rot, x1off, y1off, x2off, y2off, x3off, y3off, x4off, y4off, c1, c2, c3, c4, flip);
        }
        public void DrawColorDistort(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, int x1off, int y1off, int x2off, int y2off, int x3off, int y3off, int x4off, int y4off, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None) {
            Draw(sourceRect, pos, null, scale, rot, x1off, y1off, x2off, y2off, x3off, y3off, x4off, y4off, c1, c2, c3, c4, flip);
        }
        public void DrawColorDistortDepth(Rectangle sourceRect, Vector2 pos, float scale, float rot, int x1off, int y1off, int x2off, int y2off, int x3off, int y3off, int x4off, int y4off, Color c1, Color c2, Color c3, Color c4, float z_depth, SpriteEffects flip = SpriteEffects.None) {
            depth = z_depth;
            Draw(sourceRect, pos, null, new Vector2(scale, scale), rot, x1off, y1off, x2off, y2off, x3off, y3off, x4off, y4off, c1, c2, c3, c4, flip);
        }        


        public void Draw(Rectangle? sourceRect, Vector2 pos, Vector2? origin, Vector2? Scale, float rot, int x1off, int y1off, int x2off, int y2off, int x3off, int y3off, int x4off, int y4off, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            Vector2 scale = Vector2.One;             
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);            
            if (Scale.HasValue) scale = Scale.Value;
            if ((scale.X != 1) || (scale.Y != 1))
            {
                w = sourceRect.Value.Width * scale.X; h = sourceRect.Value.Height * scale.Y;
                if (origin.HasValue) { o_x = origin.Value.X * scale.X; o_y = origin.Value.Y * scale.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } // note: w and h are already scaled
                //get scaled offsets:
                float xo1 = x1off * scale.X, yo1 = y1off * scale.Y; float xo2 = x2off * scale.X, yo2 = y2off * scale.Y;
                float xo3 = x3off * scale.X, yo3 = y3off * scale.Y; float xo4 = x4off * scale.X, yo4 = y4off * scale.Y;
                //what is the point after offset
                x1 = pos.X + xo1; y1 = pos.Y + yo1;      //upper-left
                x2 = pos.X + w + xo2; y2 = pos.Y + yo2;      //upper-right
                x3 = pos.X + w + xo3; y3 = pos.Y + h + yo3;  //lower-right
                x4 = pos.X + xo4; y4 = pos.Y + h + yo4;  //lower-left  
            } else { //same with no scaling:            
                w = sourceRect.Value.Width; h = sourceRect.Value.Height;
                if (origin.HasValue) { o_x = origin.Value.X; o_y = origin.Value.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; }
                //need positions of destination                
                x1 = pos.X + x1off; y1 = pos.Y + y1off;     //upper-left
                x2 = pos.X + w + x2off; y2 = pos.Y + y2off;     //upper-right
                x3 = pos.X + w + x3off; y3 = pos.Y + h + y3off; //lower-right
                x4 = pos.X + x4off; y4 = pos.Y + h + y4off; //lower-left
            }
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot); //this is actually quite fast on a modern computer
                float hd = x1 - ox, vd = y1 - oy;
                x1 = ox + hd * cos - vd * sin; y1 = oy + hd * sin + vd * cos; hd = x2 - ox; vd = y2 - oy;
                x2 = ox + hd * cos - vd * sin; y2 = oy + hd * sin + vd * cos; hd = x3 - ox; vd = y3 - oy;
                x3 = ox + hd * cos - vd * sin; y3 = oy + hd * sin + vd * cos; hd = x4 - ox; vd = y4 - oy;
                x4 = ox + hd * cos - vd * sin; y4 = oy + hd * sin + vd * cos;
            }
            Rectangle tempRect = sourceRect.Value;
            u1 = tempRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;

            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }

        public void DrawColorDistort(Rectangle sourceRect, Vector2 pos, float scale, float rot, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None) {
            DrawColorDistort(sourceRect, pos, new Vector2(scale, scale), rot, v1, v2, v3, v4, c1, c2, c3, c4, flip);
        }
        //-------------------------------------
        // D R A W  - C O L O R - D I S T O R T  ( VECTOR TYPE )
        //-------------------------------------
        public void DrawColorDistort(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, Vector2 vv1, Vector2 vv2, Vector2 vv3, Vector2 vv4, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if ((scale.X != 1f) || (scale.Y != 1f))
            {
                w = sourceRect.Width * scale.X; h = sourceRect.Height * scale.Y;
                o_x = w * origin_default.X; o_y = h * origin_default.Y;
                //get scaled offsets:
                float xo1 = vv1.X * scale.X, yo1 = vv1.Y * scale.Y; float xo2 = vv2.X * scale.X, yo2 = vv2.Y * scale.Y;
                float xo3 = vv3.X * scale.X, yo3 = vv3.Y * scale.Y; float xo4 = vv4.X * scale.X, yo4 = vv4.Y * scale.Y;
                //what is the point after offset
                x1 = pos.X + xo1; y1 = pos.Y + yo1;         //upper-left
                x2 = pos.X + w + xo2; y2 = pos.Y + yo2;         //upper-right
                x3 = pos.X + w + xo3; y3 = pos.Y + h + yo3;     //lower-right
                x4 = pos.X + xo4; y4 = pos.Y + h + yo4;     //lower-left                
            }
            else
            { //same with no scaling:            
                w = sourceRect.Width; h = sourceRect.Height; //if (origin.HasValue) { o_x = origin.Value.X; o_y = origin.Value.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } //Note: w and h are already scaled sizes                        
                o_x = w * origin_default.X; o_y = h * origin_default.Y;
                //need positions of destination                
                x1 = pos.X + vv1.X; y1 = pos.Y + vv1.Y;         //upper-left
                x2 = pos.X + w + vv2.X; y2 = pos.Y + vv2.Y;         //upper-right
                x3 = pos.X + w + vv3.X; y3 = pos.Y + h + vv3.Y;     //lower-right
                x4 = pos.X + vv4.X; y4 = pos.Y + h + vv4.Y;     //lower-left
            }
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot); //this is actually quite fast on a modern computer
                float hd = x1 - ox, vd = y1 - oy;
                x1 = ox + hd * cos - vd * sin; y1 = oy + hd * sin + vd * cos; hd = x2 - ox; vd = y2 - oy;
                x2 = ox + hd * cos - vd * sin; y2 = oy + hd * sin + vd * cos; hd = x3 - ox; vd = y3 - oy;
                x3 = ox + hd * cos - vd * sin; y3 = oy + hd * sin + vd * cos; hd = x4 - ox; vd = y4 - oy;
                x4 = ox + hd * cos - vd * sin; y4 = oy + hd * sin + vd * cos;
            }
            Rectangle tempRect = sourceRect;
            u1 = tempRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//Draw (Color-Distort)
        // 1 color version of distort:
        public void DrawColorDistort(Rectangle sourceRect, Vector2 pos, Vector2 scale, float rot, Vector2 vv1, Vector2 vv2, Vector2 vv3, Vector2 vv4, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if ((scale.X != 1f) || (scale.Y != 1f)) 
            {
                w = sourceRect.Width * scale.X; h = sourceRect.Height * scale.Y;
                o_x = w * origin_default.X; o_y = h * origin_default.Y;
                // get scaled offsets:
                float xo1 = vv1.X * scale.X, yo1 = vv1.Y * scale.Y;  float xo2 = vv2.X * scale.X, yo2 = vv2.Y * scale.Y;
                float xo3 = vv3.X * scale.X, yo3 = vv3.Y * scale.Y;  float xo4 = vv4.X * scale.X, yo4 = vv4.Y * scale.Y;
                // what is the point after offset:
                x1 = pos.X + xo1; y1 = pos.Y + yo1;         // upper-left
                x2 = pos.X + w + xo2; y2 = pos.Y + yo2;     // upper-right
                x3 = pos.X + w + xo3; y3 = pos.Y + h + yo3; // lower-right
                x4 = pos.X + xo4; y4 = pos.Y + h + yo4;     // lower-left
            }
            else
            { //same with no scaling
                w = sourceRect.Width; h = sourceRect.Height; 
                o_x = w * origin_default.X; o_y = h * origin_default.Y;
                //need positions of destination                
                x1 = pos.X + vv1.X; y1 = pos.Y + vv1.Y;         //upper-left
                x2 = pos.X + w + vv2.X; y2 = pos.Y + vv2.Y;     //upper-right
                x3 = pos.X + w + vv3.X; y3 = pos.Y + h + vv3.Y; //lower-right
                x4 = pos.X + vv4.X; y4 = pos.Y + h + vv4.Y;     //lower-left
            }
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot); //this is actually quite fast on a modern computer
                float hd = x1 - ox, vd = y1 - oy;
                x1 = ox + hd * cos - vd * sin; y1 = oy + hd * sin + vd * cos; hd = x2 - ox; vd = y2 - oy;
                x2 = ox + hd * cos - vd * sin; y2 = oy + hd * sin + vd * cos; hd = x3 - ox; vd = y3 - oy;
                x3 = ox + hd * cos - vd * sin; y3 = oy + hd * sin + vd * cos; hd = x4 - ox; vd = y4 - oy;
                x4 = ox + hd * cos - vd * sin; y4 = oy + hd * sin + vd * cos;
            }
            Rectangle tempRect = sourceRect;
            u1 = tempRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//Draw (1 col-Distort)

        
        public void DrawEntire(Vector2 pos, Color color, SpriteEffects flip = SpriteEffects.None) {
            Draw(null, pos, color, flip); 
        }
        public void DrawDepth(Rectangle sourceRect, Vector2 pos, Color color, float z_depth, SpriteEffects flip = SpriteEffects.None)
        {
            depth = z_depth;
            Draw(sourceRect, pos, color, flip);
        }

        //-------------------
        // D R A W - 4 colors (no distort - non-uniform scale)
        //-------------------
        public void Draw(Rectangle? sourceRect, Vector2 pos, Vector2? origin, Vector2? _scale, float rot, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            Vector2 scale = Vector2.One;
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            if (_scale.HasValue) { scale = _scale.Value; }
            if ((scale.X != 1) || (scale.Y != 1))
            {
                w = sourceRect.Value.Width * scale.X; h = sourceRect.Value.Height * scale.Y;
                if (origin.HasValue) { o_x = origin.Value.X * scale.X; o_y = origin.Value.Y * scale.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } //Note: w and h are already scaled sizes       
            }
            else
            { // same but no scaling
                w = sourceRect.Value.Width; h = sourceRect.Value.Height;
                if (origin.HasValue) { o_x = origin.Value.X; o_y = origin.Value.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } //Note: w and h are already scaled sizes                                        
            }
            x1 = pos.X; y1 = pos.Y;         //upper-left
            x2 = pos.X + w; y2 = pos.Y;     //upper-right
            x3 = pos.X + w; y3 = pos.Y + h; //lower-right
            x4 = pos.X; y4 = pos.Y + h;     //lower-left                
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot); //this is actually quite fast on a modern computer               
                float hd1 = x1 - ox, vd1 = y1 - oy;//top-left dif
                float hd2 = x2 - ox, vd2 = y3 - oy;//bottom-right dif
                x1 = ox + hd1 * cos - vd1 * sin; y1 = oy + hd1 * sin + vd1 * cos; x2 = ox + hd2 * cos - vd1 * sin; y2 = oy + hd2 * sin + vd1 * cos;
                x3 = ox + hd2 * cos - vd2 * sin; y3 = oy + hd2 * sin + vd2 * cos; x4 = ox + hd1 * cos - vd2 * sin; y4 = oy + hd1 * sin + vd2 * cos;
            }
            Rectangle tempRect = sourceRect.Value;
            u1 = (tempRect.X + 0.5f) / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = (tempRect.Y + 0.5f) / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width - 0.5f) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height - 0.5f) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//Draw (secondary)


        //----------------------
        // D R A W  - Non-Uniform scale ( 1 color )
        public void Draw(Rectangle? sourceRect, Vector2 pos, Vector2? origin, Vector2? _scale, float rot, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            Vector2 scale = Vector2.One;
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            if (_scale.HasValue) { scale = _scale.Value; }
            if ((scale.X != 1) || (scale.Y != 1))
            {
                w = sourceRect.Value.Width * scale.X; h = sourceRect.Value.Height * scale.Y;
                if (origin.HasValue) { o_x = origin.Value.X * scale.X; o_y = origin.Value.Y * scale.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } //Note: w and h are already scaled sizes       
            }
            else
            { // same but no scaling
                w = sourceRect.Value.Width; h = sourceRect.Value.Height;
                if (origin.HasValue) { o_x = origin.Value.X; o_y = origin.Value.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; } //Note: w and h are already scaled sizes                                        
            }
            x1 = pos.X; y1 = pos.Y;         //upper-left
            x2 = pos.X + w; y2 = pos.Y;     //upper-right
            x3 = pos.X + w; y3 = pos.Y + h; //lower-right
            x4 = pos.X; y4 = pos.Y + h;     //lower-left                
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot); //this is actually quite fast on a modern computer               
                float hd1 = x1 - ox, vd1 = y1 - oy;//top-left dif
                float hd2 = x2 - ox, vd2 = y3 - oy;//bottom-right dif
                x1 = ox + hd1 * cos - vd1 * sin; y1 = oy + hd1 * sin + vd1 * cos; x2 = ox + hd2 * cos - vd1 * sin; y2 = oy + hd2 * sin + vd1 * cos;
                x3 = ox + hd2 * cos - vd2 * sin; y3 = oy + hd2 * sin + vd2 * cos; x4 = ox + hd1 * cos - vd2 * sin; y4 = oy + hd1 * sin + vd2 * cos;
            }
            Rectangle tempRect = sourceRect.Value;
            u1 = tempRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//Draw (Non-Uniform)

        //----------------------
        // D R A W  - ( 1 color , float scale)
        public void Draw(Rectangle? sourceRect, Vector2 pos, Vector2? origin, float scale, float rot, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            float w, h, o_x, o_y, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            if (scale != 1)
            {
                w = sourceRect.Value.Width * scale; h = sourceRect.Value.Height * scale;
                if (origin.HasValue) { o_x = origin.Value.X * scale;  o_y = origin.Value.Y * scale; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; }
            }
            else
            { // same but no scaling
                w = sourceRect.Value.Width; h = sourceRect.Value.Height;
                if (origin.HasValue) { o_x = origin.Value.X; o_y = origin.Value.Y; } else { o_x = w * origin_default.X; o_y = h * origin_default.Y; }
            }
            x1 = pos.X; y1 = pos.Y;         //upper-left
            x2 = pos.X + w; y2 = pos.Y;     //upper-right
            x3 = pos.X + w; y3 = pos.Y + h; //lower-right
            x4 = pos.X; y4 = pos.Y + h;     //lower-left   
            if (rot != 0f)
            {
                float ox = pos.X + o_x, oy = pos.Y + o_y;
                float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot);
                float hd1 = x1 - ox, vd1 = y1 - oy; // top-left dif
                float hd2 = x2 - ox, vd2 = y3 - oy; // bottom-right dif
                x1 = ox + hd1 * cos - vd1 * sin;  y1 = oy + hd1 * sin + vd1 * cos;
                x2 = ox + hd2 * cos - vd1 * sin;  y2 = oy + hd2 * sin + vd1 * cos;
                x3 = ox + hd2 * cos - vd2 * sin;  y3 = oy + hd2 * sin + vd2 * cos;
                x4 = ox + hd1 * cos - vd2 * sin;  y4 = oy + hd1 * sin + vd2 * cos;
            }
            Rectangle tempRect = sourceRect.Value;
            u1 = (tempRect.X + 0.5f) / (float)tex.Width;
            v1 = (tempRect.Y + 0.5f) / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width - 0.5f) / (float)tex.Width;
            v2 = (tempRect.Y + tempRect.Height - 0.5f) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }


        //-----------------
        // D R A W  4 Colors
        public void Draw(Rectangle? sourceRect, Vector2 pos, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            float w, h, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            w = sourceRect.Value.Width; h = sourceRect.Value.Height;
            // need positions of destination: 
            x1 = pos.X;     y1 = pos.Y;     //upper-left
            x2 = pos.X + w; y2 = pos.Y;     //upper-right
            x3 = pos.X + w; y3 = pos.Y + h; //lower-right
            x4 = pos.X;     y4 = pos.Y + h; //lower-left

            Rectangle tempRect = sourceRect.Value;
            u1 = tempRect.X / (float)tex.Width;
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width;
            v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0) {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0) {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); //upper-right texCoord
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); //lower-right texCoord
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); //lower-left texCoord
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192) {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }

        //-----------------
        // D R A W  1 Color
        public void Draw(Rectangle? sourceRect, Vector2 pos, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            float w, h, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            w = sourceRect.Value.Width; h = sourceRect.Value.Height;
            // need positions of destination: 
            x1 = pos.X;     y1 = pos.Y;     //upper-left
            x2 = pos.X + w; y2 = pos.Y;     //upper-right
            x3 = pos.X + w; y3 = pos.Y + h; //lower-right
            x4 = pos.X;     y4 = pos.Y + h; //lower-left

            Rectangle tempRect = sourceRect.Value;
            u1 = tempRect.X / (float)tex.Width;
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width;
            v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0) {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0) {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth);  vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); //upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); //lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); //lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }
        //-----------
        // DRAW_DEST
        //-----------
        public void DrawDest(Rectangle? sourceRect, Rectangle destRect, Color c1, Color c2, Color c3, Color c4, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float w, h, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2; // generate a TARGET WIDTH, HEIGHT based on the source
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            w = sourceRect.Value.Width; h = sourceRect.Value.Height;

            //need positions of destination            
            x1 = destRect.X; y1 = destRect.Y;                  //upper-left
            x2 = destRect.X + destRect.Width; y2 = destRect.Y; //upper-right
            x3 = destRect.X + destRect.Width; y3 = destRect.Y + destRect.Height; //lower-right
            x4 = destRect.X; y4 = destRect.Y + destRect.Height;     //lower-left              
            Rectangle tempRect = sourceRect.Value; // (<--this should be the case. Unlikely to use this without a sourceRect)            
            u1 = tempRect.X / (float)tex.Width; //gets the texture coords in terms of (0.0f-1.0f, 0.0f-1.0f)
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width; v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1);   //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1);   //upper-right texCoord
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2);   //lower-right texCoord
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2);   //lower-left texCoord
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//Draw (Draw Dest 4 color)
        // DRAW DEST 1 color:
        public void DrawDest(Rectangle? sourceRect, Rectangle destRect, Color c1, SpriteEffects flip = SpriteEffects.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            float w, h, x1, y1, x2, y2, x3, y3, x4, y4, u1, v1, u2, v2;
            if (!sourceRect.HasValue) sourceRect = new Rectangle(0, 0, tex.Width, tex.Height);
            w = sourceRect.Value.Width; h = sourceRect.Value.Height;
            
            // need positions of destination: 
            x1 = destRect.X; y1 = destRect.Y;                      //upper-left
            x2 = destRect.X + destRect.Width; y2 = destRect.Y;     //upper-right
            x3 = destRect.X + destRect.Width; y3 = destRect.Y + destRect.Height; //lower-right
            x4 = destRect.X; y4 = destRect.Y + destRect.Height; //lower-left

            Rectangle tempRect = sourceRect.Value;
            u1 = tempRect.X / (float)tex.Width;
            v1 = tempRect.Y / (float)tex.Height;
            u2 = (tempRect.X + tempRect.Width) / (float)tex.Width;
            v2 = (tempRect.Y + tempRect.Height) / (float)tex.Height;
            if ((flip & SpriteEffects.FlipVertically) != 0)
            {
                var temp = v2; v2 = v1; v1 = temp;
            }
            if ((flip & SpriteEffects.FlipHorizontally) != 0)
            {
                var temp = u2; u2 = u1; u1 = temp;
            }
            vertices[vert_count].Position = new Vector3(x1, y1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); //upper-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x2, y2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); //upper-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x3, y3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); //lower-right texCoord
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(x4, y4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); //lower-left texCoord
            vertices[vert_count].Color = c1; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }//DrawDest 1 color



        // B E G I N -------------------------------------
        /// <summary>
        /// Start a batch of rendering using a sprite sheet texture
        /// </summary>        
        //BEGIN-----------------------------------------------------------------------------------------------------------
        public void Begin(Texture2D texture)
        {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; } if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture; vert_count = 0; font_vert_count = 0; beginCalled = true;
        }
        public void Begin(Texture2D texture, BlendState blend_State)
        {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; } if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture; blendState = blend_State; vert_count = 0; font_vert_count = 0; beginCalled = true;
        }
        public void Begin(BlendState blend_State)
        {
            if (tex == null) { Console.WriteLine("QuadBatch Begin aborted because tex == null"); return; } if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            blendState = blend_State; vert_count = 0; font_vert_count = 0; beginCalled = true;
        }
        public void Begin(Texture2D texture, BlendState blend_State, SamplerState sampler_State) {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; }
            if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture; blendState = blend_State; samplerState = sampler_State;
            vert_count = 0; font_vert_count = 0; beginCalled = true;
        }
        public void Begin(Texture2D texture, BlendState blend_State, SamplerState sampler_State, DepthStencilState depth_StencilState) {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; }
            if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture; blendState = blend_State; samplerState = sampler_State; depthStencilState = depth_StencilState;
            vert_count = 0; font_vert_count = 0; beginCalled = true;
        }
        public void Begin(Texture2D texture, BlendState blend_State, SamplerState sampler_State, DepthStencilState depth_StencilState, Matrix World)
        {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; }
            if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture;
            blendState = blend_State;
            samplerState = sampler_State;
            depthStencilState = depth_StencilState;
            vert_count = 0; font_vert_count = 0; beginCalled = true;
            world = World; WorldViewProj = world * ViewProj;
        }        
        public void Begin(Texture2D texture, BlendState blend_state, SamplerState sampler_state, DepthStencilState depth_StencilState, Matrix World, Effect efct)
        {
            if (texture == null) { Console.WriteLine("QuadBatch Begin aborted because texture == null"); return; }
            if (beginCalled) { Console.WriteLine("QuadBatch Begin already called."); return; }
            tex = texture;
            blendState = blend_state;
            samplerState = sampler_state;
            depthStencilState = depth_StencilState;
            vert_count = 0; font_vert_count = 0; beginCalled = true;
            world = World; WorldViewProj = world * ViewProj;
            fx = efct;
        }



        // E N D --------------------------------------------
        public void End()
        {
            if (!beginCalled) { Console.WriteLine("call to End without begin called. Aborting End."); return; }
            if (vert_count >= 3)
            {
                device.BlendState = blendState;   device.DepthStencilState = depthStencilState;   device.SamplerStates[0] = samplerState;
                if (device.RasterizerState.CullMode != CullMode.None) device.RasterizerState = RasterizerState.CullNone;
                // Draw entire vertex list:
                int triangle_count = vert_count / 2;
                vertexBuffer.SetData(vertices, 0, vert_count);
                device.SetVertexBuffer(vertexBuffer);
                default_shader.CurrentTechnique.Passes[0].Apply();
                default_shader.Parameters["MatrixTransform"].SetValue(WorldViewProj);
                if (fx != null) fx.CurrentTechnique.Passes[0].Apply();
                device.Textures[0] = tex;
                device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vert_count, indices, 0, triangle_count);
            }
            if (font_vert_count > 0) EndFont();
            beginCalled = false; fx = null;
        }

        // E N D  F O N T
        public void EndFont()
        {
            if (!beginCalled) { Console.WriteLine("call to End without begin called. Aborting End."); return; }
            if (font_vert_count < 3) return;
            if (vert_count < 3) {
                device.BlendState = blendState;  device.DepthStencilState = depthStencilState;  device.SamplerStates[0] = samplerState;
                if (device.RasterizerState.CullMode != CullMode.None) device.RasterizerState = RasterizerState.CullNone;
            }
            // Draw entire vertex list:
            int triangle_count = font_vert_count / 2;
            vertexBuffer.SetData(fontverts, 0, font_vert_count);
            device.SetVertexBuffer(vertexBuffer);
            default_shader.CurrentTechnique.Passes[0].Apply();
            default_shader.Parameters["MatrixTransform"].SetValue(font_WorldViewProj);            
            device.Textures[0] = font_tex;
            device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, fontverts, 0, font_vert_count, indices, 0, triangle_count);
        }


        public void EndVerts()
        {
            if (!beginCalled) { Console.WriteLine("call to End without begin called. Aborting End."); return; }
            beginCalled = false;
            if (vert_count < 3) return;            
            device.BlendState = blendState; device.DepthStencilState = depthStencilState; device.SamplerStates[0] = samplerState;
            if (device.RasterizerState.CullMode != CullMode.None) device.RasterizerState = RasterizerState.CullNone;
            // Draw entire vertex list:
            int triangle_count = vert_count / 2;
            vertexBuffer.SetData(vertices, 0, vert_count);
            device.SetVertexBuffer(vertexBuffer);
            default_shader.CurrentTechnique.Passes[0].Apply();
            default_shader.Parameters["MatrixTransform"].SetValue(WorldViewProj);
            if (fx != null) fx.CurrentTechnique.Passes[0].Apply();
            device.Textures[0] = tex;
            device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vert_count, indices, 0, triangle_count);            
        }


        // PREPARE FONT
        private float average_width;
        private float Average_Width { get { return average_width; } }
        public void PrepareFont(Texture2D newfont_tex) 
        {
            if (newfont_tex == null) { Console.WriteLine("Invalid texture for font."); return; }
            Color[] FontColorData;
            font_tex = newfont_tex;
            FontColorData = new Color[font_tex.Width * font_tex.Height];
            font_tex.GetData<Color>(FontColorData);
            font_tex_loaded = true;
            int wide = font_tex.Width, high = font_tex.Height, font_height = 49;
            
            float w = 1.0f / wide, h = 1.0f / high;
            int i = 32;
            int xa = 0, ya = 2;
            bool go_down = false, finished = false;
            Color col = (Color)FontColorData[0];
            average_width = 1f; 
            bool done = false;
            do 
            {
                ya++; // <-- must do this first
                col = FontColorData[3 + ya * wide];
                if ((col.R<5) && (col.G>249) && (col.B<5) && (col.A >249)) { done = true; }
                if (ya >= (high-1)) {done = true; Console.WriteLine("Error finding font height."); }
            } while(!done);
            font_height = ya-2;

            ya = 4;
            finished = false;
            do
            {
                col = FontColorData[xa + ya * wide];
                if ((col.R < 5) && (col.G > 249) && (col.B < 5) && (col.A > 249)) xa++;
                else
                {
                    fd[i].x1 = (float)xa + 1; fd[i].y1 = (float)(ya - 2);
                    done = false;
                    do
                    {
                        xa++;
                        col = FontColorData[xa + ya * wide];
                        if ((col.R < 5) && (col.G > 249) && (col.B < 5) && (col.A > 249)) { done = true; }
                        if (xa >= (wide - 1)) { done = true; go_down = true; }
                    } while (!done);
                    fd[i].x2 = (float)(xa - 2); fd[i].y2 = (float)(ya - 4 + font_height);
                    fd[i].w = fd[i].x2 - fd[i].x1; fd[i].h = fd[i].y2 - fd[i].y1;
                    average_width += fd[i].w; fd[i].iw = fd[i].w * 0.5f;
                    if (i < 130) i++; else { Console.WriteLine("index too high for font."); }
                }
                if ((xa >= (wide - 1)) || (go_down))
                {
                    go_down = false;
                    xa = 1; ya = (ya + font_height) + 2;
                    if (ya >= high) finished = true;
                }
                if (i > 127) finished = true;
            } while (!finished);
            text_h_space = (fd[32].x2 - fd[32].x1) * default_font_size; text_v_space = (fd[32].y2 - fd[32].y1 - 2) * default_font_size;
            average_width = average_width / i * 1.2f; 
        } // PrepareFont


        public float Default_Font_Size { get { return default_font_size; } set { default_font_size = value;  text_h_space = (fd[32].x2 - fd[32].x1) * default_font_size;  text_v_space = (fd[32].y2 - fd[32].y1 - 2) * default_font_size; } }
        
        // MEASURE STRONG
        public Vector2 MeasureString(string text)
        {
            float kern_space = text_h_space * 0.25f;
            float mult = default_font_size * 0.5f;
            Vector2 size = new Vector2(1, fd[32].h * default_font_size);
            int a = 0;
            do
            {
                size.X += fd[(int)text[a]].w;
                a++;
            } while (a < text.Length);
            size.X *= mult;
            size.X += (kern_space * text.Length) + text_h_space * 0.5f;
            return (size);
        }

        // MEASURE STRING FAST
        public Vector2 MeasureStringFast(string text)
        {
            float kern_space = text_h_space * 0.25f;
            float mult = default_font_size * 0.5f;
            Vector2 size = new Vector2(1, fd[32].h * default_font_size);
            size.X = text.Length * average_width;
            size.X *= mult;
            size.X += (kern_space * text.Length) + text_h_space * 0.5f;
            return (size);
        }


        // MEASURE MULTI STRING
        public Vector2 MeasureMultiString(string text)
        {
            float px = 0, py = 0, right_most = 0, bottom_most = 0, kern_space = text_h_space / 4, w, h;
            int a = 0, i = 0;
            do
            {
                var c = text[a]; i = (int)c;
                if ((c == '\n') || (c == '\r')) { px = 0; py += text_v_space; a++; continue; }
                if ((c < 33) || (c > 127)) { px += text_h_space; a++; continue; }
                w = fd[i].w * default_font_size; h = fd[i].h * default_font_size;
                if ((px + w) > right_most) right_most = px + w;
                if ((py + h) > bottom_most) bottom_most = py + h;
                px += (w / 2 + kern_space);
                a++;
            } while (a < text.Length);
            return (new Vector2(right_most, bottom_most)); 
        }

        
        
        // D R A W  S T R I N G
        /// <summary>
        /// creates a list of vertices and texture coordinates to use with the currently loaded font texture for message displays
        /// </summary>
        /// <param name="text">text message</param>
        /// <param name="pos">position</param>
        /// <param name="scale">scale(horz,vert)</param>
        /// <param name="color">tint color</param>
        public void DrawString(string text, Vector2 pos, Vector2 scale, Color color, Alignment align = Alignment.None)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            if (!font_tex_loaded) { Console.WriteLine("Font texture not loaded. Aborting DrawString."); return; }
            float px = pos.X, py = pos.Y;
            float spacing = fd[32].w * scale.X;
            float v_space = (fd[32].h - 2) * scale.Y;
            float kern_space = spacing / 4;
            float w, h, tx1, tx2, ty1, ty2;
            if (align != Alignment.None)
            {
                if (align == Alignment.LeftAlign) { pos.X = px = spacing; } // one space from left side of screen
                else
                {
                    float max_x = px, xx = px;
                    int q = 0;
                    do
                    {
                        if (max_x < xx) max_x = xx;
                        var c = text[q];
                        if ((c == '\n') || (c == '\r')) { xx = pos.X; q++; continue; }
                        if ((c < 33) || (c > 127)) { xx += spacing; q++; continue; }
                        int ii = (int)c; 
                        w = fd[ii].w * scale.X;
                        xx += (w / 2 + kern_space);
                        q++;
                    } while (q < text.Length);
                    float text_width = max_x - px;
                    if (align == Alignment.CenterAlign)
                    {
                        pos.X = screenWidth / 2 - text_width / 2; px = pos.X;
                    }
                    else if (align == Alignment.RightAlign)
                    {
                        pos.X = screenWidth - text_width - spacing; px = pos.X;
                    }
                }
            }
            int a = 0, i = 0;
            do
            {
                var c = text[a];
                i = (int)c;
                if ((c == '\n') || (c == '\r')) { px = pos.X; py += v_space; a++; continue; }
                if ((c < 33) || (c > 127)) { px += spacing; a++; continue; }
                w = fd[i].w * scale.X;
                h = fd[i].h * scale.Y;
                tx1 = fd[i].x1 / font_tex.Width; ty1 = fd[i].y1 / font_tex.Height;
                tx2 = fd[i].x2 / font_tex.Width; ty2 = fd[i].y2 / font_tex.Height;
                fontverts[font_vert_count].Position = new Vector3(px, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty1);  // upper-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px+w, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty1);  // upper-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px + w, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty2);  // lower-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty2);  // lower-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                if ((font_vert_count + 1) >= 8192) { //Flush 
                    EndFont(); beginCalled = true; font_vert_count=0;
                }
                px += (w / 2 + kern_space);
                a++;
            } while (a < text.Length);
        }



        public void DrawStringFast(string text, Vector2 pos, Color color)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            if (!font_tex_loaded) { Console.WriteLine("Font texture not loaded. Aborting DrawString."); return; }
            float px = pos.X + average_width * 0.5f, py = pos.Y;
            float spacing = (average_width + fd[32].iw) * default_font_size * 0.5f;
            float v_space = (fd[32].h - 2) * default_font_size;
            float w, h, tx1, tx2, ty1, ty2;
            float inv_width = 1.0f / font_tex.Width, inv_height = 1.0f / font_tex.Height;
            h = fd[32].h * default_font_size;
            int a = 0, i = 0;
            do
            {
                var c = text[a];
                i = (int)c;
                if ((c == '\n') || (c == '\r')) { px = pos.X; py += v_space; a++; continue; }
                if ((c < 33) || (c > 127)) { px += spacing; a++; continue; }
                w = fd[i].w * default_font_size;
                tx1 = fd[i].x1 * inv_width; ty1 = fd[i].y1 * inv_height;
                tx2 = fd[i].x2 * inv_width; ty2 = fd[i].y2 * inv_height;
                fontverts[font_vert_count].Position = new Vector3(px - w, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty1);  // upper-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px + w, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty1);  // upper-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px + w, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty2);  // lower-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px - w, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty2);  // lower-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                if ((font_vert_count + 1) >= 8192)
                { //Flush 
                    EndFont(); beginCalled = true; font_vert_count = 0;
                }
                px += spacing;
                a++;
            } while (a < text.Length);
        }

        public void DrawStringFastScaled(string text, Vector2 pos, float scale, Color color)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before Quadbatch Draw. Draw aborted."); return; }
            if (!font_tex_loaded) { Console.WriteLine("Font texture not loaded. Aborting DrawString."); return; }
            float px = pos.X + average_width * 0.5f * scale, py = pos.Y;
            float spacing = (average_width + fd[32].iw) * default_font_size * 0.5f * scale;
            float v_space = (fd[32].h - 2) * default_font_size * scale;
            float w, h, tx1, tx2, ty1, ty2;
            float inv_width = 1.0f / font_tex.Width, inv_height = 1.0f / font_tex.Height;
            h = fd[32].h * default_font_size * scale;
            int a = 0, i = 0;
            do
            {
                var c = text[a];
                i = (int)c;
                if ((c == '\n') || (c == '\r')) { px = pos.X; py += v_space; a++; continue; }
                if ((c < 33) || (c > 127)) { px += spacing; a++; continue; }
                w = fd[i].w * default_font_size * scale;
                tx1 = fd[i].x1 * inv_width; ty1 = fd[i].y1 * inv_height;
                tx2 = fd[i].x2 * inv_width; ty2 = fd[i].y2 * inv_height;
                fontverts[font_vert_count].Position = new Vector3(px - w, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty1);  // upper-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px + w, py, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty1);  // upper-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px + w, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx2, ty2);  // lower-right
                fontverts[font_vert_count].Color = color; font_vert_count++;
                fontverts[font_vert_count].Position = new Vector3(px - w, py + h, font_depth);
                fontverts[font_vert_count].TextureCoordinate = new Vector2(tx1, ty2);  // lower-left
                fontverts[font_vert_count].Color = color; font_vert_count++;
                if ((font_vert_count + 1) >= 8192)
                { //Flush 
                    EndFont(); beginCalled = true; font_vert_count = 0;
                }
                px += spacing;
                a++;
            } while (a < text.Length);
        }


        // DRAW LINE 
        private Rectangle pixel;
        public Rectangle PIXEL { get { return pixel; } set { pixel = value; pixel.Width = pixel.Height = 1;  } }

        public void DrawLine(Rectangle Pixel, Vector2 start, Vector2 end, Color color, float thickness = 2f)
        {
            Vector2 delta = end - start;
            float rot = (float)Math.Atan2(delta.Y, delta.X);
            Pixel.Width=Pixel.Height = 1;
            Draw(Pixel, start, new Vector2(0, 0.5f), new Vector2(delta.Length(), thickness), rot, color); 
        }

        public void Line(float x1, float y1, float x2, float y2, Color color, float thickness = 1f)
        {            
            Vector2 end = new Vector2(x2, y2);
            Vector2 start = new Vector2(x1, y1);
            Vector2 delta = end - start;
            float rot = (float)Math.Atan2(delta.Y, delta.X);            
            Draw(pixel, start, Vector2.Zero, new Vector2(delta.Length(), thickness), rot, color); 
        }


        public void Rect(Rectangle r, Color color, float thickness = 1f)
        {
            RectLines(r.X, r.Y, r.X + r.Width, r.Y + r.Height, color, thickness);
        }
        public void Rect(Rectangle r, Vector2 p, Color color, float thickness = 1f)
        {
            RectLines(p.X, p.Y, p.X + r.Width, p.Y + r.Height, color, thickness);
        }
        // Faster version: 
        public void RectLines(float x1, float y1, float x2, float y2, Color color, float thickness = 1f)
        {            
            Draw(pixel, new Vector2(x1, y1), Vector2.Zero, new Vector2((x2 - x1), thickness), 0f, color);
            Draw(pixel, new Vector2(x2, y1), Vector2.Zero, new Vector2(thickness, (y2 - y1)), 0f, color);
            Draw(pixel, new Vector2(x1, y2), Vector2.Zero, new Vector2((x2 - x1), thickness), 0f, color);
            Draw(pixel, new Vector2(x1, y1), Vector2.Zero, new Vector2(thickness, (y2 - y1)), 0f, color);
        }
        // LINE version of Rectangle drawing 
        public void Rect(float x1, float y1, float x2, float y2, Color color, float thickness = 1f)
        {            
            Draw(pixel, new Vector2(x1, y1), Vector2.Zero, new Vector2((x2-x1),thickness), 0f, color);
            Draw(pixel, new Vector2(x2, y1), Vector2.Zero, new Vector2((y2 - y1), thickness), -4.712389f, color);
            Draw(pixel, new Vector2(x2, y2), Vector2.Zero, new Vector2((x2 - x1), thickness), 3.1415926f, color);
            Draw(pixel, new Vector2(x1, y2), Vector2.Zero, new Vector2((y2 - y1), thickness), -1.570796f, color);
        }

        
        // DRAW COLOR QUAD
        public void DrawColorQuad(Rectangle Pixel, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c1, Color c2, Color c3, Color c4)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float u1, v1, u2, v2;
            u1 = Pixel.X / (float)tex.Width;
            v1 = Pixel.Y / (float)tex.Height;
            u2 = (Pixel.X + Pixel.Width) / (float)tex.Width;
            v2 = (Pixel.Y + Pixel.Height) / (float)tex.Height;
            vertices[vert_count].Position = new Vector3(p1, depth);  vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); // upper-left
            vertices[vert_count].Color = c1; vert_count++;
            vertices[vert_count].Position = new Vector3(p2, depth);  vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); // upper-right
            vertices[vert_count].Color = c2; vert_count++;
            vertices[vert_count].Position = new Vector3(p3, depth);  vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); // lower-right
            vertices[vert_count].Color = c3; vert_count++;
            vertices[vert_count].Position = new Vector3(p4, depth);  vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); // lower-left
            vertices[vert_count].Color = c4; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }

        // Draw filled rect using blank pixel: 
        public void DrawRect(Rectangle Pixel, Rectangle r, Color col)
        {
            if (!beginCalled) { Console.WriteLine("BEGIN not called before QuadBatch Draw. Draw aborted."); return; }
            float u1, v1, u2, v2;
            u1 = Pixel.X / (float)tex.Width;
            v1 = Pixel.Y / (float)tex.Height;
            u2 = (Pixel.X + Pixel.Width) / (float)tex.Width;
            v2 = (Pixel.Y + Pixel.Height) / (float)tex.Height;
            Vector2 p1, p2, p3, p4;
            p1.X = r.X; p1.Y = r.Y; p2 = p1; p2.X += r.Width; p3 = p2; p3.Y += r.Height; p4 = p1; p4.Y += r.Height;
            vertices[vert_count].Position = new Vector3(p1, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v1); // upper-left
            vertices[vert_count].Color = col; vert_count++;
            vertices[vert_count].Position = new Vector3(p2, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v1); // upper-right
            vertices[vert_count].Color = col; vert_count++;
            vertices[vert_count].Position = new Vector3(p3, depth); vertices[vert_count].TextureCoordinate = new Vector2(u2, v2); // lower-right
            vertices[vert_count].Color = col; vert_count++;
            vertices[vert_count].Position = new Vector3(p4, depth); vertices[vert_count].TextureCoordinate = new Vector2(u1, v2); // lower-left
            vertices[vert_count].Color = col; vert_count++;
            if ((vert_count + 1) >= 8192)
            {
                EndVerts(); beginCalled = true; vert_count = 0;
            }
        }


    }
}
