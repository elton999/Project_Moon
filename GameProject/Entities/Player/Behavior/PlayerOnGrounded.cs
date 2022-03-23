using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using GameProject.Entities.Actors.Behavior;

namespace GameProject.Entities.Player.Behavior
{
    public class PlayerOnGrounded : Jetpack, IBehaviorAdapter
    {
        private readonly float _gravityY = 0.0008f;
        public Actor Actor { get; set; }
        public AsepriteAnimation Animation { get; set; }

        public PlayerOnGrounded(Actor actor, Player player, AsepriteAnimation animation)
        {
            Actor = actor;
            Animation = animation;
            Player = player;
            Player.IsFlying = false;

            RechargeFuel();
            
            Actor.gravity2D = _gravityY * Vector2.UnitY;
            Actor.velocityDecrecentY = 2050;
        }

        public void Idle(GameTime gameTime)
        {
            // TODO: condicition for shoot and shoot-up animation
            var animationDirection = AsepriteAnimation.AnimationDirection.FORWARD;
            Animation.Play(gameTime, "idle", animationDirection);
        }

        public void Attack(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.FORWARD;
            Animation.Play(gameTime, "shoot", animationDirection);
        }

        public void Walk(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            if (Actor.velocity.X != 0)
                Animation.Play(gameTime, "walk", animationDirection);
            else
                Idle(gameTime);
        }

        public void Move(GameTime gametime, Vector2 speed)
        {
            Actor.velocity = speed * Vector2.UnitX + Actor.velocity * Vector2.UnitY;
        }

        public void Jump(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public void Fall(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "fall", animationDirection);
        }
    }
}
