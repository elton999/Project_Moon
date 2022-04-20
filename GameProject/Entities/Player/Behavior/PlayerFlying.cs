using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Player.Behavior
{
    public class PlayerFlying : PlayerBasicBehavior
    {
        public PlayerFlying(Actor actor, Player player, AsepriteAnimation animation) : base(actor, player, animation)
        {
            Player.IsFlying = true;
            Actor.moveY(-2);

            Actor.gravity2D = Vector2.Zero;
            Actor.velocityDecrecentY = 0;
        }

        public override void Idle(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);

            if (Player.CurrentState.ButtonShoot)
                Shoot(gameTime, Player.CurrentState.ButtonUP);
        }

        public override void Walk(GameTime gameTime)
        {
            if (Player.CurrentState.ButtonShoot)
                Shoot(gameTime, Player.CurrentState.ButtonUP);

            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public override void Shoot(GameTime gameTime, bool upShoot)
        {
            upShoot = false;
            base.Shoot(gameTime, upShoot);
        }

        public override void Move(GameTime gameTime, Vector2 speed)
        {
            CheckFuel(gameTime);
            TimeOfFly += gameTime.ElapsedGameTime.Milliseconds;

            Actor.velocityDecrecentY = 0;
            Actor.velocity.Y = -0.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Actor.velocity.Y += speed.Y;
            Actor.velocity.X = speed.X;
        }
    }
}
