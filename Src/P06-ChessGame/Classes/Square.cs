using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ChessGame.Classes
{
    class Square
    {
        private Piece piece;

        public int Row { get; set; }
        public int Column { get; set; }
        public Piece Piece
        {
            get { return piece; }
            set
            {
                if (value != null)
                {
                    value.ImagePos = this.ImagePos;
                    value.Square = this;
                    piece = value;
                }
                else
                    piece = null;
            }
        }
        public Color Color { get; set; }
        public Texture2D Image { get; set; }
        public Texture2D SelectionImage { get; set; }
        public Vector2 ImagePos { get; set; }

        public Square(int row, int column, Color color, Vector2 imagePos)
        {
            Row = row;
            Column = column;
            Color = color;
            ImagePos = imagePos;
        }

        public void LoadContent(ContentManager content)
        {
            SelectionImage = content.Load<Texture2D>("square_selection");
            if (Color == Color.White)
                Image = content.Load<Texture2D>("square_white");
            else
                Image = content.Load<Texture2D>("square_black");
        }
    }
}
