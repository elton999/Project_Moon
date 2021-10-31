using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.UI.Gameplay;

namespace GameProject
{
    public class GameManagementGame : UmbrellaToolsKit.GameManagement
    {
        public HUD GameplayHud;
        public static GameManagementGame Instance;

        public override void Start()
        {
            this.Game = Game1.Instance;
            base.Start();
            if (Instance == null)
                Instance = this;

            this.SetAllValues();
            setSceneSettings();
            CreateHud();
            this.SceneManagement.MainScene.LevelReady = true;
            this.CurrentStatus = Status.PLAYING;

            // teste light
            /*var lights = new Light();
            lights.Scene = this.SceneManagement.MainScene;
            this.SceneManagement.MainScene.UI.Add(lights);
            lights.Start();*/
        }

        private void setSceneSettings()
        {
            this.SceneManagement = new SceneManagementGame();
            this.SceneManagement.GameManagement = this;
            this.SceneManagement.Start();
        }

        private void CreateHud()
        {
            this.GameplayHud = new HUD();
            this.GameplayHud.Scene = this.SceneManagement.MainScene;
            this.GameplayHud.Start();
            this.SceneManagement.MainScene.UI.Add(this.GameplayHud);
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.SceneManagement.MainScene.Draw(spriteBatch, Game1.Instance.GraphicsDevice, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height));
            base.Draw(spriteBatch);
        }
    }
}
