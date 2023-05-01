using Microsoft.Xna.Framework;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateWalk : PlayerState
    {
        public override void InputUpdate()
        {
            _direction = Vector2.Zero;

            if (ButtonRight)
            {
                _direction = Vector2.UnitX * -1f;
                Player.Flip(true);
            }
            else if (ButtonLeft)
            {
                _direction = Vector2.UnitX;
                Player.Flip(false);
            }

            if (ButtonUP)
                _direction.Y = 1;

            if (ButtonDown)
                _direction.Y = -1;

            if (CanJump && ButtonJump)
                Player.SwitchState(new PlayerStateJump());

            base.InputUpdate();
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            Player.Behavior.Walk(gameTime);
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            base.PhysicsUpdate(gameTime);

            if (!Player.IsGrounded && !Player.IsFlying)
                Player.SwitchState(new PlayerStateFall());

            if (!Player.IsFlying && _direction.Length() == 0)
                Player.SwitchState(new PlayerStateIdle());
        }

    }
}
