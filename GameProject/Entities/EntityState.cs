using Microsoft.Xna.Framework;
namespace GameProject.Entities
{
    public abstract class EntityState
    {
        public virtual void Enter() { }
        public virtual void LogicUpdate(GameTime gameTime) { }
        public virtual void PhysicsUpdate(GameTime gameTime) { }
        public virtual void InputUpdate() { }
        public virtual void Exit() { }
    }
}
