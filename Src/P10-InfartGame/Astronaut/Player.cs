using GameManager.Assets;
using GameManager.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GameManager.Drawing;

namespace GameManager.Astronaut
{
    public class Player : Actor
    {
        private readonly ScoreggiaParticleSystem _scoreggiaSystem;
        private readonly JalapenoParticleSystem _jalapenoSystem;
        private readonly BroccoloParticleSystem _broccoloSystem;
        private readonly BeanParticleSystem _beanSystem;
        private readonly InfartGame _gameManagerReference;

        private const double TimeBetweenNewParticleScoregge = 30.0f;
        private double _timeTillNewParticleScoregge = 0.0f;
        private const double TimeBetweenNewParticleJalapeno = 20.0f;
        private const double TimeBetweenNewParticleBean = 0f;
        private double _timeTillNewParticleJalapeno = 0.0f;
        private const double TimeBetweenNewParticleBroccolo = 80.0f;
        private double _timeTillNewParticleBroccolo = 0.0f;
        private bool _allowInput = true;
        private int _jalapenosJumpCount = 0;
        private int _noPowerUpsJumpCount;
        private bool _broccolo = false;
        private double _elapsedJalapenos = 0.0;
        private double _elapsedBroccolo = 0.0;
        private double _elapsedBean;
        private Color _fillColor;
        private bool _bean;
        private double _timeTillNewParticleBean;

        public Player(
            Vector2 startingPos,
            AssetsLoader assetsLoader,
            InfartGame gameManagerReference)
            : base(1.0f, startingPos, gameManagerReference.GroundObjects())
        {
            _fillColor = _overlayColor;
            Position = startingPos;
            HorizontalMoveSpeed = 300f;
            _origin = Vector2.Zero;

            _gameManagerReference = gameManagerReference;

            _scoreggiaSystem = new ScoreggiaParticleSystem(10, assetsLoader.Textures, assetsLoader.TexturesRectangles["ScoreggiaParticle"]);
            _jalapenoSystem = new JalapenoParticleSystem(10, assetsLoader);
            _broccoloSystem = new BroccoloParticleSystem(8, assetsLoader);
            _beanSystem = new BeanParticleSystem(10, assetsLoader);

            LoadAnimation("idle", assetsLoader.PlayerIdleRects,
                true, 0.1f, assetsLoader.Textures);

            LoadAnimation("run", assetsLoader.PlayerRunRects,
                true, 0.05f, assetsLoader.Textures);

            LoadAnimation("fall", assetsLoader.PlayerFallRects,
                true, 0.01f, assetsLoader.Textures);

            LoadAnimation("fart_sustain_up", assetsLoader.PlayerFartRects,
                true, 0.05f, assetsLoader.Textures);

            LoadAnimation("truck", assetsLoader.PlayerMerdaRects,
                true, 0.01f, assetsLoader.Textures);

            PlayAnimation("fall");
        }

        protected void LoadAnimation(
            string name,
            List<Rectangle> frames,
            bool loopAnimation,
            float frameLenght,
            Texture2D textureReference)
        {
            Animations.Add(
               name,
               new AnimationManager(
                    frames,
                    name,
                    textureReference
               ));
            Animations[name].LoopAnimation = loopAnimation;
            Animations[name].FrameLength = frameLenght;
        }

        public void Reset(Vector2 position)
        {
            ((GameObject)this).Position = position;
            Dead = false;
            JalapenosJump = false;
            _jalapenosJumpCount = 0;
            _broccolo = false;
            _bean = false;
            _elapsedJalapenos = 0.0;
            _elapsedBroccolo = 0.0;
            _fillColor = Color.White;
            HorizontalMoveSpeed = 300f;
            Animations["run"].FrameLength = 0.05f;
            Velocity = Vector2.Zero;
            PlayAnimation("fall");
        }

        public bool JalapenosJump { get; private set; } = false;

