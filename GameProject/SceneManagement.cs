using Microsoft.Xna.Framework;

namespace GameProject
{
    public class SceneManagementGame : SceneTransition
    {
        public override void Start()
        {
            base.Start();
            MainScene.updateDataTime = 1f / 60f;
            StartGame();
        }

        public void StartGame()
        {
            MainScene.SetLevelLdtk(1);
            _setRampSettingOnScene();
        }

        public override void Update(GameTime gameTime)
        {
            Coroutine.Update(gameTime);

            if (_isCameraTransition)
                return;

            if (!CheckBoundsCurrentScene())
                ChangeLevel(GetWherePlayerLevelIs());
            base.Update(gameTime);
        }
    }
}