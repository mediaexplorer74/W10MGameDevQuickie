﻿using System;
using GameManager.Classes;
using GameManager.Components;
using GameManager.Enums;
using Microsoft.Xna.Framework;

namespace GameManager.Common
{
    public class ReportPostionEventArgs : EventArgs
    {
        public ReportPostionEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }

    public abstract class BaseActor : BaseDrawableGameComponent
    {
        private readonly int _speedCoef;

        private SpriteTypeEnum _heroSprite = SpriteTypeEnum.Space;
        private bool _isFalling;
        private TimeSpan _moveInterval;

        protected BaseActor(Game1 game, Room room, int speedCoef)
            : base(game)
        {
            Room = room;
            Visible = false;
            Enabled = false;
            _speedCoef = speedCoef;
        }

        protected DirectionEnum Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        protected Room Room { get; private set; }
        public event EventHandler<ReportPostionEventArgs> OnReportPostion;

        protected virtual void Reset(int x, int y)
        {
            X = x;
            Y = y;
            _isFalling = false;
            Direction = DirectionEnum.None;
            _moveInterval = new TimeSpan();
        }

        private bool HasNoStrongHold(SpriteTypeEnum sprite, bool isFalling)
        {
            bool result = Maps.IsBiomass(sprite)
                          || Maps.IsLiveWall(sprite)
                          || Maps.IsSpace(sprite)
                          || Maps.IsChest(sprite);
            if (isFalling && !result)
                result = Maps.IsFloor(sprite);
            return result;
        }

        protected virtual bool ProcessFalling(bool isFailing)
        {
            SpriteTypeEnum cur1 = Room[X, Y];
            SpriteTypeEnum cur2 = Room[X + 1, Y];
            SpriteTypeEnum pos1 = Room[X, Y + 1];
            SpriteTypeEnum pos2 = Room[X + 1, Y + 1];

            bool result;

            if (isFailing) 
            {
                result = HasNoStrongHold(pos1, true) && HasNoStrongHold(pos2, true)
                         && !(Maps.IsBiomass(cur1) && Maps.IsBiomass(cur2));
            }
            else
                result = HasNoStrongHold(pos1, false) && HasNoStrongHold(pos2, false)
                         && !(Maps.IsStairs(cur1) || Maps.IsStairs(cur2));

            if (result)
                Y++;
            return result;
        }

        protected virtual bool CanMove(DirectionEnum direction)
        {
            bool result = false;
            switch (direction)
            {
                case DirectionEnum.Up:
                {
                    if (Y == 0)
                        break;
                    SpriteTypeEnum cur1 = Room[X, Y];
                    SpriteTypeEnum cur2 = Room[X + 1, Y];
                    SpriteTypeEnum pos1 = Room[X, Y - 1];
                    SpriteTypeEnum pos2 = Room[X + 1, Y - 1];
                    result = ((Maps.IsStairs(cur1) && Maps.IsStairs(cur2)) && !(Maps.IsWall(pos1) || Maps.IsWall(pos2)));
                    break;
                }
                case DirectionEnum.Down:
                {
                    if (Y == 16)
                        break;
                    SpriteTypeEnum pos1 = Room[X, Y + 1];
                    SpriteTypeEnum pos2 = Room[X + 1, Y + 1];
                    result = (HasNoStrongHold(pos1, false) && HasNoStrongHold(pos2, false)) ||
                             (Maps.IsStairs(pos1) && Maps.IsStairs(pos2));
                    break;
                }
                case DirectionEnum.Left:
                {
                    if (X == 0)
                        break;
                    SpriteTypeEnum pos = Room[X - 1, Y];
                    result = !Maps.IsWall(pos);
                    break;
                }
                case DirectionEnum.Right:
                {
                    if (X == 49)
                        break;
                    SpriteTypeEnum pos = Room[X + 2, Y];
                    result = !Maps.IsWall(pos);
                    break;
                }
            }
            return result;
        }

        protected virtual bool Move(DirectionEnum direction)
        {
            bool result = true;
            switch (direction)
            {
                case DirectionEnum.Left:
                {
                    if (CanMove(DirectionEnum.Left))
                        X--;
                    else
                        result = false;
                    break;
                }
                case DirectionEnum.Right:
                {
                    if (CanMove(DirectionEnum.Right))
                        X++;
                    else
                        result = false;
                    break;
                }
                case DirectionEnum.Down:
                {
                    if (CanMove(DirectionEnum.Down))
                        Y++;
                    else
                        result = false;
                    break;
                }
                case DirectionEnum.Up:
                {
                    if (CanMove(DirectionEnum.Up))
                        Y--;
                    else
                        result = false;
                    break;
                }
            }
            return result;
        }

        protected abstract SpriteTypeEnum GetSprite(bool isFalling, SpriteTypeEnum oldSprite);


        public override void Update(GameTime gameTime)
        {
            _moveInterval = _moveInterval - gameTime.ElapsedGameTime;
            if (_moveInterval.Ticks <= 0)
            {
                ActorUpdate(gameTime);
                _moveInterval = new TimeSpan(Game.GameSpeedScaleFactor*_speedCoef);
            }
            base.Update(gameTime);
        }

        protected virtual void ActorUpdate(GameTime gameTime)
        {
            _isFalling = ProcessFalling(_isFalling);
            if (!_isFalling)
            {
                bool isMoving = Move(Direction);
                if (!isMoving)
                    Direction = DirectionEnum.None;
            }
            else
                Direction = DirectionEnum.None;

            if (OnReportPostion != null)
                OnReportPostion(this, new ReportPostionEventArgs(X, Y));

            _heroSprite = GetSprite(_isFalling, _heroSprite);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(Game.Sprites[_heroSprite, Game.BackColor].Texture, new Vector2(X*16, (Y + 2)*32),
                Color.White);
            Game.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}