        public void Jump(float amount)
        {
            Velocity.Y = -amount;

            _gameManagerReference.PlayerJumped();
            _gameManagerReference.AddScoreggia();
        }

        public void IncreaseMoveSpeed()
        {
            XMoveSpeed += 40.0f;
            Animations["run"].FrameLength -= 0.005f;
        }

        public bool AllowInput
        {
            set { _allowInput = value; }
        }

        public void ActivateJalapenos()
        {
            Jump(500);
            JalapenosJump = true;
            _elapsedJalapenos = 0.0;
            HorizontalMoveSpeed += 200.0f;
            _fillColor = Color.DarkRed;
            _gameManagerReference.IncreaseParallaxSpeed();
            _jalapenosJumpCount = 0;
        }

        public void DeactivateJalapenos()
        {
            _elapsedJalapenos = 0.0;
            JalapenosJump = false;
            _gameManagerReference.JalapenosModeActive = false;
            HorizontalMoveSpeed -= 200.0f;
            _fillColor = _overlayColor;
            _gameManagerReference.DecreaseParallaxSpeed();
        }

        public void ActivateBroccolo()
        {
            _elapsedBroccolo = 0.0;
            _fillColor = Color.Green;
            _broccolo = true;
            Jump(200);
            HorizontalMoveSpeed += 400.0f;

            _gameManagerReference.IncreaseParallaxSpeed();
            _gameManagerReference.IncreaseParallaxSpeed();
        }

        public void ActivateBean()
        {
            _elapsedBean = 0.0;
            _fillColor = Color.Brown;
            _bean = true;
            Jump(300);
        }

        public void DeactivateBroccolo()
        {
            _broccolo = false;
            _gameManagerReference.MerdaModeActive = false;
            _elapsedBroccolo = 0.0;
            HorizontalMoveSpeed -= 400.0f;
            _fillColor = _overlayColor;
            _gameManagerReference.DecreaseParallaxSpeed();
            _gameManagerReference.DecreaseParallaxSpeed();
        }

        public void DeactivateBean()
        {
            _bean = false;
            _gameManagerReference.BeanModeActive = false;
            _elapsedBean = 0.0;
            _fillColor = _overlayColor;
            _gameManagerReference.DecreaseParallaxSpeed();
            _gameManagerReference.DecreaseParallaxSpeed();
        }

        public void HandleInput()
        {
            if (JalapenosJump)
            {
                if (_jalapenosJumpCount < 2)
                {
                    ++_jalapenosJumpCount;
                    Jump(800);
                }
            }
            else if (_broccolo)
            {
                Jump(500);
            }
            else if(_bean)
            {
                Jump(500);
            }
            else if (_noPowerUpsJumpCount < 1)
            {
                ++_noPowerUpsJumpCount;
                Jump(650);
            }
        }

        public override void Update(double dt)
        {
            if (!Dead)
            {
                string newAnimation = "run";

                if (_allowInput)
                {
                    FlipEffect = SpriteEffects.None;
                    Velocity.X = +HorizontalMoveSpeed;
                }
                else
                {
                    newAnimation = "idle";
                }

                if (!OnGround)
                {
                    if (Velocity.Y < 0)
                    {
                        newAnimation = "fart_sustain_up";
                    }
                    else if (CurrentAnimation == "fart_sustain_up" || CurrentAnimation == "fall")
                    {
                        newAnimation = "fall";
                    }
                }
                else
                {
                    _noPowerUpsJumpCount = 0;
                }

                if (_broccolo)
                    newAnimation = "truck";

                if (newAnimation != CurrentAnimation)
                {
                    PlayAnimation(newAnimation);
                    if (newAnimation == "truck")
                        _collisionRectangle.Width -= 120;
                }

                _scoreggiaSystem.Update(dt);
                _jalapenoSystem.Update(dt);
                _broccoloSystem.Update(dt);
                _beanSystem.Update(dt);

                if (JalapenosJump && !_broccolo && !_bean)
                    JalapenoGeneration(dt);
                else if (!JalapenosJump && _broccolo && !_bean)
                    BroccoloGeneration(dt);
                else if (!JalapenosJump && _bean && !_broccolo)
                    BeanGeneration(dt);
                else
                    ScoreggiaGeneration(dt);

                if (!Dead)
                {
                    _gameManagerReference.ScoreMetri = ((int)(((GameObject)this).Position.X / 100));
                }
            }

            base.Update(dt);
        }

