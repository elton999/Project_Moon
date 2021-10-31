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
        public int[] NumberPartOfLevel = { 2, 1, 1 };

        public override void Start()
        {
            this.GameManagement = GameManagementGame.Instance;
            this.MainSceneSettings();
            this.LoadLevel();
            this.SetLevel();
        }

        private void MainSceneSettings()
        {
            this.MainScene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);
            this.MainScene.GameManagement = GameManagementGame.Instance;
            this.MainScene.SetSizes((int)(426 / 1.18f), (int)(240 / 1.18f));
            this.MainScene.CreateBackBuffer();
            this.MainScene.CreateCamera();
            this.MainScene.Camera.Position = Vector2.Add(this.MainScene.Camera.Position, Vector2.Divide(this.MainScene.Sizes.ToVector2(), 2f));

            this.MainScene.updateDataTime = 1 / 60f;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameManagementGame.Instance.isPlaying)
            {
                this.MainScene.Update(gameTime);

                if (CheckPlayerPart(CurrentPartLevel) && checkOfPlayer() != CurrentPartLevel)
                {
                    int _newPartOfLevel = this.checkOfPlayer();
                    GameManagementGame.Instance.CurrentStatus = GameManagement.Status.STOP;
                    AddPartOfLevelOnMainScene(_newPartOfLevel);

                    this.MainScene.LevelSize = this.LevelParts[_newPartOfLevel - 1].LevelSize;
                    this.MainScene.ScreemOffset = this.LevelParts[_newPartOfLevel - 1].ScreemOffset;

                    this.SetNewTargetCamera();

                    GameManagementGame.Instance.wait(0.5f, new Action(() =>
                    {
                        this.CurrentPartLevel = _newPartOfLevel;
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
                this.MainScene.Camera.Position = Vector2.Lerp(
                    this.MainScene.Camera.Position,
                    this.MainScene.Camera.Target,
                    (float)gameTime.ElapsedGameTime.TotalSeconds * 8f
                );
            }
        }

        private void SetNewTargetCamera()
        {
            Point _playerSize = this.MainScene.Players[0].size;
            Vector2 _playerPosition = this.MainScene.Players[0].Position;
            Vector2 _cameraOrigin = this.MainScene.Camera.Origin;
            Vector2 _cameraPosition = this.MainScene.Camera.Position;

            this.MainScene.Camera.Target = new Vector2(
                _cameraPosition.X < _playerPosition.X ? _playerPosition.X + _cameraOrigin.X : _playerPosition.X - _cameraOrigin.X,
                this.MainScene.Players[0].Position.Y
            );
        }

        public void LoadLevel()
        {
            this.LevelParts = new List<Scene>();
            for (int i = 0; i < this.NumberPartOfLevel[this.CurrentScene - 1]; i++)
            {
                Scene scene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);

                scene.MapLevelPath = this.CurrentPathLevel;
                scene.GameManagement = GameManagementGame.Instance;
                scene.SetLevel(i + 1);

                scene.Grid.CollidesRamps.Add("r");
                scene.Grid.CollidesRamps.Add("l");

                this.LevelParts.Add(scene);
            }
        }

        public bool CheckPlayerPart(int partLevel)
        {
            partLevel--;
            var areaScene = new Actor();
            var player = this.MainScene.AllActors[0];

            areaScene.size = new Point(
                (int)(this.LevelParts[partLevel].LevelSize.X + Math.Sign(player.velocity.X)),
                (int)(this.LevelParts[partLevel].LevelSize.Y)
            );

            areaScene.Position = new Vector2(
                this.LevelParts[partLevel].ScreemOffset.X - (float)Math.Sin(player.velocity.X) * 50f,
                this.LevelParts[partLevel].ScreemOffset.Y
            );

            if (this.MainScene.AllActors.Count > 0 && player.overlapCheck(areaScene))
                return false;

            return true;
        }

        public int checkOfPlayer()
        {
            for (int i = 0; i < this.NumberPartOfLevel[this.CurrentScene - 1]; i++)
                if (!this.CheckPlayerPart(i + 1))
                    return i + 1;
            return this.CurrentPartLevel;
        }

        public void AddPartOfLevelOnMainScene(int partLevel)
        {
            this.MainScene.Foreground.AddRange(this.LevelParts[partLevel - 1].Foreground);
            this.MainScene.Middleground.AddRange(this.LevelParts[partLevel - 1].Middleground);
            this.MainScene.Backgrounds.AddRange(this.LevelParts[partLevel - 1].Backgrounds);
            this.MainScene.Enemies.AddRange(this.LevelParts[partLevel - 1].Enemies);

            foreach (List<GameObject> ListGameObject in this.MainScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.MainScene;
        }

        public void SetLevel()
        {
            var newCurrentScene = this.LevelParts[this.CurrentPartLevel - 1];

            SetNewScene(newCurrentScene);

            // Collision
            this.MainScene.AllActors.AddRange(newCurrentScene.AllActors);

            if (this.MainScene.AllActors.Count > 0)
            {
                Actor player = this.MainScene.AllActors[0];
                this.MainScene.AllActors.Clear();

                if (this.MainScene.Players.Count > 0)
                    this.MainScene.Players.RemoveAt(0);

                this.MainScene.AllActors.Insert(0, player);
                this.MainScene.Players.Insert(0, player);
                newCurrentScene.AllActors.Remove(player);
            }

            this.MainScene.Grid = newCurrentScene.Grid;
            this.MainScene.AllActors.AddRange(newCurrentScene.AllActors);
            this.MainScene.AllSolids.AddRange(newCurrentScene.AllSolids);

            foreach (List<GameObject> ListGameObject in this.MainScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.MainScene;

            this.MainScene.Grid.Scene = this.MainScene;
            this.MainScene.ScreemOffset = newCurrentScene.ScreemOffset;
            this.MainScene.LevelSize = newCurrentScene.LevelSize;
            this.MainScene.addLayers();
        }

        private void SetNewScene(Scene newCurrentScene)
        {
            this.MainScene.Foreground.Clear();
            this.MainScene.Foreground.AddRange(newCurrentScene.Foreground);
            this.MainScene.Middleground.Clear();
            this.MainScene.Middleground.AddRange(newCurrentScene.Middleground);
            this.MainScene.Backgrounds.Clear();
            this.MainScene.Backgrounds.AddRange(newCurrentScene.Backgrounds);
            this.MainScene.Enemies.Clear();
            this.MainScene.Enemies.AddRange(newCurrentScene.Enemies);
        }
    }
}