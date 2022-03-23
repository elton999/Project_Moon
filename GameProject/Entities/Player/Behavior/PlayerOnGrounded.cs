using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using GameProject.Entities.Actors.Behavior;
using GameProject.Entities.Player.Interfaces;

namespace GameProject.Entities.Player.Behavior
{
    public class PlayerOnGrounded : BasicBehaviorAdapter
    {
        private float _gravityY = 0.0008f;
        public Player Player { get; set; }

        public PlayerOnGrounded(Actor actor, Player player, AsepriteAnimation animation) : base(actor, animation)
        {
            Player = player;
            Player.IsFlying = false;

            Jetpack.RechargeFuel(Player);
            
            Actor.gravity2D = _gravityY * Vector2.UnitY;
            Actor.velocityDecrecentY = 2050;
        }

        public override void Attack(GameTime gameTime)
        {
            var animationDirection = AsepriteAnimation.AnimationDirection.FORWARD;
            Animation.Play(gameTime, "shoot", animationDirection);
        }

        public override void Idle(GameTime gameTime)
        {
            // TODO: condicition for shoot and shoot-up animation
            base.Idle(gameTime);
        }
    }
}
