using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateWalk : PlayerState
    {
        public override void InputUpdate()
        {
            _direction = Vector2.Zero;
            
            var keyboard = Keyboard.GetState();
            
            if (keyboard.IsKeyDown(Keys.Right))
            {
                _direction = Vector2.UnitX * - 1f;
                Player.Flip(true);
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                _direction = Vector2.UnitX;
                Player.Flip(false);
            }

            if (keyboard.IsKeyDown(Keys.Up))
                _direction.Y = 1;

            if (keyboard.IsKeyDown(Keys.Down))
                _direction.Y = -1;


            if (keyboard.IsKeyDown(Keys.Z) && _jumpButtonReleased)
                Player.SwitchState(new PlayerStateJump());

            base.InputUpdate();
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            Player.Behavior.Walk(gameTime);

            if (_direction != null && _direction.X == 0)
                Player.SwitchState(new PlayerStateIdle());
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            base.PhysicsUpdate(gameTime);

            if (!Player.IsGrounded && !Player.IsFlying)
                Player.SwitchState(new PlayerStateFall());
        }

    }
}
