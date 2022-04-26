using System;
using UmbrellaToolsKit;
using GameProject.Entities.Player;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Collision;

namespace GameProject
{
    public class SceneManagementGame : SceneManagement
    {
        public Player Player;

        public override void Start()
        {
            base.Start();
            StartGame();
        }

        public void StartGame() => MainScene.SetLevelLdtk(1);

        public void ChangeLevel(int level)
        {
            var player = MainScene.AllActors[0];
            RemovePlayerFromCurrentScene();
            MainScene.Dispose();

            MainScene.SetLevelLdtk(level);
            RemovePlayerFromCurrentScene();

            MainScene.Players.Add(player);
            MainScene.AllActors.Insert(0, player);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!CheckBoundsCurrentScene())
                ChangeLevel(GetWherePlayerLevelIs());
        }

        public int GetWherePlayerLevelIs()
        {
            foreach (ldtk.Level level in MainScene.TileMapLdtk.Levels)
            {
                var sceneBounds = new UmbrellaToolsKit.Collision.Solid()
                {
                    Position = new Vector2(level.WorldX, level.WorldY),
                    size = new Point((int)level.PxWid, (int)level.PxHei)
                };

                var player = GetPlayerActorDirection();

                if (sceneBounds.overlapCheck(player))
                    return int.Parse(level.Identifier.ToString().Replace("Level_", ""));
            }
            return 0;
        }

        public bool CheckBoundsCurrentScene()
        {
            var sceneBounds = new UmbrellaToolsKit.Collision.Solid()
            {
                Position = MainScene.ScreenOffset.ToVector2(),
                size = MainScene.LevelSize.ToPoint()
            };

            var player = GetPlayerActorDirection();

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

        private Actor GetPlayerActorDirection()
        {
            var directionPlayer = new Vector2(Math.Sign(MainScene.AllActors[0].velocity.X), Math.Sign(MainScene.AllActors[0].velocity.Y));

            return new UmbrellaToolsKit.Collision.Actor()
            {
                Position = MainScene.AllActors[0].Position + directionPlayer * 2f,
                size = MainScene.AllActors[0].size
            };
        }
    }
}