using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit;

namespace GameProject.UI.Gameplay
{
    public class HUD : UmbrellaToolsKit.Sprite.Square
    {
        public GameObject LifeText = new GameObject();
        public GameObject LifeCountSprite_ON = new GameObject();
        public GameObject LifeCountStripte_OFF = new GameObject();

        public GameObject PowerText = new GameObject();
        public GameObject StatusSprite_ON = new GameObject();
        public GameObject StatusSprite_OFF = new GameObject();

        public override void Start()
        {
            SquareColor = Color.Black;
            size = new Point(Scene.Sizes.X, 29);
            base.Start();

            Position = Scene.Sizes.ToVector2() * Vector2.UnitY - Sprite.Height * Vector2.UnitY;

            var sprite = Scene.Content.Load<Texture2D>("Sprites/tilemap");

            // HUD Texts
            LifeText.Sprite = sprite;
            LifeText.Position = new Vector2(10, this.Scene.Sizes.Y - this.Sprite.Height + 4);
            LifeText.Body = new Rectangle(new Point(0, 72), new Point(36, 8));

            PowerText.Sprite = sprite;
            PowerText.Position = new Vector2(66, this.Scene.Sizes.Y - this.Sprite.Height + 4);
            PowerText.Body = new Rectangle(new Point(0, 88), new Point(52, 8));

            LifeCountSprite_ON.Sprite = sprite;
            LifeCountSprite_ON.Body = new Rectangle(new Point(40, 72), new Point(10, 10));

            LifeCountStripte_OFF.Sprite = sprite;
            LifeCountStripte_OFF.Body = new Rectangle(new Point(48, 72), new Point(10, 10));

            StatusSprite_ON.Sprite = sprite;
            StatusSprite_ON.Body = new Rectangle(new Point(56, 88), new Point(4, 9));

            StatusSprite_OFF.Sprite = sprite;
            StatusSprite_OFF.Body = new Rectangle(new Point(61, 88), new Point(4, 9));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch, false);
            //background
            this.DrawSprite(spriteBatch);

            // Texts
            LifeText.DrawSprite(spriteBatch);
            PowerText.DrawSprite(spriteBatch);

            // life status
            for (int i = 0; i < Scene.GameManagement.Values["CURRENT_LIFES"]; i++)
            {
                LifeCountSprite_ON.Position = new Vector2(10 + (i * 11), Position.Y + 16);
                LifeCountSprite_ON.DrawSprite(spriteBatch);
            }

            // Fuel Status
            for (int i = 0; i < 10; i++)
            {
                float _PowerStatusFloat = Scene.GameManagement.Values["POWER"] / 10f;
                int _PowerStatus = (int)_PowerStatusFloat;

                StatusSprite_ON.Position = new Vector2(66 + (i * 5), Position.Y + 16);
                StatusSprite_OFF.Position = StatusSprite_ON.Position;

                if (i < _PowerStatus)
                    StatusSprite_ON.DrawSprite(spriteBatch);
                else
                    StatusSprite_OFF.DrawSprite(spriteBatch);
            }
            EndDraw(spriteBatch);
        }
    }
}
