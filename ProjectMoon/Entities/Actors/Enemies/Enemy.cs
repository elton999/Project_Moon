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
    class Enemy : UmbrellaToolKit.Collision.Actor
    {

        public UmbrellaToolKit.Sprite.Square Box;
        public bool isLive = true;
        public override void Start()
        {
            base.Start();

            this.tag = "enemy";

            this.Scene.AllActors.Add(this);
            this.size = new Point(10, 32);

            this.Box = new UmbrellaToolKit.Sprite.Square();
            this.Box.Position = this.Position;
            this.Box.size = this.size;
            this.Box.SquareColor = Color.Magenta;
            this.Box.Scene = this.Scene;

            this.Box.Start();

            this.gravity2D = new Vector2(0, this.GravityY);
            this.velocityDecrecentY = 2050;
            this.velocityDecrecentX = 0;
        }

        public override void restart()
        {
            base.restart();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.isLive) {
                base.Update(gameTime);
                this.Box.Position = this.Position;

                if (this._StartAttack && !this._waitAttack)
                {
                    this._waitAttack = true;
                    this._Speed = this.Scene.AllActors[0].Position.X < this.Position.X ? _Speed : -_Speed;
                    wait(this._TimeToAttack, new Action(() => { this._Attack = true; }));
                }
            }
        }

        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);
            if (tag == "bullet")
            {
                this.isLive = false;
                this.active = false;
                this.restart();
            }
        }

        private float GravityY = -200f;
        private float _Speed = 60;
        public override void UpdateData(GameTime gameTime)
        {
            if (this.isLive) {
                if (this._Attack)
                {
                    this.velocity.X = _Speed;
                    if (this.CheckPath())
                        this._Speed = -this._Speed;
                }

                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    this.Scene.AllActors[0].OnCollision(this.tag);

                base.UpdateData(gameTime);
            }
        }

        public bool CheckPath()
        {
            Actor _actor = new Actor();
            _actor.Position = this.Position;
            _actor.size = this.size;

            if (this._Speed > 0)
            {
                _actor.size = new Point(1, 1);
                _actor.Position = new Vector2(this.Position.X -3, this.Position.Y+this.size.Y - 8);
                // check wall grid
                if (this.Scene.Grid.checkOverlap(_actor.size, _actor.Position, _actor, true))
                    return true;
                // check wall
                foreach (Solid solid in this.Scene.AllSolids)
                    if (solid.check(this.size, new Vector2(this.Position.X - 1, this.Position.Y)))
                        return true;

                _actor.Position = new Vector2(this.Position.X + this.size.X / 2f, this.Position.Y + this.size.Y + 16);
                // check ground grid
                if (!this.Scene.Grid.checkOverlap(_actor.size, _actor.Position, _actor))
                    return true;
                // check ground
                foreach (Solid solid in this.Scene.AllSolids)
                    if (!solid.check(this.size, new Vector2(this.Position.X - this.size.X, this.Position.Y + 1)))
                        return true;

            } else
            {
                _actor.size = new Point(1, 1);
                _actor.Position = new Vector2(this.Position.X + this.size.X + 3, this.Position.Y + this.size.Y - 8);
                // check wall grid
                if (this.Scene.Grid.checkOverlap(_actor.size, _actor.Position, _actor, false))
                    return true;
                // check wall
                foreach (Solid solid in this.Scene.AllSolids)
                    if (solid.check(this.size, new Vector2(this.Position.X + 1, this.Position.Y)))
                        return true;

                _actor.Position = new Vector2(this.Position.X + this.size.X, this.Position.Y + this.size.Y + 16);
                // check ground grid
                if (!this.Scene.Grid.checkOverlap(_actor.size, _actor.Position, _actor, true))
                    return true;
                // check ground
                foreach (Solid solid in this.Scene.AllSolids)
                    if (solid.check(this.size, new Vector2(this.Position.X + this.size.X, this.Position.Y + 1)))
                        return true;
            }

            return false;
        }

        private bool _StartAttack = false;
        private bool _waitAttack = false;
        private bool _Attack = false;
        private float _TimeToAttack = 1;

        public override void Isvisible()
        {
            if (this.isLive) {
                this._StartAttack = true;
                base.Isvisible();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isLive) {
                base.Draw(spriteBatch);
                this.Box.Draw(spriteBatch);
            }
        }

    }
}
