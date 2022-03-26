using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public abstract class PlayerState : EntityState
    {
        public Player Player;
        protected Vector2 _direction;
        protected bool _jumpButtonReleased = true;
        protected bool _isUpbuttonPressed = false;

        public bool CanJump { get => ButtonJump && _jumpButtonReleased && !Player.IsFlying; }
        public bool ButtonJump { get => Keyboard.GetState().IsKeyDown(Keys.Z); }

        public bool ButtonUP { get => Keyboard.GetState().IsKeyDown(Keys.Up); }
        public bool ButtonDown { get => Keyboard.GetState().IsKeyDown(Keys.Down); }
        public bool ButtonRight { get => Keyboard.GetState().IsKeyDown(Keys.Right); }
        public bool ButtonLeft { get => Keyboard.GetState().IsKeyDown(Keys.Left); }

        public bool ButtonFlyPressed { get => Keyboard.GetState().IsKeyDown(Keys.C); }
        public bool ButtonFlyReleassed { get => Keyboard.GetState().IsKeyUp(Keys.C); }

        public bool ButtonShoot { get => Keyboard.GetState().IsKeyDown(Keys.X); }

        public override void Enter() => InputUpdate();

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();
            _jumpButtonReleased = keyboard.IsKeyUp(Keys.Z);

            float power = (float)Player.Scene.GameManagement.Values["POWER"];
            if (ButtonFlyPressed && power > 0f && !Player.IsFlying)
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
