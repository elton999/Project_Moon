using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Sprite;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateFall : PlayerState
    {
        public override void LogicUpdate(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Player.AsepriteAnimation.Play(gameTime, "jump-jetpack", animationDirection);
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            if (Player.IsGrounded)
            {
                Player.SwitchState(new PlayerStateIdle());
            }
        }
    }
}
