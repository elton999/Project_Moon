using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;
using UmbrellaToolKit.Collision;

namespace ProjectMoon.Entities.Actors.Enemies
{
    public class Bat : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "bat";

            this.Scene.AllActors.Add(this);
            this.size = new Point(10, 10);
            this._Speed = 30;

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Black;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            if (this.isLive)
            {
                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    this.Scene.AllActors[0].OnCollision(this.tag);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isLive)
            {
                base.Draw(spriteBatch);
                if (this.Scene.GameManagement.Values["DEBUG"])
                {
                    this.Box.Position = this.Position;
                    this.Box.Draw(spriteBatch);
                }
            }
        }
    }
}
