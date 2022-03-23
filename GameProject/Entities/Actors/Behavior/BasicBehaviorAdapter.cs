using UmbrellaToolsKit.Collision;
using UmbrellaToolsKit.Sprite;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Actors.Behavior
{
    public class BasicBehaviorAdapter : IBehaviorAdapter
    {
        public Actor Actor { get; set; }
        public AsepriteAnimation Animation { get; set; }

        public BasicBehaviorAdapter(Actor actor, AsepriteAnimation animation)
        {
            Actor = actor;
            Animation = animation;
        }
        public virtual void Idle(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "idle", animationDirection);
        }

        public virtual void Walk(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            if (Actor.velocity.X != 0)
                Animation.Play(gameTime, "walk", animationDirection);
            else
                Idle(gameTime);
        }

        public virtual void Move(GameTime gametime, Vector2 speed)
        {
            Actor.velocity = speed * Vector2.UnitX + Actor.velocity * Vector2.UnitY;
        }

        public virtual void Attack(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.FORWARD;
            Animation.Play(gameTime, "attack", animationDirection);
        }

        public virtual void Jump(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public virtual void Fall(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "fall", animationDirection);
        }
    }
}
