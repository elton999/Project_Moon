using System;
using System.Collections;
using UmbrellaToolsKit;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Collision;

namespace GameProject
{
    public class SceneTransition : SceneManagement
    {
        private Vector2 _oldScenePosition = Vector2.Zero;
        protected bool _isCameraTransition = false;

        public CoroutineManagement Coroutine = new();

        public void ChangeLevel(int level)
        {
            var player = MainScene.AllActors[0];
            var background = MainScene.Backgrounds[0];
            MainScene.Backgrounds.RemoveAt(0);

            _oldScenePosition = MainScene.ScreenOffset.ToVector2();
            MainScene.Players.Clear();
            MainScene.Dispose();

            MainScene.SetLevelLdtk(level);
            _setGameObjectsOnScene(player, background);
            _setRampSettingOnScene();

            MainScene.updateDataTime = 1f / 30f;
            Coroutine.StarCoroutine(CameraTransition());
        }

        public int GetWherePlayerLevelIs()
        {
            foreach (ldtk.Level level in MainScene.TileMapLdtk.Levels)
            {
                var sceneBounds = new Solid()
                {
                    Position = new Vector2(level.WorldX, level.WorldY),
                    size = new Point((int)level.PxWid, (int)level.PxHei)
                };

                var player = _getPlayerActorDirection();
                if (sceneBounds.overlapCheck(player))
                    return int.Parse(level.Identifier.ToString().Replace("Level_", ""));
            }
            return 0;
        }

        public bool CheckBoundsCurrentScene()
        {
            var sceneBounds = new Solid()
            {
                Position = MainScene.ScreenOffset.ToVector2(),
                size = MainScene.LevelSize.ToPoint()
            };

            var player = _getPlayerActorDirection();
            return sceneBounds.overlapCheck(player);
        }

        public void RemovePlayerFromCurrentScene()
        {
            if (MainScene.Players.Count == 0)
                return;

            var player = MainScene.AllActors[0];
            MainScene.AllActors.Remove(player);
            MainScene.Players.Remove(player);
        }

        public IEnumerator CameraTransition()
        {
            _isCameraTransition = true;
            SetCamaraTargetToTransition();

            yield return null;

            float timer = 0f;
            while (timer <= .5f)
            {
                timer += (float)Coroutine.GameTime.ElapsedGameTime.TotalSeconds;
                MainScene.Camera.Position = Vector2.Lerp(
                   MainScene.Camera.Position,
                   MainScene.Camera.Target,
                   (float)Coroutine.GameTime.ElapsedGameTime.TotalSeconds * 5f
               );
                yield return null;
            }
            MainScene.updateDataTime = 1f / 60f;
            _removeOldBackground();
            _isCameraTransition = false;
            yield return null;
        }


        public void SetCamaraTargetToTransition()
        {
            Vector2 playerPosition = MainScene.Players[0].Position;
            Vector2 cameraPosition = MainScene.Camera.Position;
            var player = MainScene.AllActors[0];
            Vector2 velocityDirection = new Vector2(Math.Sign(player.velocity.X), Math.Sign(player.velocity.Y));

            if (_oldScenePosition.X != MainScene.ScreenOffset.ToVector2().X)
            {
                MainScene.Camera.Target = cameraPosition * Vector2.UnitY;
                MainScene.Camera.Target += (velocityDirection.X > 0 ? playerPosition + MainScene.Camera.Origin : playerPosition - MainScene.Camera.Origin) * Vector2.UnitX;
            }
            else
            {
                MainScene.Camera.Target = cameraPosition * Vector2.UnitX;
                MainScene.Camera.Target += (velocityDirection.Y > 0 ? playerPosition + MainScene.Camera.Origin : playerPosition - MainScene.Camera.Origin) * Vector2.UnitY;
            }
        }

        private void _setGameObjectsOnScene(Actor player, GameObject background)
        {
            RemovePlayerFromCurrentScene();

            _setPlayerOnLevel(player);
            MainScene.Backgrounds.Add(background);
        }

        private void _setPlayerOnLevel(Actor player)
        {
            MainScene.Players.Add(player);
            MainScene.AllActors.Insert(0, player);
        }

        private void _removeOldBackground()
        {
            MainScene.Backgrounds[1].Dispose();
            MainScene.Backgrounds.RemoveAt(1);
        }

        protected void _setRampSettingOnScene()
        {
            MainScene.Grid.CollidesRamps.Add("3");
            MainScene.Grid.CollidesRamps.Add("2");
        }

        private Actor _getPlayerActorDirection()
        {
            var directionPlayer = new Vector2(Math.Sign(MainScene.AllActors[0].velocity.X), Math.Sign(MainScene.AllActors[0].velocity.Y));

            return new Actor()
            {
                Position = MainScene.AllActors[0].Position + directionPlayer * 2f,
                size = MainScene.AllActors[0].size
            };
        }
    }
}

