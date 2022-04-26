using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UmbrellaToolsKit
{
    public class SceneManagement
    {
        public GameManagement GameManagement;
        public Scene MainScene;
        public int CurrentScene = 1;
        public int MaxScenes = 1;

        public virtual void Start() => SetScene();

        public virtual void SetScene()
        {
            MainScene = new Scene(
                GameManagement.Game.GraphicsDevice,
                GameManagement.Game.Content
            );
            MainScene.GameManagement = GameManagement;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (MainScene != null && MainScene.LevelReady)
                MainScene.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (MainScene != null && MainScene.LevelReady)
                MainScene.Draw(spriteBatch,
                GameManagement.Game.GraphicsDevice,
                new Vector2(
                    GameManagement.Game.GraphicsDevice.Viewport.Width,
                    GameManagement.Game.GraphicsDevice.Viewport.Height
                ));
        }
    }
}