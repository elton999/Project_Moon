using System;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using GameProject.Gameplay;
using GameProject.Entities.Actors.Behavior;

namespace GameProject.Entities.Player.Behavior
{
    public abstract class PlayerBasicBehavior : Jetpack, IBehaviorAdapter
    {
        private readonly float _gravityY = 0.0008f;
        private float _directionY = 0f;

        public Actor Actor { get; set; }
        public AsepriteAnimation Animation { get; set; }
        public Weapon Weapon;

        public PlayerBasicBehavior(Actor actor, Player player, AsepriteAnimation animation)
        {
            Actor = actor;
            Animation = animation;
            Player = player;
            Player.IsFlying = false;

            Weapon = new Weapon(Actor);

            RechargeFuel();

            Actor.gravity2D = _gravityY * Vector2.UnitY;
            Actor.velocityDecrecentY = 2050;
        }

        public virtual void Idle(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            if(_directionY >= 0)
                Animation.Play(gameTime, "idle", animationDirection);
            else
                Animation.Play(gameTime, "shoot-up", animationDirection);
        }

        public virtual void Walk(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            if (Actor.velocity.X != 0)
                Animation.Play(gameTime, "walk", animationDirection);
            else
                Idle(gameTime);
        }

        public virtual void Shoot(GameTime gameTime, bool upShoot)
        {
            Weapon.UpShoot = upShoot;
            Weapon.Update(gameTime, true);
        }

        public virtual void Move(GameTime gametime, Vector2 speed)
        {
            _directionY = Math.Sign(speed.Y);
            Actor.velocity = speed * Vector2.UnitX + Actor.velocity * Vector2.UnitY;
        }

        public virtual void Jump(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public virtual void Fall(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "fall", animationDirection);
        }

        public void Attack(GameTime gameTime) { }
    }
}
