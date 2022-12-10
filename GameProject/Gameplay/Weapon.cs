using System;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Entities.Actors;
using GameProject.Entities.Actors.Interfaces;

namespace GameProject.Gameplay
{
    public class Weapon : IActorReference
    {
        public Actor Actor { get; set; }

        public Weapon(Actor actor) => Actor = actor;

        private float _bulletVelocity = -0.2f;
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
                return;
            }

            if (!fire)
            {
                IsFire = false;
                timerPressed = 0;
                firstBullet = false;
            }
        }

        public void Shoot()
        {
            Bullet bullet = Bullet.CreateBullet(Actor, GetBulletOrientation());
            bullet.Position = GetRandomBulletPosition();
            bullet.Start();
        }

        private Vector2 GetBulletOrientation()
        {
            Vector2 bulletVelocity = Vector2.One;
            bulletVelocity.X = Actor.spriteEffect == SpriteEffects.None ? -_bulletVelocity : _bulletVelocity;
            bulletVelocity.Y = UpShoot ? _bulletVelocity : 0;
            return bulletVelocity;
        }

        public Vector2 GetRandomBulletPosition()
        {
            Vector2 randomPosition = Vector2.UnitY * (new Random()).Next(-_swingBullet, _swingBullet);

            if (!UpShoot)
                randomPosition += Actor.spriteEffect == SpriteEffects.None ? _weaponPositionNormal : _weaponPositionNormal * new Vector2(-1, 1);

            if (UpShoot)
                randomPosition += Actor.spriteEffect == SpriteEffects.None ? _weaponPositionUp : _weaponPositionUp * new Vector2(-1, 1);

            return Actor.Position + randomPosition;
        }


    }
}