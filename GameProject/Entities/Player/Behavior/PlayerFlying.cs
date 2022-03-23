using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using GameProject.Entities.Actors.Behavior;
using GameProject.Entities.Player.Interfaces;

namespace GameProject.Entities.Player.Behavior
{
    public class PlayerFlying : IBehaviorAdapter, IPlayerReference
    {
        public Player Player { get; set; }
        public Actor Actor { get; set; }
        public AsepriteAnimation Animation { get; set; }

        public PlayerFlying(Actor actor, Player player, AsepriteAnimation animation)
        {
            Player = player;
            Actor = actor;
            Animation = animation;

            Player.IsFlying = true;
            Actor.moveY(-2);

            Actor.gravity2D = Vector2.Zero;
            Actor.velocityDecrecentY = 0;
        }

        public void Idle(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public void Walk(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public void Move(GameTime gameTime, Vector2 speed)
        {
            Jetpack.CheckFuel(gameTime, Player);

            Actor.velocityDecrecentY = 0;
            Actor.velocity.Y = -1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Actor.velocity.Y += speed.Y;
            Actor.velocity.X = speed.X;
        }

        public void Attack(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public void Fall(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.LOOP;
            Animation.Play(gameTime, "jump", animationDirection);
        }

        public void Jump(GameTime gameTime) { }
    }
}
