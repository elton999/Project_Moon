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

        public GameManagement GameManagement;
        public AssetManagement AssetManagement;

        public Game1()
        {
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
            AssetManagement.Set<Entities.Player>("player", "PLAYER");
            AssetManagement.Set<Entities.Actors.Victim>("victim", "MIDDLEGROUND");
            AssetManagement.Set<Gameplay.HitBoxDamage>("damage", "MIDDLEGROUND");
            AssetManagement.Set<Entities.Actors.Enemies.Soldier>("soldier", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Spider>("spider", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Bat>("bat", "ENEMIES");
            AssetManagement.Set<Entities.Actors.Enemies.Jumper>("jumper", "ENEMIES");

            this.GameManagement = new GameManagement();
            this.GameManagement.game = this;
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

            this.GameManagement.Render(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
