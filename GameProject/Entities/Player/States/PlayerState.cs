using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public abstract class PlayerState : EntityState
    {
        public Player Player;
        protected Vector2 _direction;
        protected bool _jumpButtonReleased = true;

        public override void Enter() => InputUpdate();

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();

            _jumpButtonReleased = keyboard.IsKeyUp(Keys.Z);

            if (keyboard.IsKeyDown(Keys.X))
                Player.SwitchState(new PlayerStateShoot());

            float power = (float)Player.Scene.GameManagement.Values["POWER"];
            if (keyboard.IsKeyDown(Keys.C) && power > 0f)
                Player.SwitchBehavior(new Behavior.PlayerFlying(Player, Player, Player.AsepriteAnimation));
        }

        public override void LogicUpdate(GameTime gameTime){}

        public override void PhysicsUpdate(GameTime gameTime)
        {
            Player.Behavior.Move(gameTime, _direction * Player.SpeedIncrement);
            
            if (Player.IsGrounded && Player.IsFlying)
                Player.SwitchBehavior(new Behavior.PlayerOnGrounded(Player, Player, Player.AsepriteAnimation));
        }

        public override void Exit(){}
    }
}
