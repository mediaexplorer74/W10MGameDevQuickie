using FbonizziMonoGame.Assets;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameManager.Assets
{
    public class AssetsLoader
    {
        private readonly ContentManager _contentManager;

        public Texture2D Textures { get; private set; }
        public Texture2D TexturesBuildingsBack { get; private set; }
        public Texture2D TexturesBuildingsMid { get; private set; }
        public Texture2D TexturesBuildingsGround { get; private set; }
        public List<Rectangle> PlayerRunRects { get; private set; }
        public List<Rectangle> PlayerIdleRects { get; private set; }
        public List<Rectangle> PlayerFallRects { get; private set; }
        public List<Rectangle> PlayerFartRects { get; private set; }
        public List<Rectangle> PlayerMerdaRects { get; private set; }

        // In this class there are two different ways to load textures/sprites,
        // because this is an old project that I reworked to make it build with MonoGame

        public IDictionary<string, Sprite> OtherSprites { get; private set; }

        public SpriteFont Font { get; private set; }

        // Reserved
        public IDictionary<string, SoundEffect> EnvironmentalSounds { get; private set; }
        public IDictionary<string, SoundEffect> SpecialSounds { get; private set; }
        public IDictionary<string, Rectangle> TexturesRectangles { get; private set; }


        public IDictionary<AnimationsNames, SpriteAnimation> Animations { get; }
            = new Dictionary<AnimationsNames, SpriteAnimation>();
        public IDictionary<string, Sprite> Sprites { get; } 
            = new Dictionary<string, Sprite>();
        public IDictionary<SoundsNames, SoundEffect> Sounds { get; } 
            = new Dictionary<SoundsNames, SoundEffect>();
        
       
        public enum AnimationsNames
        {
            PlayerRun,
            PlayerJump,
            PlayerDeath,
            GoodGlow,
            BadGlow
        }

        public enum SoundsNames
        {
            running,
            slideshow,
            takegem,
            die,
            menu
        }

        private readonly CustomSpriteImporter _textureImporter;
     
        private ContentManager content;

        // PLAN A
        public AssetsLoader(ContentManager contentManager, CTextFileLoader fileLoader)
        {
            //RnD 
            if (fileLoader == null)
            {
                Debug.WriteLine("AssetsLoader -- fileLoader bug:  ArgumentNullException");
                throw new ArgumentNullException(nameof(fileLoader));
            }

            _contentManager = contentManager ?? 
                throw new ArgumentNullException(nameof(contentManager));

            //Experimental
            _textureImporter = new CustomSpriteImporter(fileLoader);
        }

        // PLAN B
        public AssetsLoader(ContentManager contentManager)
        {
            _contentManager = contentManager; //??throw new ArgumentNullException(nameof(contentManager));

            //_textureImporter = new CustomSpriteImporter(fileLoader);
        }

        public void LoadResources()
        {
            Font = _contentManager.Load<SpriteFont>("TextFont");//("Font/Starfall-Regular");

            //RnD
            Textures = _contentManager.Load<Texture2D>("textures");

            //RnD
            TexturesBuildingsBack = _contentManager.Load<Texture2D>("buildings_back");
            TexturesBuildingsMid = _contentManager.Load<Texture2D>("buildings_mid");
            TexturesBuildingsGround = _contentManager.Load<Texture2D>("buildings_ground");

            //RnD
            OtherSprites = new Dictionary<string, Sprite>();


            CreateSprites();
            //RnD
            CreateAnimations();

            CreateSounds();
        }

        private void CreateSprites()
        {
            // RnD txt content

            //const string backgroundSpriteSheetName = "backgrounds";
            var backroundSpriteSheet = _contentManager.Load<Texture2D>(
                "SpriteSheets/backgrounds.png");

            IDictionary<string, SpriteDescription> backgroundSpritesDescriptions =
                _textureImporter.Import("SpriteSheets/backgrounds.txt");

            const string othersSpriteSheetName = "others";
            var othersSpriteSheet = _contentManager.Load<Texture2D>(
                $"SpriteSheets/others.png");
            
            IDictionary<string, SpriteDescription> othersSpritesDescriptions =
                _textureImporter.Import("SpriteSheets/others.txt");

            const string protipsSpriteSheetName = "protips";
            var protipsSpriteSheet = _contentManager.Load<Texture2D>(
                $"SpriteSheets/protips.png");
            
            IDictionary<string, SpriteDescription> protipsSpritesDescription =
                _textureImporter.Import("SpriteSheets/protips.txt");

            AddSpritesFromDictionary(backgroundSpritesDescriptions, backroundSpriteSheet);
            AddSpritesFromDictionary(othersSpritesDescriptions, othersSpriteSheet);
            AddSpritesFromDictionary(protipsSpritesDescription, protipsSpriteSheet);
        }

        private void CreateSounds()
        {
            Sounds.Add(SoundsNames.running, _contentManager.Load<SoundEffect>("Music/Running.wav"));
            Sounds.Add(SoundsNames.menu, _contentManager.Load<SoundEffect>("Music/menu.wav"));
            Sounds.Add(SoundsNames.slideshow, _contentManager.Load<SoundEffect>("Music/Slideshow.wav"));
            Sounds.Add(SoundsNames.takegem, _contentManager.Load<SoundEffect>("Music/TakeGem.wav"));
            Sounds.Add(SoundsNames.die, _contentManager.Load<SoundEffect>("Music/Die.wav"));
        }

        private void CreateAnimations()
        {
            List<Sprite> runAnimation = new List<Sprite>()
            {
                Sprites["run_000"],
                Sprites["run_001"],
                Sprites["run_002"],
                Sprites["run_003"],
                Sprites["run_004"],
                Sprites["run_005"],
                Sprites["run_006"],
                Sprites["run_007"],
                Sprites["run_008"],
                Sprites["run_009"],
                Sprites["run_010"],
                Sprites["run_011"],
                Sprites["run_012"],
                Sprites["run_013"],
                Sprites["run_014"],
                Sprites["run_015"],
                Sprites["run_016"],
                Sprites["run_017"],
                Sprites["run_018"],
                Sprites["run_019"]
            };

            List<Sprite> jumpAnimation = new List<Sprite>()
            {
                Sprites["jump_000"],
                Sprites["jump_001"],
                Sprites["jump_002"],
                Sprites["jump_003"],
                Sprites["jump_004"],
                Sprites["jump_005"],
                Sprites["jump_006"],
                Sprites["jump_007"],
                Sprites["jump_008"],
                Sprites["jump_009"],
                Sprites["jump_010"],
                Sprites["jump_011"],
                Sprites["jump_012"],
                Sprites["jump_013"],
                Sprites["jump_014"],
                Sprites["jump_015"],
                Sprites["jump_016"],
                Sprites["jump_017"],
                Sprites["jump_018"],
                Sprites["jump_019"]
            };

            List<Sprite> goodGlowAnimation = new List<Sprite>()
            {
                Sprites["good_glow_000"],
                Sprites["good_glow_001"],
                Sprites["good_glow_002"],
                Sprites["good_glow_003"],
                Sprites["good_glow_004"],
                Sprites["good_glow_005"],
                Sprites["good_glow_006"],
                Sprites["good_glow_007"],
                Sprites["good_glow_008"],
                Sprites["good_glow_009"],
                Sprites["good_glow_010"],
                Sprites["good_glow_011"],
                Sprites["good_glow_012"],
                Sprites["good_glow_013"],
                Sprites["good_glow_014"],
                Sprites["good_glow_015"],
                Sprites["good_glow_016"],
                Sprites["good_glow_017"],
                Sprites["good_glow_018"],
                Sprites["good_glow_019"]
            };

            List<Sprite> badGlowAnimation = new List<Sprite>()
            {
                Sprites["bad_glow_000"],
                Sprites["bad_glow_001"],
                Sprites["bad_glow_002"],
                Sprites["bad_glow_003"],
                Sprites["bad_glow_004"],
                Sprites["bad_glow_005"],
                Sprites["bad_glow_006"],
                Sprites["bad_glow_007"],
                Sprites["bad_glow_008"],
                Sprites["bad_glow_009"],
                Sprites["bad_glow_010"],
                Sprites["bad_glow_011"],
                Sprites["bad_glow_012"],
                Sprites["bad_glow_013"],
                Sprites["bad_glow_014"],
                Sprites["bad_glow_015"],
                Sprites["bad_glow_016"],
                Sprites["bad_glow_017"],
                Sprites["bad_glow_018"],
                Sprites["bad_glow_019"]
            };

            List<Sprite> playerDeathAnimation = new List<Sprite>()
            {
                Sprites["death_000"],
                Sprites["death_001"],
                Sprites["death_002"],
                Sprites["death_003"],
                Sprites["death_004"],
                Sprites["death_005"],
                Sprites["death_006"],
                Sprites["death_007"],
                Sprites["death_008"],
                Sprites["death_009"],
                Sprites["death_010"],
                Sprites["death_011"],
                Sprites["death_012"],
                Sprites["death_013"],
                Sprites["death_014"],
                Sprites["death_015"],
                Sprites["death_016"],
                Sprites["death_017"],
                Sprites["death_018"],
                Sprites["death_019"]
            };

            Animations[AnimationsNames.PlayerRun] = 
                new SpriteAnimation(runAnimation, TimeSpan.FromMilliseconds(20));
            Animations[AnimationsNames.PlayerJump] = 
                new SpriteAnimation(jumpAnimation, TimeSpan.FromMilliseconds(0.6));
            Animations[AnimationsNames.GoodGlow] = 
                new SpriteAnimation(goodGlowAnimation, TimeSpan.FromMilliseconds(400));
            Animations[AnimationsNames.BadGlow] = 
                new SpriteAnimation(badGlowAnimation, TimeSpan.FromMilliseconds(100));
            Animations[AnimationsNames.PlayerDeath] = 
                new SpriteAnimation(playerDeathAnimation, TimeSpan.FromMilliseconds(30), false);
        }

        private void AddSpritesFromDictionary(
            IDictionary<string, SpriteDescription> textureDictionary,
            Texture2D spriteSheet)
        {
            //RnD
            if (textureDictionary != null)
            {
                foreach (KeyValuePair<string, SpriteDescription> texture in textureDictionary)
                {
                    Sprites.Add(texture.Key, new Sprite(
                        texture.Value,
                        spriteSheet));
                }
            }
        }

    }
}
