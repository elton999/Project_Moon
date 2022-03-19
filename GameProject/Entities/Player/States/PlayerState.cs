using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Player.States
{
    public abstract class PlayerState : EntityState
    {
        public Player Player;
        protected Vector2 _direction;
        protected bool _jumpButtonReleased = true;

        public override void Enter()
        {
            InputUpdate();
        }

        public override void InputUpdate()
        {
            var keyboard = Keyboard.GetState();

            _jumpButtonReleased = keyboard.IsKeyUp(Keys.Z);

            if (keyboard.IsKeyDown(Keys.X))
            {
                Player.SwitchState(new PlayerStateShoot());
            }
        }

        public override void LogicUpdate(GameTime gameTime){}
        
        public override void PhysicsUpdate(GameTime gameTime)
        {
            Player.velocity.X = _direction.X * Player.SpeedIncrement;
        }

        public override void Exit(){}
    }
}
