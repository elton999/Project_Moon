using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;

namespace ProjectMoon
{
    public class Light : GameObject
    {
        Effect LightShader;

        private GameObject Light1;
        private GameObject Light2;
        private RenderTarget2D lightsLayer;

        public override void Start()
        {
            base.Start();
            LightShader = this.Scene.Content.Load<Effect>("Shaders/Light");

            Light1 = new GameObject();
            Light1.Scene = this.Scene;
            Light1.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/light");
            Light1.Scale = 0.5f;

            Light2 = new GameObject();
            Light2.Scene = this.Scene;
            Light2.Sprite = this.Scene.Content.Load<Texture2D>("Sprites/light");
            Light2.Position = new Vector2(0, 0);
            Light2.Scale = 0.5f;

            lightsLayer = new RenderTarget2D(this.Scene.ScreemGraphicsDevice, 400, 400);
        }

        float timer = 0;
        public override void Update(GameTime gameTime)
        {
            timer+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Light2.Position.X = MathF.Cos(timer * 0.0005f) * 150f;
        }

        public override void DrawBeforeScene(SpriteBatch spriteBatch)
        {
            this.Effect = null;
            this.Scene.ScreemGraphicsDevice.SetRenderTarget(lightsLayer);
            this.Scene.ScreemGraphicsDevice.Clear(Color.Transparent);
            BeginDraw(spriteBatch, false);
            Light1.DrawSprite(spriteBatch);
            Light2.DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
            this.Scene.ScreemGraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Effect = this.LightShader;
            this.Sprite = (Texture2D)this.lightsLayer;
            BeginDraw(spriteBatch, false);
            DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }
    }
}
