using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateIdle : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            Player.velocity.X = 0;
        }

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.Down))
                Player.SwitchState(new PlayerStateMove());

            if (CanJump)
                Player.SwitchState(new PlayerStateJump());

            if (ButtonDown && !Player.IsFlying)
                Player.SwitchState(new PlayerSquatState());

            base.InputUpdate();
        }

        public override void LogicUpdate(GameTime gameTime) => Player.Behavior.Idle(gameTime);

        public override void PhysicsUpdate(GameTime gameTime)
        {
            base.PhysicsUpdate(gameTime);
            if (!Player.IsGrounded && !Player.IsFlying)
                Player.SwitchState(new PlayerStateFall());
        }
    }
}
