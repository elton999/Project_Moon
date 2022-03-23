using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateIdle : PlayerState
    {
        private bool _buttonUpPressed = false;

        public override void Enter()
        {
            base.Enter();
            Player.velocity.X = 0;
        }

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();

            _buttonUpPressed = keyboard.IsKeyDown(Keys.Up);

            // TODO: colocar as keys em um array
            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.Down))
                Player.SwitchState(new PlayerStateWalk());

            if(keyboard.IsKeyDown(Keys.Z) && _jumpButtonReleased)
                Player.SwitchState(new PlayerStateJump());

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
