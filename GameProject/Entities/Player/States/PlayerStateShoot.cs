using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Sprite;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateShoot : PlayerState
    {
        private bool _shooting = true;
        private bool _buttonUpPressed = false;

        public override void Enter()
        {
            base.Enter();
            Player.velocity.X = 0;
        }

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();

            _shooting = keyboard.IsKeyDown(Keys.X);
            _buttonUpPressed = keyboard.IsKeyDown(Keys.Up);
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            base.LogicUpdate(gameTime);

            Player.AsepriteAnimation.Play(gameTime, "jump-jetpack", AsepriteAnimation.AnimationDirection.LOOP);

            if (_shooting)
            {
                Player.Weapon.UpShoot = _buttonUpPressed;
                Player.Weapon.Update(gameTime, true);
            }

            if(!_shooting)
            {
                Player.SwitchState(new PlayerStateIdle());
            }
        }
    }
}
