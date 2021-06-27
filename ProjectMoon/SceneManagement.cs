using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;
using ProjectMoon.UI.Gameplay;
using UmbrellaToolKit.Collision;
using ProjectMoon.Entities;

namespace ProjectMoon
{
    public class SceneManagement
    {
        public Scene MainScene;
        public int CurrentLevel = 1;
        public int CurrentPartLevel = 1;
        public string CurrentPathLevel { get => "Maps/Level" + CurrentLevel + "/level_"; }
        public List<Scene> LevelParts;
        public int[] NumberPartOfLevel = { 2, 1, 1 };

        public void Start()
        {

            this.MainScene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);
            this.MainScene.GameManagement = GameManagement.Instance;
            this.MainScene.SetSizes((int)(426 / 1.18f), (int)(240 / 1.18f));
            this.MainScene.CreateBackBuffer();
            this.MainScene.CreateCamera();
            this.MainScene.Camera.Position = new Vector2(this.MainScene.Camera.Position.X + this.MainScene.Sizes.X / 2, this.MainScene.Camera.Position.Y + this.MainScene.Sizes.Y / 2);

            this.MainScene.updateDataTime = 1 / 60f;

            this.LoadLevel();
            this.SetLevel();
        }

        public void Update(GameTime gameTime)
        {
            if (GameManagement.Instance.isPlaying)
            {
                this.MainScene.Update(gameTime);
                if (this.CheckPlayerPart(this.CurrentPartLevel))
                {
                    int _newPartOfLevel = this.checkOfPlayer();
                    if (_newPartOfLevel != this.CurrentPartLevel)
                    {
                        GameManagement.Instance.CurrentStatus = GameManagement.Status.STOP;
                        this.AddPartOfLevelOnMainScene(_newPartOfLevel);

                        this.MainScene.LevelSize = this.LevelParts[_newPartOfLevel - 1].LevelSize;
                        this.MainScene.ScreemOffset = this.LevelParts[_newPartOfLevel - 1].ScreemOffset;

                        Point _playerSize = this.MainScene.Players[0].size;
                        Vector2 _playerPosition = this.MainScene.Players[0].Position;
                        Vector2 _cameraOrigin = this.MainScene.Camera.Origin;
                        Vector2 _cameraPosition = new Vector2(this.MainScene.Camera.Position.X - _cameraOrigin.X, this.MainScene.Camera.Position.Y - _cameraOrigin.Y);

                        this.MainScene.Camera.Target = new Vector2(
                            _cameraPosition.X < _playerPosition.X ? _playerPosition.X + _playerSize.X + _cameraOrigin.X : _playerPosition.X - _cameraOrigin.X,
                            this.MainScene.Players[0].Position.Y
                            );

                        GameManagement.Instance.wait(0.5f, new Action(() =>
                        {
                            this.CurrentPartLevel = _newPartOfLevel;
                            this.LoadLevel();
                            this.SetLevel();
                            GameManagement.Instance.CurrentStatus = GameManagement.Status.PLAYING;
                        }));
                    }
                }
            }

            if (GameManagement.Instance.isStoping)
            {
                this.MainScene.Camera.moveX((float)gameTime.ElapsedGameTime.TotalSeconds);
                this.MainScene.Camera.moveY((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

        }
        public void LoadLevel()
        {
            this.LevelParts = new List<Scene>();
            for (int i = 0; i < this.NumberPartOfLevel[this.CurrentLevel - 1]; i++)
            {
                Scene scene = new Scene(Game1.Instance.GraphicsDevice, Game1.Instance.Content);

                scene.MapLevelPath = this.CurrentPathLevel;
                scene.GameManagement = GameManagement.Instance;
                scene.SetLevel(i + 1);

                scene.Grid.CollidesRamps.Add("r");
                scene.Grid.CollidesRamps.Add("l");

                this.LevelParts.Add(scene);
            }
        }

        public bool CheckPlayerPart(int partLevel)
        {
            Actor _AreaScene = new Actor();
            _AreaScene.size = new Point(
                (int)(this.LevelParts[partLevel - 1].LevelSize.X + (this.MainScene.AllActors[0].velocity.X > 0 ? -1 : 1)),
                (int)(this.LevelParts[partLevel - 1].LevelSize.Y)
            );

            _AreaScene.Position = new Vector2(
                this.LevelParts[partLevel - 1].ScreemOffset.X - (this.MainScene.AllActors[0].velocity.X > 0 ? -2 : 2),
                this.LevelParts[partLevel - 1].ScreemOffset.Y
            );


            if (this.MainScene.AllActors.Count > 0)
                if (this.MainScene.AllActors[0].overlapCheck(_AreaScene))
                    return false;

            return true;
        }

        public int checkOfPlayer()
        {
            for (int i = 0; i < this.NumberPartOfLevel[this.CurrentLevel - 1]; i++)
            {
                if (!this.CheckPlayerPart(i + 1))
                    return i + 1;
            }
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
            this.MainScene.Foreground.Clear();
            this.MainScene.Foreground.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Foreground);
            this.MainScene.Middleground.Clear();
            this.MainScene.Middleground.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Middleground);
            this.MainScene.Backgrounds.Clear();
            this.MainScene.Backgrounds.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Backgrounds);
            this.MainScene.Enemies.Clear();
            this.MainScene.Enemies.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Enemies);

            // Collision
            this.MainScene.AllActors.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllActors);

            if (this.MainScene.AllActors.Count > 0)
            {
                Actor _player = this.MainScene.AllActors[0];
                this.MainScene.AllActors.Clear();

                if (this.MainScene.Players.Count > 0)
                    this.MainScene.Players.RemoveAt(0);

                this.MainScene.AllActors.Insert(0, _player);
                this.MainScene.Players.Insert(0, _player);
                this.LevelParts[this.CurrentPartLevel - 1].AllActors.Remove(_player);
            }

            this.MainScene.Grid = this.LevelParts[this.CurrentPartLevel - 1].Grid;
            this.MainScene.AllActors.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllActors);
            this.MainScene.AllSolids.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllSolids);

            foreach (List<GameObject> ListGameObject in this.MainScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.MainScene;

            this.MainScene.Grid.Scene = this.MainScene;
            this.MainScene.ScreemOffset = this.LevelParts[this.CurrentPartLevel - 1].ScreemOffset;
            this.MainScene.LevelSize = this.LevelParts[this.CurrentPartLevel - 1].LevelSize;
            this.MainScene.addLayers();
        }

    }
}