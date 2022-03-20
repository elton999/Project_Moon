using System;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Entities.Actors;

namespace GameProject.Gameplay
{
    public class Weapon
    {
        private Actor _actor;

        public Weapon(Actor actor)
        {
            _actor = actor;
        }

        private float BulletVelocity = -0.2f;
        private int _swingBullet = 1;

        // Cadence
        private float Cadence = 0.5f;
        private float timerPressed = 0;
        private bool firstBullet = false;

        private Vector2 _weaponPositionNormal = new Vector2(20, 5);
        private Vector2 _weaponPositionUp = new Vector2(10, -10);

        public bool IsFire = false;
        public bool UpShoot = false;

        public void Update(GameTime gameTime, bool fire)
        {
            if (fire)
            {
                IsFire = true;
                timerPressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timerPressed >= Cadence || !firstBullet)
                {
                    firstBullet = true;
                    timerPressed = 0;
                    Shoot();
                }
            }
            else
            {
                IsFire = false;
                timerPressed = 0;
                firstBullet = false;
            }
        }

        public void Shoot()
        {
            Bullet bullet = CreateBullet();
            bullet.Position = GetRandomBulletPosition();
            bullet.Start();
        }

        private Vector2 GetRandomBulletPosition()
        {
            Vector2 randomPosition = Vector2.UnitY * (new Random()).Next(-_swingBullet, _swingBullet);

            if (!UpShoot)
                randomPosition += _actor.spriteEffect == SpriteEffects.None ? _weaponPositionNormal : _weaponPositionNormal * new Vector2(-1, 1);
            else
                randomPosition += _actor.spriteEffect == SpriteEffects.None ? _weaponPositionUp : _weaponPositionUp * new Vector2(-1, 1);

            return _actor.Position + randomPosition;
        }

        private Bullet CreateBullet()
        {
            var bullet = new Bullet();
            bullet.Scene = _actor.Scene;
            bullet.spriteEffect = _actor.spriteEffect;

            float velocityX = _actor.spriteEffect == SpriteEffects.None ? -BulletVelocity : BulletVelocity;
            float velocityY = UpShoot ? BulletVelocity : 0;
            bullet.velocity = new Vector2(velocityX, velocityY);
            return bullet;
        }
    }
}