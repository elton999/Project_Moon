using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;
using UmbrellaToolKit.Collision;

namespace ProjectMoon.Entities.Actors.Enemies
{
    public class Spider : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "spider";

            this.Scene.AllActors.Add(this);
            this.size = new Point(16, 16);
            this._Speed = 25;

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Red;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateData(GameTime gameTime)
        {
            //base.UpdateData(gameTime);
            if (this.isLive)
            {
                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    this.Scene.AllActors[0].OnCollision(this.tag);
            }
            this.CheckPath(gameTime);
        }

        private bool _currentMovimentX = false;
        private bool _currentMovimentY = false;
        public void CheckPath(GameTime gameTime)
        {
            _groundTop = false;
            _groundBottom = false;
            _groundRight = false;
            _groundLeft = false;

            this.isRidingGrid(this.Scene.Grid);

            if (!(_groundLeft && _groundRight && _groundTop && _groundBottom))
            {
                if (
                        (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X - 1, this.Position.Y + 1), this) ||
                        this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X + 1, this.Position.Y + 1), this))
                        && _currentMovimentX
                    )
                {
                    moveY(1, null);
                }

                if (
                        (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X - 1, this.Position.Y - 1), this) ||
                        this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X + 1, this.Position.Y - 1), this))
                        && _currentMovimentX
                    )
                {
                    moveY(-1, null);
                }

                if (
                        (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X + 1, this.Position.Y + 1), this) ||
                        this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X + 1, this.Position.Y - 1), this))
                        && _currentMovimentY
                    )
                {
                    moveX(1, null);
                }

                if (
                        (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X - 1, this.Position.Y + 1), this) ||
                        this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X - 1, this.Position.Y - 1), this))
                        && _currentMovimentY
                    )
                {
                    moveX(-1, null);
                }
            }

            _currentMovimentX = false;
            _currentMovimentY = false;

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_groundRight)
            {
                _currentMovimentY = true;
                //setAnimationMovement("moveR");
                moveY(t * -_Speed, (string _tag) => moveX(t * _Speed, null));
            }

            if (_groundLeft)
            {
                _currentMovimentY = true;
                //setAnimationMovement("moveL");
                moveY(t * _Speed, (string _tag) => moveX(t * -_Speed, null));
            }

            if (_groundBottom)
            {
                //setAnimationMovement("moveB");
                _currentMovimentX = true;
                moveX(t * _Speed, (string _tag) => moveY(t * -_Speed, null));
            }

            if (_groundTop)
            {
                //setAnimationMovement("moveT");
                _currentMovimentX = true;
                moveX(t * -_Speed, (string _tag) => moveY(t * _Speed, null));
            }
        }


        private bool _groundLeft = false;
        private bool _groundRight = false;
        private bool _groundTop = false;
        private bool _groundBottom = false;
        public override bool isRidingGrid(Grid grid)
        {
            bool rt = false;
            if (grid.checkOverlap(this.size, new Vector2(this.Position.X + 1, this.Position.Y), this))
            {
                rt = true;
                _groundRight = true;
            }
            else if (grid.checkOverlap(this.size, new Vector2(this.Position.X - 1, this.Position.Y), this))
            {
                rt = true;
                _groundLeft = true;
            }

            if (grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y + 1), this))
            {
                rt = true;
                _groundBottom = true;
            }
            else if (grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y - 1), this))
            {
                rt = true;
                _groundTop = true;
            }

            return rt;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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
