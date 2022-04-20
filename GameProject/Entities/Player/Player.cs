using System.Linq;
using System.Collections;
using GameProject.Gameplay;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Entities.Player.Behavior;

namespace GameProject.Entities.Player
{
    public class Player : PlayerAnimationEfx
    {
        public Weapon Weapon;
        public States.PlayerState CurrentState;
        public AsepriteAnimation AsepriteAnimation;

        public float JumpForce = 0.012f;
        public float DashJumpForce = 0.095f;
        public float SpeedIncrement = -0.05f;

        public bool IsFlying = false;
        public bool IsGrounded = false;

        public PlayerBasicBehavior Behavior;

        public override void Start()
        {
            base.Start();
            Scene.AllActors.Add(this);
            size = new Point(10, 32);
            tag = "player";
            Weapon = new Weapon(this);

            Sprite = Content.Load<Texture2D>("Sprites/Player/Regina");
            AsepriteAnimation = new AsepriteAnimation(Content.Load<AsepriteDefinitions>("Sprites/Player/ReginaAnimations"));
            Origin = new Vector2(27, 16);

            SwitchBehavior(new PlayerOnGrounded(this, this, AsepriteAnimation));

            SwitchState(new States.PlayerStateIdle());
        }

        public void SwitchBehavior(PlayerBasicBehavior behavior) => Behavior = behavior;

        public void SwitchState(States.PlayerState state)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            state.Player = this;
            state.Enter();
            CurrentState = state;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CurrentState.InputUpdate();
            CurrentState.LogicUpdate(gameTime);

            CoroutineManagement.Update(gameTime);
        }

        public override void UpdateData(GameTime gameTime)
        {
            CheckGrounded();

            CurrentState.PhysicsUpdate(gameTime);

            Scene.Camera.Target = Position + size.ToVector2() / 2f;
            base.UpdateData(gameTime);
        }

        #region physics
        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);

            if (_enemyTags.Contains(tag))
                TakeDamage();
        }

        public override bool isRiding(Solid solid)
        {
            if (solid.check(size, Position + Vector2.UnitY))
                return true;
            return false;
        }

        private void CheckGrounded()
        {
            IsGrounded = false;
            foreach (Solid solid in Scene.AllSolids)
                if (solid.check(size, Position + Vector2.UnitY))
                    IsGrounded = true;

            if (Scene.Grid.checkOverlap(size, Position + Vector2.UnitY, this))
                IsGrounded = true;
        }
        #endregion

        #region Animation and Render

        public void Flip(bool right) => spriteEffect = right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Body = AsepriteAnimation.Body;
            BeginDraw(spriteBatch);
            spriteBatch.Draw(
                Sprite,
                new Rectangle(Vector2.Subtract(Position, _positionSmash).ToPoint(),
                Vector2.Subtract(Body.Size.ToVector2(), _bodySmash.ToVector2()).ToPoint()),
                Body, SpriteColor * Transparent, Rotation, Origin, spriteEffect, 0);
            EndDraw(spriteBatch);
        }
        #endregion
    }
}
