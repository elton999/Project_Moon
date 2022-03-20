using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject
{
    public class SceneManagementGame : UmbrellaToolsKit.SceneManagement
    {
        public int CurrentPartLevel = 1;
        public string CurrentPathLevel { get => "Maps/Level" + CurrentScene + "/level_"; }
        public List<Scene> LevelParts;
        public int[] NumberPartOfLevel = { 2, 1};

        public override void Start()
        {
            GameManagement = GameManagementGame.Instance;
            MainSceneSettings();
            LoadLevel();
            SetLevel();
        }

        private void MainSceneSettings()
        {
            MainScene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);
            MainScene.GameManagement = GameManagementGame.Instance;
            MainScene.SetSizes((int)(426 / 1.18f), (int)(240 / 1.18f));
            MainScene.CreateBackBuffer();
            MainScene.CreateCamera();
            MainScene.Camera.Position = Vector2.Add(this.MainScene.Camera.Position, Vector2.Divide(this.MainScene.Sizes.ToVector2(), 2f));

            MainScene.updateDataTime = 1 / 60f;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameManagementGame.Instance.isPlaying)
            {
                MainScene.Update(gameTime);

                if (CheckPlayerPart(CurrentPartLevel) && checkOfPlayer() != CurrentPartLevel)
                {
                    int _newPartOfLevel = checkOfPlayer();
                    GameManagementGame.Instance.CurrentStatus = GameManagement.Status.STOP;
                    AddPartOfLevelOnMainScene(_newPartOfLevel);

                    MainScene.LevelSize = LevelParts[_newPartOfLevel - 1].LevelSize;
                    MainScene.ScreemOffset = LevelParts[_newPartOfLevel - 1].ScreemOffset;

                    SetNewTargetCamera();

                    GameManagementGame.Instance.wait(0.5f, new Action(() =>
                    {
                        CurrentPartLevel = _newPartOfLevel;
                        LoadLevel();
                        SetLevel();
                        GameManagementGame.Instance.CurrentStatus = GameManagement.Status.PLAYING;
                    }));
                }
            }

            CameraMoveTransition(gameTime);
        }

        private void CameraMoveTransition(GameTime gameTime)
        {
            if (GameManagementGame.Instance.isStoping)
            {
                MainScene.Camera.Position = Vector2.Lerp(
                    MainScene.Camera.Position,
                    MainScene.Camera.Target,
                    (float)gameTime.ElapsedGameTime.TotalSeconds * 8f
                );
            }
        }

        private void SetNewTargetCamera()
        {
            Point _playerSize = MainScene.Players[0].size;
            Vector2 _playerPosition = MainScene.Players[0].Position;
            Vector2 _cameraOrigin = MainScene.Camera.Origin;
            Vector2 _cameraPosition = MainScene.Camera.Position;

            MainScene.Camera.Target = new Vector2(
                _cameraPosition.X < _playerPosition.X ? _playerPosition.X + _cameraOrigin.X : _playerPosition.X - _cameraOrigin.X,
                MainScene.Players[0].Position.Y
            );
        }

        public void LoadLevel()
        {
            LevelParts = new List<Scene>();
            for (int i = 1; i <= NumberPartOfLevel[CurrentScene - 1]; i++)
            {
                Scene scene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);

                scene.MapLevelPath = this.CurrentPathLevel;
                scene.GameManagement = GameManagementGame.Instance;
                scene.MapLevelLdtkPath = "Maps/TileSettingsLdtk";
                scene.SetLevelLdtk(i);

                scene.Grid.CollidesRamps.Add("3");
                scene.Grid.CollidesRamps.Add("2");

                LevelParts.Add(scene);
            }
        }

        public bool CheckPlayerPart(int partLevel)
        {
            partLevel--;
            var areaScene = new Actor();
            var player = MainScene.AllActors[0];

            areaScene.size = new Point(
                (int)(LevelParts[partLevel].LevelSize.X + Math.Sign(player.velocity.X)),
                (int)(LevelParts[partLevel].LevelSize.Y)
            );

            areaScene.Position = new Vector2(
                LevelParts[partLevel].ScreemOffset.X - (float)Math.Sin(player.velocity.X) * 50f,
                LevelParts[partLevel].ScreemOffset.Y
            );

            if (MainScene.AllActors.Count > 0 && player.overlapCheck(areaScene))
                return false;

            return true;
        }

        public int checkOfPlayer()
        {
            for (int i = 0; i < NumberPartOfLevel[CurrentScene - 1]; i++)
                if (!CheckPlayerPart(i + 1))
                    return i + 1;
            return CurrentPartLevel;
        }

        public void AddPartOfLevelOnMainScene(int partLevel)
        {
            MainScene.Foreground.AddRange(LevelParts[partLevel - 1].Foreground);
            MainScene.Middleground.AddRange(LevelParts[partLevel - 1].Middleground);
            MainScene.Backgrounds.AddRange(LevelParts[partLevel - 1].Backgrounds);
            MainScene.Enemies.AddRange(LevelParts[partLevel - 1].Enemies);

            foreach (List<GameObject> ListGameObject in MainScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = MainScene;
        }

        public void SetLevel()
        {
            var newCurrentScene = LevelParts[CurrentPartLevel - 1];

            SetNewScene(newCurrentScene);

            // Collision
            MainScene.AllActors.AddRange(newCurrentScene.AllActors);

            if (MainScene.AllActors.Count > 0)
            {
                Actor player = MainScene.AllActors[0];
                MainScene.AllActors.Clear();

                if (MainScene.Players.Count > 0)
                    MainScene.Players.RemoveAt(0);

                MainScene.AllActors.Insert(0, player);
                MainScene.Players.Insert(0, player);
                newCurrentScene.AllActors.Remove(player);
            }

            MainScene.Grid = newCurrentScene.Grid;
            MainScene.AllActors.AddRange(newCurrentScene.AllActors);
            MainScene.AllSolids.AddRange(newCurrentScene.AllSolids);

            foreach (List<GameObject> ListGameObject in this.MainScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.MainScene;

            MainScene.Grid.Scene = this.MainScene;
            MainScene.ScreemOffset = newCurrentScene.ScreemOffset;
            MainScene.LevelSize = newCurrentScene.LevelSize;
            MainScene.addLayers();
        }

        private void SetNewScene(Scene newCurrentScene)
        {
            MainScene.Foreground.Clear();
            MainScene.Foreground.AddRange(newCurrentScene.Foreground);
            MainScene.Middleground.Clear();
            MainScene.Middleground.AddRange(newCurrentScene.Middleground);
            MainScene.Backgrounds.Clear();
            MainScene.Backgrounds.AddRange(newCurrentScene.Backgrounds);
            MainScene.Enemies.Clear();
            MainScene.Enemies.AddRange(newCurrentScene.Enemies);
        }
    }
}