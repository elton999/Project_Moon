using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Sprite;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateJump : PlayerState
    {
        private float _timer = 50.5f;
        private bool _canJump = false;

        public override void Enter()
        {
            base.Enter();
            _direction = Vector2.UnitX * Math.Sign(Player.velocity.X);
            jumpDash();
        }

        private void jumpDash()
        {
            Player.velocity.X = _direction.X * Player.DashJumpForce;
        }

        public override void InputUpdate()
        {
            base.InputUpdate();
            var keyboard = Keyboard.GetState();
            _canJump = keyboard.IsKeyDown(Keys.Z) && _timer > 0;
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Player.AsepriteAnimation.Play(gameTime, "jump", animationDirection);
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            if(_canJump)
            {
                jumpLogic();
                return;
            }

            Player.SwitchState(new PlayerStateFall());
        }

        private void jumpLogic()
        {
            float g = (2f * Player.JumpForce) / ((float)Math.Pow(0.08f, 2f));
            float initJumpVelocity = (float)Math.Sqrt(2.0f * g * Player.JumpForce);
            Player.velocity.Y = -initJumpVelocity;
        }
    }
}
