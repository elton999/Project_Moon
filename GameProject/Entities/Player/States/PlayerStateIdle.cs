using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Sprite;

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

            if(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.Left))
            {
                Player.SwitchState(new PlayerStateWalk());
            }

            if(keyboard.IsKeyDown(Keys.Z) && _jumpButtonReleased)
            {
                Player.SwitchState(new PlayerStateJump());
            }
            base.InputUpdate();
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Player.AsepriteAnimation.Play(gameTime, "idle-jetpack", animationDirection);
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            if (!Player.IsGrounded)
            {
                Player.SwitchState(new PlayerStateFall());
            }
        }
    }
}
