using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Collision;
using UmbrellaToolsKit.Sprite;

namespace GameProject.Entities.Actors.Behavior
{
    public interface IBehaviorAdapter
    {
        Actor Actor { get; set; }
        AsepriteAnimation Animation { get; set; }
        
        void Walk(GameTime gameTime);
        void Move(GameTime gamTime, Vector2 speed);
        void Idle(GameTime gameTime);
        void Jump(GameTime gameTime);
        void Fall(GameTime gameTime);
        void Attack(GameTime gameTime);
    }
}
