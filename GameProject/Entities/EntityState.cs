using Microsoft.Xna.Framework;
namespace GameProject.Entities
{
    public abstract class EntityState
    {
        public abstract void Enter();
        public abstract void LogicUpdate(GameTime gameTime);
        public abstract void PhysicsUpdate(GameTime gameTime);
        public abstract void InputUpdate();
        public abstract void Exit();
    }
}
