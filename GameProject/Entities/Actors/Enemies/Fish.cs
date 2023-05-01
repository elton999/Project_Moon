using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Entities.Actors.Enemies
{
    public class Fish : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "fish";

            this.Scene.AllActors.Add(this);
            this.InitialPosition = this.Position;
            this.size = new Point(16, 16);
            this._Speed = 5;

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolsKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Orange;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        private bool _Attack = true;
        private bool _BackToInitialPosition = false;
        private float timer = 0;
        private float Duration = 1 * 1000;
        public override void UpdateData(GameTime gameTime)
        {
            float _deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds;

            if (_Attack && !_BackToInitialPosition)
            {
                timer += _deltaTime;
                if (timer < Duration)
                    this.Position.Y = EaseOutQuad(timer, this.InitialPosition.Y, this.Nodes[0].Y - this.InitialPosition.Y, Duration);
                else
                {
                    timer = 0;
                    _Attack = false;
                    _BackToInitialPosition = true;

                }
            }

            if (!_Attack && _BackToInitialPosition)
            {
                timer += _deltaTime;
                if (timer < Duration)
                    this.Position.Y = EaseInQuad(timer, this.Nodes[0].Y, -(this.Nodes[0].Y - this.InitialPosition.Y), Duration);
                else
                {
                    timer = 0;
                    _BackToInitialPosition = false;
                    wait(0.8f, () => { _Attack = true; });
                }
            }

            if (this.Scene.AllActors[0].overlapCheckPixel(this))
            {
                this.Scene.AllActors[0].OnCollision(this.tag);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.isLive)
            {
                base.Draw(spriteBatch);
                if (this.Scene.GameManagement.Values["DEBUG"])
                {
                    this.Box.Scene = this.Scene;
                    this.Box.Position = this.Position;
                    this.Box.Draw(spriteBatch);
                }
            }
        }
    }
}
