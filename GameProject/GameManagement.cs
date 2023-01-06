using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.UI.Gameplay;
using UmbrellaToolsKit.EditorEngine;
using System;

namespace GameProject
{
    public class GameManagementGame : UmbrellaToolsKit.GameManagement
    {
        private EditorMain _edtior;
        public HUD GameplayHud;
        public static GameManagementGame Instance;
        public GameTime GameTime;

        public override void Start()
        {
            Game = Game1.Instance;
            base.Start();
            if (Instance == null)
                Instance = this;

            SetAllValues();
            setSceneSettings();
            CreateHud();
            SceneManagement.MainScene.LevelReady = true;
            CurrentStatus = Status.PLAYING;

            _edtior = new EditorMain(Game, this);

            // teste light
            /*var lights = new Light();
            lights.Scene = this.SceneManagement.MainScene;
            this.SceneManagement.MainScene.UI.Add(lights);
            lights.Start();*/
        }

        private void setSceneSettings()
        {
            SceneManagement = new SceneManagementGame();
            SceneManagement.GameManagement = this;
            SceneManagement.Start();
        }

        private void CreateHud()
        {
            GameplayHud = new HUD();
            GameplayHud.Scene = SceneManagement.MainScene;
            GameplayHud.Start();
            SceneManagement.MainScene.UI.Add(this.GameplayHud);
        }

        // gameplay
        public void SetAllValues()
        {
            Values.Add("POWER", 100f);
            Values.Add("TOTAL_LIFES", 3);
            Values.Add("CURRENT_LIFES", 3);
            Values.Add("DEBUG", true);
        }

        public bool isPlaying
        {
            get => CurrentStatus == Status.PLAYING;
        }

        public bool isStoping
        {
            get => CurrentStatus == Status.STOP;
        }
        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
            base.Draw(spriteBatch);
            _edtior.Draw(GameTime);
        }
    }
}
