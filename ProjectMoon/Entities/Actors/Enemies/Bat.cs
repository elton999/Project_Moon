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
            this._Speed = 5;

            this.InitialPosition = this.Position;
            this._XCon = getRandom.Next(5, 15);
            this._YCon = getRandom.Next(15, 25);

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

        float _YCon = 20;
        float _XCon = 10;
        public override void UpdateData(GameTime gameTime)
        {
            if (this.isLive)
            {
                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    this.Scene.AllActors[0].OnCollision(this.tag);

                this.Position.Y = this.InitialPosition.Y + (int)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds * 0.001f * this._Speed) * _YCon);
                this.Position.X = this.InitialPosition.X + (int)(Math.Cos(gameTime.TotalGameTime.TotalMilliseconds * 0.001f * this._Speed) * _XCon);

            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
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
