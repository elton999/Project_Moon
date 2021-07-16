using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolKit;

namespace ProjectMoon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameManagementGame GameManagement;
        public AssetManagement AssetManagement;

        public static Game1 Instance;

        public Game1()
        {
            if (Instance == null)
                Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 426 * 4;
            _graphics.PreferredBackBufferHeight = 240 * 4;
            _graphics.ApplyChanges();

            AssetManagement = new AssetManagement();
            AssetManagement.Set<Entities.Player.Player>("player", "PLAYER");
            AssetManagement.Set<Entities.Actors.Victim>("victim", "MIDDLEGROUND");
            AssetManagement.Set<Gameplay.HitBoxDamage>("damage", "MIDDLEGROUND");

            // Enemies
            AssetManagement.Set<Entities.Actors.Enemies.Soldier>("soldier", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Spider>("spider", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Bat>("bat", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Jumper>("jumper", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Fish>("fish", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Shooter>("shooter", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Vomiter>("vomiter", "ENEMIES");

            this.GameManagement = new GameManagementGame();
            this.GameManagement.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this.GameManagement.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.GameManagement.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
