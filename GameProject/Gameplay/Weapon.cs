using System;
using System.Linq;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Entities;
using GameProject.Entities.Actors;

namespace GameProject.Gameplay
{
    public class Weapon
    {
        private Actor _actor;

        public Weapon(Actor actor)
        {
            this._actor = actor;
        }

        private float BulletVelocity = -0.2f;
        public bool IsFire = false;
        private int _swingBullet = 2;

        // Cadence
        private float Cadence = 0.5f;
        private float timerPressed = 0;
        private bool firstBullet = false;

        public void Update(GameTime gameTime, bool fire)
        {
            if (fire)
            {
                this.IsFire = true;
                timerPressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timerPressed >= Cadence || !firstBullet)
                {
                    firstBullet = true;
                    timerPressed = 0;
                    this.Shoot();
                }
            }
            else
            {
                this.IsFire = false;
                timerPressed = 0;
                firstBullet = false;
            }
        }

        public bool UpShoot = false;
        public void Shoot()
        {
            var bullet = new Bullet();
            bullet.Scene = this._actor.Scene;
            bullet.spriteEffect = this._actor.spriteEffect;

            float velocityX = this._actor.spriteEffect == SpriteEffects.None ? -this.BulletVelocity : this.BulletVelocity;
            float velocityY = this.UpShoot ? this.BulletVelocity : 0;
            bullet.velocity = new Vector2(velocityX, velocityY);

            if (this._actor.spriteEffect == SpriteEffects.None)
            {
                if (!this.UpShoot)
                    bullet.Position = new Vector2(
                        this._actor.Position.X + 20,
                        this._actor.Position.Y + 10 + (new Random()).Next(-this._swingBullet, this._swingBullet)
                    );
                else
                    bullet.Position = new Vector2(
                        this._actor.Position.X + 10,
                        this._actor.Position.Y - 10 + (new Random()).Next(-this._swingBullet, this._swingBullet)
                    );
                this._actor.moveX(-10);
                //SmashEfx(true);
            }
            else
            {
                if (!this.UpShoot)
                    bullet.Position = new Vector2(this._actor.Position.X - 20, this._actor.Position.Y + 10 + (new Random()).Next(-this._swingBullet, this._swingBullet));
                else
                    bullet.Position = new Vector2(this._actor.Position.X - 10, this._actor.Position.Y - 10 + (new Random()).Next(-this._swingBullet, this._swingBullet));
                this._actor.moveX(10);
                //SmashEfx(true);
            }
            bullet.Start();
        }
    }
}