using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Sprite;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Actors
{
    public class Victim : GameObject
    {

        Square Square;
        public override void Start()
        {
            base.Start();
            this.size = new Point(10, 32);
            this.Square = new Square();
            this.Square.size = this.size;
            this.Square.Scene = this.Scene;
            this.Square.SquareColor = Color.Green;
            this.Square.Start();
            this.Square.Position = this.Position;
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            this.Square.Draw(spriteBatch);
        }
    }
}
