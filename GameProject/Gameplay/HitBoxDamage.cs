using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;

namespace GameProject.Gameplay
{
    public class HitBoxDamage : Actor
    {
        Square Box;
        public override void Start()
        {
            base.Start();
            tag = "damage";

            if (!Scene.GameManagement.Values["DEBUG"])
                return;

            Box = new Square();
            Box.Position = Position;
            Box.size = size;
            Box.SquareColor = Color.Red;
            Box.Scene = Scene;

            Box.Start();
        }

        public override void UpdateData(GameTime gameTime)
        {
            if (Scene.AllActors[0].overlapCheckPixel(this))
                Scene.AllActors[0].OnCollision(tag);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Scene.GameManagement.Values["DEBUG"])
            {
                Box.Scene = Scene;
                Box.Position = Position;
                Box.Draw(spriteBatch);
            }
        }
    }
}
