using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;

namespace ProjectMoon.Entities.Actors.Enemies
{
    class Enemy : UmbrellaToolKit.Collision.Actor
    {

        public UmbrellaToolKit.Sprite.Square Box;
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
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Box.Position = this.Position;

            if (this._StartAttack && !this._waitAttack)
            {
                this._waitAttack = true;
                wait(this._TimeToAttack, new Action(() => { this._Attack = true; }));
            }
        }

        private float GravityY = -200f;
        private float _Speed = 60;
        public override void UpdateData(GameTime gameTime)
        {

            if (this._Attack) {
                this.velocity.X = this.Scene.AllActors[0].Position.X < this.Position.X ? _Speed : -_Speed;
            }

            if (this.overlapCheckPixel(this.Scene.AllActors[0]))
            {
                System.Console.WriteLine("ok");
                this.Scene.AllActors[0].OnCollision(this.tag);
            }

            base.UpdateData(gameTime);
        }

        private bool _StartAttack = false;
        private bool _waitAttack = false;
        private bool _Attack = false;
        private float _TimeToAttack = 3;

        public override void Isvisible()
        {
            this._StartAttack = true;
            base.Isvisible();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            this.Box.Draw(spriteBatch);
        }

    }
}
