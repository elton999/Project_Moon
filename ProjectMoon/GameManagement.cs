using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMoon.UI.Gameplay;

namespace ProjectMoon
{
    public class GameManagement : UmbrellaToolKit.GameManagement
    {
        public SceneManagement SceneManagement;

        public HUD GameplayHud;
        public static GameManagement Instance;
        public override void Start()
        {
            if (Instance == null)
                Instance = this;
            this.SetAllValues();
            this.SceneManagement = new SceneManagement();
            this.SceneManagement.Start();

            this.GameplayHud = new HUD();
            this.GameplayHud.Scene = this.SceneManagement.MainScene;

            this.GameplayHud.Start();
            this.SceneManagement.MainScene.UI.Add(this.GameplayHud);

            this.SceneManagement.MainScene.LevelReady = true;
            this.CurrentStatus = Status.PLAYING;
        }

        // gameplay
        public void SetAllValues()
        {
            this.Values.Add("POWER", 100f);
            this.Values.Add("TOTAL_LIFES", 3);
            this.Values.Add("CURRENT_LIFES", 3);
            this.Values.Add("DEBUG", true);
        }

        public bool isPlaying
        {
            get => this.CurrentStatus == Status.PLAYING;
        }

        public bool isStoping
        {
            get => this.CurrentStatus == Status.STOP;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.SceneManagement.Update(gameTime);
            System.Console.Clear();
            System.Console.WriteLine(1 / (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            this.SceneManagement.MainScene.Draw(spriteBatch, Game1.Instance.GraphicsDevice, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height));
        }
    }
}
