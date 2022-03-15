using System;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Player.States
{
    public abstract class PlayerState : EntityState
    {
        public Player Player;
        protected Vector2 _direction;
        public override void Enter()
        {
            base.Enter();
            InputUpdate();
        }

        public override void PhysicsUpdate(GameTime gameTime)
        {
            Player.velocity.X = _direction.X * Player.SpeedIncrement;
        }
    }
}
