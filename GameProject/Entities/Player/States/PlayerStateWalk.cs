using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Sprite;

namespace GameProject.Entities.Player.States
{
    public class PlayerStateWalk : PlayerState
    {
        public override void InputUpdate()
        {
            _direction = Vector2.Zero;
            
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Right))
            {
                _direction = Vector2.UnitX * - 1f;
                Player.Flip(true);
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                _direction = Vector2.UnitX;
                Player.Flip(false);
            }

            if (keyboard.IsKeyDown(Keys.Z))
            {
                Player.SwitchState(new PlayerStateJump());
            }
        }

        public override void LogicUpdate(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Player.AsepriteAnimation.Play(gameTime, "walk-jetpack", animationDirection);

            if (_direction != null && _direction.X == 0)
            {
                Player.SwitchState(new PlayerStateIdle());
            }
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            base.PhysicsUpdate(gameTime);

            if (!Player.IsGrounded)
            {
                Player.SwitchState(new PlayerStateFall());
            }
        }

    }
}
