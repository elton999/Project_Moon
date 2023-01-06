using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UmbrellaToolsKit
{
    public class GameManagement : GameObject
    {
        public Dictionary<String, dynamic> Values = new Dictionary<string, dynamic>();

        public enum Status { LOADING, CREDITS, MENU, PAUSE, STOP, PLAYING };
        public Status CurrentStatus;

        public enum GameplayStatus { ALIVE, DEATH, };
        public GameplayStatus CurrentGameplayStatus;

        public SceneManagement SceneManagement;
        public Game Game;

        public SpriteBatch SpriteBatch;

        public override void Start()
        {
            CurrentStatus = Status.PLAYING;
            SceneManagement = new SceneManagement();
            SceneManagement.GameManagement = this;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            processWait(gameTime);
            SceneManagement.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) => SceneManagement.Draw(spriteBatch);
    }
}
