using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Actors.Enemies
{
    public class Bat : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "bat";

            this.Scene.AllActors.Add(this);
            this.size = new Point(10, 10);
            this._Speed = 5;

            this.InitialPosition = this.Position;
            this._PositionIdleFly = this.Position;

            this._XCon = getRandom.Next(5, 15);
            this._YCon = getRandom.Next(15, 25);

            this.CurrentStatus = (Status)int.Parse(this.Values["behavior"]);
            this.InitialStatus = this.CurrentStatus;

            this._AreaCheck = new Actor();
            this._AreaCheck.tag = "hitbox";
            this._AreaCheck.Position = this.Position;
            this._AreaCheck.size = new Point(3 * 8, 13 * 8);

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolsKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Black;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        float _YCon = 20;
        float _XCon = 10;

        private enum Status { IDLE, IDLE_FLYING, ATTACK, }
        private Status CurrentStatus = Status.IDLE;
        private Status InitialStatus;
        private Vector2 _PositionIdleFly;
        private Vector2 _PositionToAttack;

        private Actor _AreaCheck;

        public override void UpdateData(GameTime gameTime)
        {
            if (this.isLive)
            {
                Actor _player = this.Scene.AllActors[0];
                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    _player.OnCollision(this.tag);

                if (this.CurrentStatus == Status.IDLE)
                {
                    this._AreaCheck.Position = new Vector2(this.Position.X - (3 * 8 / 2), this.Position.Y);

                    if (_player.overlapCheckPixel(_AreaCheck))
                    {
                        this._PositionToAttack = new Vector2(_player.Position.X + 5, _player.Position.Y + 10);
                        this.CurrentStatus = Status.ATTACK;
                    }
                }

                if (this.CurrentStatus == Status.IDLE_FLYING)
                {
                    this.Position.Y = this._PositionIdleFly.Y + (int)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds * 0.001f * this._Speed) * _YCon);
                    this.Position.X = this._PositionIdleFly.X + (int)(Math.Cos(gameTime.TotalGameTime.TotalMilliseconds * 0.001f * this._Speed) * _XCon);
                }

                if (this.CurrentStatus == Status.ATTACK)
                {
                    if (this.Position.Y > this._PositionToAttack.Y + 5 || this.Position.Y < this._PositionToAttack.Y - 5)
                    {
                        this.Position.Y = lerp(this.Position.Y, this._PositionToAttack.Y, ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f * this._Speed));
                        this.Position.X = lerp(this.Position.X, this._PositionToAttack.X, ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f * this._Speed));

                    }
                    else
                    {
                        this.CurrentStatus = Status.IDLE_FLYING;
                        this._PositionIdleFly = this.Position;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
