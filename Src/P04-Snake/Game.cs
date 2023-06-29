using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Snake
{
    internal struct Apple
    {
        public Vector2 position;
    }
    internal struct Snake
    {
        public Vector2 position;
        public Vector2 speed;
        public List<Vector2> links;
        public int length;
    }
    internal struct State
    {
        public Apple apple;
        public Snake snake;
        public int score;
        public int highscore;
    }

    public class Game
    {
        Random r;
        int tiles;
        public int Tiles => tiles;

        public Game(int tiles)
        {
            this.r = new Random();
            this.tiles = tiles;
        }

        Apple NextApple()
        {
            return new Apple { position = new Vector2(r.Next(0, tiles), r.Next(0, tiles)) };
        }

        Snake NextSnake()
        {
            return new Snake
            {
                position = new Vector2((float)Math.Floor(tiles / 2f), (float)Math.Floor(tiles / 2f)),
                speed = Vector2.Zero,
                links = new List<Vector2>(),
                length = 5
            };
        }

        internal State Init(int highscore)
        {
            return new State
            {
                apple = NextApple(),
                snake = NextSnake(),
                score = 0,
                highscore = highscore
            };
        }

        public Vector2 NextSpeed(Vector2 speed, Input key = Input.None)
        {
            if (key != Input.None)
            {
                var nextSpeed = new Vector2(
                    (key == Input.Right ? 1 : 0) - (key == Input.Left ? 1 : 0),
                    (key == Input.Down ? 1 : 0) - (key == Input.Up ? 1 : 0)
                );
                if (key == Input.TurnLeft)
                {
                    nextSpeed = new Vector2(speed.Y, -speed.X);
                }
                else if (key == Input.TurnRight)
                {
                    nextSpeed = new Vector2(-speed.Y, speed.X);
                }
                // when starting the game, and snake isn't moving yet, move upwards
                if(nextSpeed == Vector2.Zero)
                {
                    nextSpeed = -Vector2.UnitY;
                }
                // prevent 180° turns
                return nextSpeed != -speed ? nextSpeed : speed;
            }
            else
            {
                return speed;
            }
        }

        bool IsMoving(Vector2 speed) => speed != Vector2.Zero;

        Snake MoveSnake(Snake snake, Input key)
        {
            var speed = NextSpeed(snake.speed, key);
            var nextPosition = snake.position + speed;
            return new Snake
            {
                position = new Vector2(
                    (snake.position.X + speed.X + tiles) % tiles,
                    (snake.position.Y + speed.Y + tiles) % tiles
                ),
                speed = speed,
                links = new List<Vector2>(snake.links),
                length = snake.length
            };
        }

        bool IsEatingApple(Vector2 position, Apple apple)
        {
            return Vector2.DistanceSquared(position, apple.position) < .5f;
        }

        bool BitesHimself(Snake snake)
        {
            return IsMoving(snake.speed) && snake.links.Any(
                link => Vector2.DistanceSquared(link, snake.position) < .5f);
        }

        Snake NextTail(Snake snake, bool isGrowing)
        {
            var links = snake.links.Concat(new Vector2[] { snake.position });
            if (isGrowing)
            {
                return new Snake
                {
                    position = snake.position,
                    speed = snake.speed,
                    links = links.ToList(),
                    length = snake.length + 1
                };
            }
            else
            {
                return new Snake
                {
                    position = snake.position,
                    speed = snake.speed,
                    links = links.Reverse().Take(snake.length).Reverse().ToList(),
                    length = snake.length
                };
            }
        }

        internal Action<Moment> OnMoment;

        internal State Update(State state, Input key)
        {
            var snake = MoveSnake(state.snake, key);
            var eaten = IsEatingApple(snake.position, state.apple);
            var bitten = BitesHimself(snake);
            var moving = IsMoving(snake.speed);

            if (bitten)
            {
                OnMoment?.Invoke(Moment.Dying);
                return Init(state.highscore);
            }
            else if (eaten)
            {
                OnMoment?.Invoke(Moment.EatingApple);
                var score = state.score + 1;

                if (score > state.highscore)
                {
                    OnMoment?.Invoke(Moment.NewHighscore);
                }

                var highscore = score > state.highscore ? score : state.highscore;
                return new State
                {
                    apple = NextApple(),
                    snake = NextTail(snake, true),
                    score = score,
                    highscore = highscore
                };
            }
            else if (moving)
            {

                OnMoment?.Invoke(Moment.Moving);
                return new State
                {
                    apple = state.apple,
                    snake = NextTail(snake, false),
                    score = state.score,
                    highscore = state.highscore
                };
            }
            else
            {
                return state;
            }
        }
    };

    public enum Input
    {
        None,
        Left,
        Up,
        Right,
        Down,
        TurnLeft,
        TurnRight
    }

    enum Moment
    {
        EatingApple,
        Dying,
        NewHighscore,
        Moving
    }
}