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

        protected string _idleAnimationId = "idle";
        protected string _walkAnimationId = "walk";
        protected string _fallingAnimationId = "fall";
        protected string _jumpAnimationId = "jump";
        protected string _shoot_upAnimationId = "shoot-up";
        protected string _shootAnimationId = "shoot";
        protected string _squatAnimationId = "squat";

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
            if (_directionY >= 0)
                Animation.Play(gameTime, _idleAnimationId, animationDirection);
            else
                Animation.Play(gameTime, _shoot_upAnimationId, animationDirection);
        }

        public virtual void Walk(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            if (Actor.velocity.X != 0)
                Animation.Play(gameTime, _walkAnimationId, animationDirection);
            else
                Idle(gameTime);
        }

        public virtual void Shoot(GameTime gameTime, bool upShoot)
        {
            Weapon.UpShoot = upShoot;
            Weapon.Update(gameTime, true);
        }

        public virtual void Squat(GameTime gameTime) => Animation.Play(gameTime, _squatAnimationId);

        public virtual void Move(GameTime gametime, Vector2 speed)
        {
            TimeOnGround += (float)gametime.ElapsedGameTime.Milliseconds;
            _directionY = Math.Sign(speed.Y);
            Actor.velocity = speed * Vector2.UnitX + Actor.velocity * Vector2.UnitY;
        }

        public virtual void Jump(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, _jumpAnimationId, animationDirection);
        }

        public virtual void Fall(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, _fallingAnimationId, animationDirection);
        }

        public void Attack(GameTime gameTime) { }
    }
}
