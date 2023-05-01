using Microsoft.Xna.Framework;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateFall : PlayerState
    {
        public override void LogicUpdate(GameTime gameTime) => Player.Behavior.Fall(gameTime);

        public override void InputUpdate() { }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            if (Player.IsGrounded)
                Player.SwitchState(new PlayerStateIdle());
        }

        public override void Exit()
        {
            base.Exit();
            Player.SpriteDeformer.Squash();
        }
    }
}
