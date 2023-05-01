using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Player.Behavior
{
    public class PlayerOnGrounded : PlayerBasicBehavior
    {
        public PlayerOnGrounded(Actor actor, Player player, AsepriteAnimation animation) : base(actor, player, animation) { }

        public override void Idle(GameTime gameTime)
        {
            if (Player.CurrentState.ButtonShoot)
                Shoot(gameTime, Player.CurrentState.ButtonUP);
            else
                base.Idle(gameTime);
        }

        public override void Walk(GameTime gameTime)
        {
            if (Player.CurrentState.ButtonShoot)
                Shoot(gameTime, Player.CurrentState.ButtonUP);
            else
                base.Walk(gameTime);
        }

        public override void Move(GameTime gametime, Vector2 speed)
        {
            if (Player.IsGrounded && Player.CurrentState.ButtonShoot)
            {
                StopHorizontalMovement();
                return;
            }
            base.Move(gametime, speed);
        }

        public override void Jump(GameTime gameTime)
        {
            base.Jump(gameTime);
            if (Player.CurrentState.ButtonShoot)
                base.Shoot(gameTime, false);
        }

        public override void Fall(GameTime gameTime) => Jump(gameTime);

        public override void Shoot(GameTime gameTime, bool upShoot)
        {
            base.Shoot(gameTime, upShoot);

            if (Player.IsGrounded)
                ShootAnimation(gameTime, upShoot);
            else
                Jump(gameTime);
        }

        public void ShootAnimation(GameTime gameTime, bool upShoot)
        {
            StopHorizontalMovement();
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            string animationId = !upShoot ? _shootAnimationId : _shoot_upAnimationId;
            Animation.Play(gameTime, animationId, animationDirection);
        }

        private void StopHorizontalMovement() => Actor.velocity.X = 0;
    }
}
