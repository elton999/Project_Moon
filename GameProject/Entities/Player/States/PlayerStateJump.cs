using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateJump : PlayerState
    {
        private float _timer = 50.5f;
        private bool _canJump = false;

        public override void Enter()
        {
            _direction = Vector2.UnitX * Math.Sign(Player.velocity.X);
            Player.velocity.X = _direction.X * Player.DashJumpForce;
            base.Enter();
        }

        public override void InputUpdate()
        {
            base.InputUpdate();
            _canJump = ButtonJump && _timer > 0;
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Player.Behavior.Jump(gameTime);
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
