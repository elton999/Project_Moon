using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit.Sprite;
using UmbrellaToolKit.Collision;

namespace ProjectMoon.Gameplay
{
    public class HitBoxDamage : Actor
    {

        Square Box;
        public override void Start()
        {
            base.Start();
            this.tag = "damage";

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Red;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            if (this.Scene.AllActors[0].overlapCheckPixel(this))
            {
                this.Scene.AllActors[0].OnCollision(this.tag);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box.Scene = this.Scene;
                this.Box.Position = this.Position;
                this.Box.Draw(spriteBatch);
            }
        }
    }
}
