using Microsoft.Xna.Framework;

namespace GameProject.Entities.Player.States
{
    public class PlayerSquatState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void InputUpdate()
        {
            if (!ButtonDown)
                Player.SwitchState(new PlayerStateIdle());
        }

        public override void LogicUpdate(GameTime gameTime) => Player.Behavior.Squat(gameTime);
    }
}