        public bool IsFalling => CurrentAnimation == "fall";

        private void JalapenoGeneration(double dt)
        {
            if (OnGround)
            {
                _jalapenosJumpCount = 0;
            }

            _elapsedJalapenos += dt;
            if (_elapsedJalapenos >= _gameManagerReference.PeperoncinoDuration)
                DeactivateJalapenos();

            _timeTillNewParticleJalapeno -= dt;
            if (_timeTillNewParticleJalapeno < 0)
            {
                Vector2 where = ((GameObject)this).Position + new Vector2(
                   Animations[CurrentAnimation].FrameWidth / 3,
                   (Animations[CurrentAnimation].FrameHeight / 2) + 30);

                _jalapenoSystem.AddParticles(where);

                _timeTillNewParticleJalapeno = TimeBetweenNewParticleJalapeno;
            }
        }

        private void BeanGeneration(double dt)
        {
            _elapsedBean += dt;
            if (_elapsedBean >= _gameManagerReference.BeanDuration)
                DeactivateBean();

            _timeTillNewParticleBean -= dt;
            if (_timeTillNewParticleBean < 0)
            {
                Vector2 where = ((GameObject)this).Position + new Vector2(
                   Animations[CurrentAnimation].FrameWidth / 3,
                   (Animations[CurrentAnimation].FrameHeight / 2) + 30);

                _beanSystem.AddParticles(where);

                _timeTillNewParticleBean = TimeBetweenNewParticleBean;
            }
        }

        private void BroccoloGeneration(double dt)
        {
            _elapsedBroccolo += dt;
            if (_elapsedBroccolo >= _gameManagerReference.BroccoloDuration)
                DeactivateBroccolo();

            _timeTillNewParticleBroccolo -= dt;
            if (_timeTillNewParticleBroccolo < 0)
            {
                Vector2 where = ((GameObject)this).Position + new Vector2(
                   Animations[CurrentAnimation].FrameWidth / 3,
                   (Animations[CurrentAnimation].FrameHeight / 2) + 30);

                _broccoloSystem.AddParticles(where);

                _timeTillNewParticleBroccolo = TimeBetweenNewParticleBroccolo;
            }
        }

        private void ScoreggiaGeneration(double dt)
        {
            if (Velocity.Y < 0)
            {
                _timeTillNewParticleScoregge -= dt;
                if (_timeTillNewParticleScoregge < 0)
                {
                    Vector2 where = ((GameObject)this).Position + new Vector2(
                       Animations[CurrentAnimation].FrameWidth / 3,
                       (Animations[CurrentAnimation].FrameHeight / 2) + 30);

                    _scoreggiaSystem.AddParticles(where);

                    _timeTillNewParticleScoregge = TimeBetweenNewParticleScoregge;
                }
            }
            else
                _gameManagerReference.StopScoreggia();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Animations.ContainsKey(CurrentAnimation))
            {
                spriteBatch.Draw(
                    Animations[CurrentAnimation].Texture,
                    ((GameObject)this).Position,
                    Animations[CurrentAnimation].FrameRectangle,
                    _fillColor,
                    _rotation,
                    _origin,
                    _scale,
                    _flip,
                    _depth);
            }
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                _scoreggiaSystem.Draw(spriteBatch);
                _jalapenoSystem.Draw(spriteBatch);
                _broccoloSystem.Draw(spriteBatch);
            }
        }

        public void DrawBeanParticles(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                _beanSystem.Draw(spriteBatch);
            }
        }

    }
}