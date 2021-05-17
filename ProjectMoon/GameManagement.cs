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
    public class GameManagement : UmbrellaToolKit.GameManagement
    {
        public Game1 game;

        public Scene GeralScene;
        public int CurrentLevel = 1;
        public int CurrentPartLevel = 1;
        public string CurrentPathLevel { get => "Maps/Level" + CurrentLevel + "/level_"; }
        public List<Scene> LevelParts;
        public int[] NumberPartOfLevel = {2, 1};


        public HUD GameplayHud;
        public void Start()
        {
            this.SetAllValues();

            this.GeralScene = new Scene(this.game.GraphicsDevice, this.game.Content);
            this.GeralScene.GameManagement = this;
            this.GeralScene.SetSizes((int)(426 / 1.18f), (int)(240 / 1.18f));
            this.GeralScene.CreateBackBuffer();
            this.GeralScene.CreateCamera();
            this.GeralScene.Camera.Position = new Vector2(this.GeralScene.Camera.Position.X + this.GeralScene.Sizes.X / 2, this.GeralScene.Camera.Position.Y + this.GeralScene.Sizes.Y / 2);

            this.GeralScene.AssetManagement = this.game.AssetManagement;
            this.GeralScene.updateDataTime = 1 / 60f;

            this.GameplayHud = new HUD();
            this.GameplayHud.Scene = this.GeralScene;

            this.GameplayHud.Start();
            this.GeralScene.UI.Add(this.GameplayHud);

           
            this.LoadLevel();
            this.SetLevel();
            this.GeralScene.LevelReady = true;
            this.CurrentStatus = Status.PLAYING;
        }

        // gameplay
        public void SetAllValues()
        {
            this.Values.Add("POWER", 100f);
            this.Values.Add("TOTAL_LIFES", 3);
            this.Values.Add("CURRENT_LIFES", 3);
            this.Values.Add("DEBUG", true);
        }

        #region Level settings
        public void LoadLevel()
        {
            this.LevelParts = new List<Scene>();
            for (int i = 0; i < this.NumberPartOfLevel[this.CurrentLevel - 1]; i++)
            {
                Scene _scene = new Scene(this.game.GraphicsDevice, this.game.Content);
                
                _scene.MapLevelPath = this.CurrentPathLevel;
                _scene.GameManagement = this;
                _scene.AssetManagement = this.game.AssetManagement;
                _scene.SetLevel(i + 1);

                _scene.Grid.CollidesRamps.Add("r");
                _scene.Grid.CollidesRamps.Add("l");
                
                this.LevelParts.Add(_scene);
            }
        }

        public bool CheckPlayerPart(int partLevel)
        {
            Actor _AreaScene = new Actor();
            _AreaScene.size = new Point(
                (int)(this.LevelParts[partLevel - 1].LevelSize.X + (this.GeralScene.AllActors[0].velocity.X > 0 ? -1 : 1)),
                (int)(this.LevelParts[partLevel - 1].LevelSize.Y)
            );

            _AreaScene.Position = new Vector2(
                this.LevelParts[partLevel - 1].ScreemOffset.X - (this.GeralScene.AllActors[0].velocity.X > 0 ? -2 : 2),
                this.LevelParts[partLevel - 1].ScreemOffset.Y
            );


            if (this.GeralScene.AllActors.Count > 0)
                if (this.GeralScene.AllActors[0].overlapCheck(_AreaScene))
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

        public void AddPartOfLevelOnGeralScene(int partLevel)
        {
            this.GeralScene.Foreground.AddRange(this.LevelParts[partLevel - 1].Foreground);
            this.GeralScene.Middleground.AddRange(this.LevelParts[partLevel - 1].Middleground);
            this.GeralScene.Backgrounds.AddRange(this.LevelParts[partLevel - 1].Backgrounds);
            this.GeralScene.Enemies.AddRange(this.LevelParts[partLevel - 1].Enemies);

            foreach (List<GameObject> ListGameObject in this.GeralScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.GeralScene;
        }

        public void SetLevel()
        {
            this.GeralScene.Foreground.Clear(); 
            this.GeralScene.Foreground.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Foreground);
            this.GeralScene.Middleground.Clear();
            this.GeralScene.Middleground.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Middleground);
            this.GeralScene.Backgrounds.Clear();
            this.GeralScene.Backgrounds.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Backgrounds);
            this.GeralScene.Enemies.Clear(); 
            this.GeralScene.Enemies.AddRange(this.LevelParts[this.CurrentPartLevel - 1].Enemies);

            // Collision
            this.GeralScene.AllActors.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllActors);

            if (this.GeralScene.AllActors.Count > 0) {
                Actor _player = this.GeralScene.AllActors[0];
                this.GeralScene.AllActors.Clear();

                if(this.GeralScene.Players.Count > 0)
                    this.GeralScene.Players.RemoveAt(0);

                this.GeralScene.AllActors.Insert(0, _player);
                this.GeralScene.Players.Insert(0, _player);
                this.LevelParts[this.CurrentPartLevel - 1].AllActors.Remove(_player);
            }

            this.GeralScene.Grid = this.LevelParts[this.CurrentPartLevel - 1].Grid;
            this.GeralScene.AllActors.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllActors);
            this.GeralScene.AllSolids.AddRange(this.LevelParts[this.CurrentPartLevel - 1].AllSolids);

            foreach (List<GameObject> ListGameObject in this.GeralScene.SortLayers)
                foreach (GameObject gameObject in ListGameObject)
                    gameObject.Scene = this.GeralScene;

            this.GeralScene.Grid.Scene = this.GeralScene;
            this.GeralScene.ScreemOffset = this.LevelParts[this.CurrentPartLevel - 1].ScreemOffset;
            this.GeralScene.LevelSize = this.LevelParts[this.CurrentPartLevel - 1].LevelSize;
            this.GeralScene.addLayers();
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        
            if (this.CurrentStatus == GameManagement.Status.PLAYING)
                this.GeralScene.Update(gameTime);


            if (this.CurrentStatus == GameManagement.Status.PLAYING)
            {
                if (this.CheckPlayerPart(this.CurrentPartLevel))
                {
                    int _newPartOfLevel = this.checkOfPlayer();
                    if (_newPartOfLevel != this.CurrentPartLevel)
                    {
                        this.CurrentStatus = GameManagement.Status.STOP;
                        this.AddPartOfLevelOnGeralScene(_newPartOfLevel);

                        this.GeralScene.LevelSize = this.LevelParts[_newPartOfLevel - 1].LevelSize;
                        this.GeralScene.ScreemOffset = this.LevelParts[_newPartOfLevel - 1].ScreemOffset;

                        Point _playerSize = this.GeralScene.Players[0].size;
                        Vector2 _playerPosition = this.GeralScene.Players[0].Position;
                        Vector2 _cameraOrigin = this.GeralScene.Camera.Origin;
                        Vector2 _cameraPosition = new Vector2(this.GeralScene.Camera.Position.X - _cameraOrigin.X, this.GeralScene.Camera.Position.Y - _cameraOrigin.Y);

                        this.GeralScene.Camera.Target = new Vector2(
                            _cameraPosition.X < _playerPosition.X ? _playerPosition.X + _playerSize.X + _cameraOrigin.X : _playerPosition.X - _cameraOrigin.X,
                            this.GeralScene.Players[0].Position.Y
                            );

                        wait(0.5f, new Action(() =>
                        {
                            this.CurrentPartLevel = _newPartOfLevel;
                            this.LoadLevel();
                            this.SetLevel();
                            this.CurrentStatus = GameManagement.Status.PLAYING;
                        }));
                    }
                }
            }

            if(this.CurrentStatus == GameManagement.Status.STOP)
            {
                this.GeralScene.Camera.moveX((float)gameTime.ElapsedGameTime.TotalSeconds);
                this.GeralScene.Camera.moveY((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            this.GeralScene.Draw(spriteBatch, this.game.GraphicsDevice, new Vector2(this.game.GraphicsDevice.Viewport.Width, this.game.GraphicsDevice.Viewport.Height));
        }
    }
}
