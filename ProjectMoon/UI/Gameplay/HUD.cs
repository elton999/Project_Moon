using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaToolKit;

using ProjectMoon.Entities;

namespace ProjectMoon.UI.Gameplay
{
    public class HUD : GameObject
    {
        public override void Start()
        {
            base.Start();
            
            // create square
            this.Sprite = new Texture2D(this.Scene.ScreemGraphicsDevice, this.Scene.Sizes.X, 29);
            Color[] data = new Color[this.Scene.Sizes.X*29];
            for (int i = 0; i < data.Length; ++i) 
                data[i] = Color.Black;
            this.Sprite.SetData(data);

            this.Position = new Vector2(0, this.Scene.Sizes.Y - this.Sprite.Height);

            // HUD Texts
            this.LifeText.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.LifeText.Position = new Vector2(10, this.Scene.Sizes.Y - this.Sprite.Height + 4);
            this.LifeText.Body = new Rectangle(new Point(0,72), new Point(36, 8));

            this.PowerText.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.PowerText.Position = new Vector2(66, this.Scene.Sizes.Y - this.Sprite.Height + 4);
            this.PowerText.Body = new Rectangle(new Point(0, 88), new Point(52, 8));

            this.LifeCountSprite_ON.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.LifeCountSprite_ON.Body = new Rectangle(new Point(40,72), new Point(10,10));

            this.LifeCountStripte_OFF.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.LifeCountStripte_OFF.Body = new Rectangle(new Point(48,72), new Point(10,10));

            this.StatusSprite_ON.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.StatusSprite_ON.Body = new Rectangle(new Point(56,88), new Point(4,9));

            this.StatusSprite_OFF.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/tilemap");
            this.StatusSprite_OFF.Body = new Rectangle(new Point(61,88), new Point(4,9));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



        public GameObject LifeText = new GameObject();
        public GameObject LifeCountSprite_ON = new GameObject();
        public GameObject LifeCountStripte_OFF = new GameObject();

        public GameObject PowerText = new GameObject();
        public GameObject StatusSprite_ON = new GameObject();
        public GameObject StatusSprite_OFF = new GameObject();

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //background
            this.DrawSprite(spriteBatch);

            // Texts
            this.LifeText.DrawSprite(spriteBatch);
            this.PowerText.DrawSprite(spriteBatch);

            // life status
            for (int i = 0; i < this.Scene.GameManagement.Values["CURRENT_LIFES"]; i++)
            {
                this.LifeCountSprite_ON.Position = new Vector2(10 + (i*11), this.Position.Y + 16);
                this.LifeCountSprite_ON.DrawSprite(spriteBatch);
            }

            // Fuel Status
            for(int i = 0; i < 10; i++)
            {
                float _PowerStatusFloat = this.Scene.GameManagement.Values["POWER"] / 10f;
                int _PowerStatus = (int)(_PowerStatusFloat > 0 ? _PowerStatusFloat + 1 : _PowerStatusFloat);

                if (i < _PowerStatus)
                {
                    this.StatusSprite_ON.Position = new Vector2(66 + (i * 5), this.Position.Y + 16);
                    this.StatusSprite_ON.DrawSprite(spriteBatch);
                } else
                {
                    this.StatusSprite_OFF.Position = new Vector2(66 + (i * 5), this.Position.Y + 16);
                    this.StatusSprite_OFF.DrawSprite(spriteBatch);
                }
            }


        }
    }
}
