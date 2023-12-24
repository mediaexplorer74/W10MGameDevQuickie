// MeoMotion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GameManager
{
    // Holds info about sprite parts
    class SpritePart
    {
        public string name;
        public Rectangle rect;         // source rectangle on sprite sheet     
        public Vector2 pivot;          // pivot/origin to rotate around
        public int parent;             // what sprite was the parent (useful when programming vector tracking [like head looking or pointing weapon] )
        public Vector2 m1, m2, m3, m4; // model points around origin(0,0) before transforms
    }


    // Holds an individual key for a part in an animation
    class Key
    {
        public int part;               // which actual sprite part is to be rendered
        public int order;              // index for draw order
        public Vector2 scale;          // sprite size
        public float rot;              // sprite rotation 
        public Vector2 pos;            // sprite position
        public float alpha;            // transparency
        public Vector2 o1, o2, o3, o4; // vertex offsets/distortions
        public bool active;            // active (for hiding parts not animated)
    }


    // Holds an animation (and all key and part manipulation info) 
    class MeoAnimation
    {
        public string animation_name; // used when need to identify which animation index to use (using dictionary)
        public int num_keys;          // total keys in this animation sequence
        public bool looping;          // is a loop type animation? 
        public int[] times;           // time of key_index
        public Key[,] keys;           // keyframes (part index, key index)
        public int key1;              // current key
        public int key2;              // next key 
        public int root;              // index of root bone (could be useful) 
        public int start_part;        // section of parts this animation works on
        public int end_part;          // "                                      "
        public float timer;           // used in the example Play() method for interpolating between keys

        // add customization properties here:
        public float speed = 1.0f; // 100%
        public Vector2 offset = Vector2.Zero;
    }


    // Holds final rendering data for the Draw method
    class Final
    {
        public int order;              // which index to use ( drawing order )
        public int part;               // index of actual part/rect to render
        public float alpha;            // transparency
        public Vector2 v1, v2, v3, v4; // transformed vertices
        public bool hide;              // don't draw? 
    }


    class MeoMotion
    {
        ContentManager content;
        public QuadBatch batch;
        public Texture2D tex;
        public int num_parts, max_parts, total_animations; // Note: max_parts can be used elsewhere to allocate a memory pool for final renders (see NPC.cs)
        Final[] final = new Final[10];                 // used in final rendering of animated parts     
        public SpritePart[] parts = new SpritePart[10];     // sprite part list
        public MeoAnimation[] anim = new MeoAnimation[10];    // all the animations
        public Dictionary<string, int> lookup = new Dictionary<string, int>(); // used to find animation index of a named animation

        #region CONSTRUCTOR
        // C O N S T R U C T O R ------------------
        public MeoMotion(ContentManager _content, QuadBatch quadBatch)
        {
            content = _content;
            batch = quadBatch;
            max_parts = 0;
        }
        #endregion


        // L O A D  _  T X T -------------------------------------------------------------------------------------------------------------
        /// NEED TO: place the export .TXT file in your project (in BIN and then in x86 and then in debug - or whatever your build place is)
        /// -- apon final release of your project you'll need to make sure this exported file is in the same folder as the executable
        /// [ You can specify export folders in MeoMotion using export options ]
        // rescale - to resize the how big the sprites will appear
        public async void Load_TXT(string Filename, Vector2 rescale, bool adjustOrder = false)
        {
            Windows.Storage.StorageFolder AppStorageFolder =
               // Windows.Storage.ApplicationData.Current.LocalFolder;
               Windows.ApplicationModel.Package.Current.InstalledLocation;

            int a = 0;

            if (Filename.EndsWith(".txt")) { } else Filename += ".txt";
            
            if (!File.Exists(Filename)) 
            { 
                Debug.WriteLine("[ex] File not found: " + Filename); 
                return; 
            }

            StorageFile some_file = await AppStorageFolder.GetFileAsync(Filename);

            using (FileStream some_fs = new FileStream(some_file.Path, FileMode.OpenOrCreate))
            {

                using (StreamReader reader = new StreamReader(/*Filename*/some_fs))
                {
                    string anim_name = "";
                    a = -1;
                    string line = reader.ReadLine(); string[] strs;
                    strs = line.Split(',');
                    if ((strs[0] != "SPRITESHEET_FILENAME") && (strs[0] != "COMBO_FILENAME"))
                    { 
                        Debug.WriteLine("Data=" + strs[0]); 
                        
                        Debug.WriteLine("\n Unexpected first line in " 
                            + Filename + 
                            " while trying to load as .TXT MeoMotion file."); 
                        
                        return; 
                    }
                    string image_filename = Path.GetFileName(strs[1]);
                    image_filename = Path.GetFileNameWithoutExtension(image_filename);

                    tex = content.Load<Texture2D>(image_filename);  // load actual spritemap
                                                                    // continue loadint txt:
                    int current_root = 0, part_count = 0;
                    if (strs[0] == "COMBO_FILENAME")
                    {
                        num_parts = Convert.ToInt32(strs[4]);
                        total_animations = Convert.ToInt32(strs[5]);
                    }
                    else
                    {
                        line = reader.ReadLine(); strs = line.Split(','); // reading TOTAL_NUM_PARTS
                        num_parts = Convert.ToInt32(strs[1]); part_count = num_parts; 

                        if (part_count > max_parts) 
                            max_parts = part_count;

                        line = reader.ReadLine(); strs = line.Split(','); // reading ROOT_INDEX
                        current_root = Convert.ToInt32(strs[1]);
                        line = reader.ReadLine(); strs = line.Split(','); // reading TOTAL_ANIMATIONS
                        total_animations = Convert.ToInt32(strs[1]);
                    }
                    parts = new SpritePart[num_parts];                   //allocate parts
                    anim = new MeoAnimation[total_animations];           //allocate animations   

                    int p = 0, pi = -1, first_index = 0;  // part index, timeline part index, first index within a sheet section
                    int k = 0;                            // key index
                    string current_sheetname = "";        // used when sheet data has been combined into one sheet
                                                          // (old sheet names - used for looking up animations which
                                                          // use different sections of a spritemap) 
                    bool first = false;
                    do
                    {
                        line = reader.ReadLine(); strs = line.Split(',');
                        switch (strs[0])
                        {
                            case "SPRITESHEET_FILENAME": 
                                current_sheetname = Path.GetFileNameWithoutExtension(strs[1]); 
                                first = true; 
                                break; // SET FIRST so we know to set the first index
                            case "TOTAL_NUM_PARTS": 
                                part_count = Convert.ToInt32(strs[1]); 
                                if (part_count > max_parts) max_parts = part_count; 
                                break;

                            case "ROOT_INDEX": 
                                current_root = Convert.ToInt32(strs[1]); 
                                break;

                            case "PART_INDEX":
                                p = Convert.ToInt32(strs[1]); 
                                if (p >= num_parts) 
                                { 
                                    Debug.WriteLine("Meo file integrity problem: part index p>=part_count"); 
                                    return; 
                                }
                                if (first) 
                                { 
                                    first = false; 
                                    first_index = p; 
                                } // record the first index if this is the first entry of this sheet section 
                                break;

                            case "PART_NAME": 
                                parts[p] = new SpritePart(); parts[p].name = strs[1]; 
                                break;

                            case "PART_RECTANGLE": 
                                parts[p].rect.X = Convert.ToInt32(strs[1]); 
                                parts[p].rect.Y = Convert.ToInt32(strs[2]); 
                                parts[p].rect.Width = Convert.ToInt32(strs[3]); 
                                parts[p].rect.Height = Convert.ToInt32(strs[4]); 
                                break;

                            case "LOCAL_POINTS_M1M2M3M4":
                                parts[p].m1.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                parts[p].m1.Y = Convert.ToSingle(strs[2]) * rescale.Y;
                                parts[p].m2.X = Convert.ToSingle(strs[3]) * rescale.X; 
                                parts[p].m2.Y = Convert.ToSingle(strs[4]) * rescale.Y;
                                parts[p].m3.X = Convert.ToSingle(strs[5]) * rescale.X; 
                                parts[p].m3.Y = Convert.ToSingle(strs[6]) * rescale.Y;
                                parts[p].m4.X = Convert.ToSingle(strs[7]) * rescale.X; 
                                parts[p].m4.Y = Convert.ToSingle(strs[8]) * rescale.Y;
                                parts[p].pivot = -parts[p].m1; 
                                break;

                            case "PART_PIVOT": 
                                break; // version of pivot used in original (not used in game) 

                            case "PART_PARENT": 
                                parts[p].parent = Convert.ToInt32(strs[1]) + first_index; 
                                break; // offset by first index
                                                //------------------------------------------------------------------------
                            case "ANIMATION_NAME": anim_name = strs[1]; 
                                break;
                            case "ANIMATION_NUMBER":
                                a++;
                                if (a >= total_animations) 
                                { 
                                    Debug.WriteLine("Error: animation index a>=total_animations"); 
                                    return; 
                                }
                                anim[a] = new MeoAnimation();
                                anim[a].animation_name = anim_name;
                                anim[a].looping = false;
                                anim[a].root = current_root + first_index;  // offset by first index
                                anim[a].start_part = first_index;
                                anim[a].end_part = first_index + part_count;
                                lookup.Add(current_sheetname + anim_name, a);
                                anim[a].key1 = 0; anim[a].key2 = 0; anim[a].timer = 0; pi = -1; k = 0;
                                break;

                            case "ANIMATION_KEY_COUNT":
                                anim[a].num_keys = Convert.ToInt32(strs[1]);
                                anim[a].keys = new Key[num_parts, anim[a].num_keys]; // allocate keys for this animation
                                anim[a].times = new int[anim[a].num_keys];            // allocate times
                                break;

                            case "KEY": 
                                k = Convert.ToInt32(strs[1]); pi = -1; 
                                break;

                            case "LOOPING": anim[a].looping = true; 
                                break;
                            case "TIME": 
                                anim[a].times[k] = Convert.ToInt32(strs[1]); 
                                break;
                            case "PART":
                                pi++; anim[a].keys[pi, k] = new Key();
                                anim[a].keys[pi, k].part = Convert.ToInt32(strs[1]) + first_index; // offset by first_index
                                anim[a].keys[pi, k].active = true; 
                                break;

                            case "ORDER":
                                anim[a].keys[pi, k].order = Convert.ToInt32(strs[1]);
                                if (adjustOrder) anim[a].keys[pi, k].order += first_index;
                                break;

                            case "NOT_ACTIVE": anim[a].keys[pi, k].active = false; 
                                break;
                            case "K_SCALE": anim[a].keys[pi, k].scale.X = Convert.ToSingle(strs[1]); 
                                anim[a].keys[pi, k].scale.Y = Convert.ToSingle(strs[2]); 
                                break;
                            case "K_ROT": anim[a].keys[pi, k].rot = Convert.ToSingle(strs[1]); 
                                break;
                            case "K_POS": anim[a].keys[pi, k].pos.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                anim[a].keys[pi, k].pos.Y = Convert.ToSingle(strs[2]) * rescale.Y; 
                                break;
                            case "K_ALPHA": anim[a].keys[pi, k].alpha = Convert.ToSingle(strs[1]); 
                                break;
                            case "K_VERT_OFF1": anim[a].keys[pi, k].o1.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                anim[a].keys[pi, k].o1.Y = Convert.ToSingle(strs[2]) * rescale.Y; 
                                break;
                            case "K_VERT_OFF2": anim[a].keys[pi, k].o2.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                anim[a].keys[pi, k].o2.Y = Convert.ToSingle(strs[2]) * rescale.Y; 
                                break;
                            case "K_VERT_OFF3": anim[a].keys[pi, k].o3.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                anim[a].keys[pi, k].o3.Y = Convert.ToSingle(strs[2]) * rescale.Y; 
                                break;
                            case "K_VERT_OFF4": anim[a].keys[pi, k].o4.X = Convert.ToSingle(strs[1]) * rescale.X; 
                                anim[a].keys[pi, k].o4.Y = Convert.ToSingle(strs[2]) * rescale.Y; 
                                break;
                        }
                    } while (reader.EndOfStream != true);
                }//using reader
            }

            if (total_animations > a) total_animations = a;
            //precautionary setting of second key (in case no other keys):
            a = 0;
            do 
            { 
                if (anim[a].num_keys > 1) anim[a].key2 = 1; else anim[a].key2 = 0; a++; 
            } 
            while (a < total_animations);

            final = new Final[num_parts];
            a = 0;
            do 
            { 
                final[a] = new Final(); a++; 
            } 
            while (a < num_parts);
        }// Load_TXT


        // G E T  I N D E X --------------------------------------
        // finds the animation index of a named animation
        public int GetIndex(string sheetname, string animation_name, bool show_error_messages = true)
        {
            int value;
            if (!lookup.TryGetValue(sheetname + animation_name, out value)) 
            {
                if (show_error_messages) 
                    Debug.WriteLine("Animation not found in dictionary: " 
                    + sheetname + animation_name);
                return 0;
            }
            return value; 
        }


        // ADJUST SCALE FOR SCREEN RESOLUTION
        // a tool to scale position, size, and speed of characters to match display resolutions
        public void Adjust_Scale_For_ScreenResolution(ref Vector2 rescale, int width, int height) 
        {
            // Assuming default is 800 x 600 (can change this)
            float DEFAULT_W = 800, DEFAULT_H = 600;
            rescale.X = (rescale.X * width) / DEFAULT_W;
            rescale.Y = (rescale.Y * height) / DEFAULT_H;
        }

    }
}
