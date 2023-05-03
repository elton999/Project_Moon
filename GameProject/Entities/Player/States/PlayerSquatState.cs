using Microsoft.Xna.Framework;

namespace GameProject.Entities.Player.States
{
    public class PlayerSquatState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            Player.size = new Point(10, 26);
            Player.Origin = new Vector2(27, 23);
            Player.Position += Vector2.UnitY * 7;
        }

        public override void InputUpdate()
        {
            if (!ButtonDown)
                Player.SwitchState(new PlayerStateIdle());
        }

        public override void LogicUpdate(GameTime gameTime) => Player.Behavior.Squat(gameTime);

        public override void Exit()
        {
            Player.size = new Point(10, 32);
            Player.Origin = new Vector2(27, 16);
            Player.Position -= Vector2.UnitY * 7;
            base.Exit();
        }
    }
}

