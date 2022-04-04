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
    public class Player : Actor
    {
        private string[] _enemyTags = new string[6] {
             "soldier",
             "spider",
             "bat",
             "jumper",
             "damage",
             "bullet"
        };

        public Weapon Weapon;
        public States.PlayerState CurrentState;

        public float JumpForce = 0.012f;
        public float DashJumpForce = 0.095f;
        public float SpeedIncrement = -0.05f;

        public bool IsFlying = false;

        public PlayerBasicBehavior Behavior;

        public CoroutineManagement CoroutineManagement = new();

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
            if(CurrentState != null)
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
                this.TakeDamage();
        }

        public override bool isRiding(Solid solid)
        {
            if (solid.check(this.size, new Vector2(this.Position.X, this.Position.Y + 1)))
                return true;
            return false;
        }

        public bool IsGrounded = false;
        private void CheckGrounded()
        {
            this.IsGrounded = false;
            foreach (Solid solid in this.Scene.AllSolids)
                if (solid.check(this.size, new Vector2(this.Position.X, this.Position.Y + 1)))
                    this.IsGrounded = true;

            if (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y + 1), this))
                this.IsGrounded = true;
        }
        #endregion

        #region damage
        public bool IsTakingDamage = false;
        public void TakeDamage()
        {
            if (IsTakingDamage)
                return;

            IsTakingDamage = true;
            Scene.GameManagement.Values["CURRENT_LIFES"]--;

            CoroutineManagement.ClearCoroutines();
            CoroutineManagement.StarCoroutine(DamageFX(CoroutineManagement.GameTime));
        }

        public IEnumerator DamageFX(GameTime gameTime)
        {
            for(int i = 0; i < 60 * 4; i++)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds % 8 > 4)
                {
                    SpriteColor = Color.Red;
                    Transparent = 0.7f;
                }
                else
                {
                    SpriteColor = Color.White;
                    Transparent = 1;
                }
                yield return null;
            }

            IsTakingDamage = false;
            SpriteColor = Color.White;
            Transparent = 1f;

            yield return null;
        }
        #endregion

        #region Animation and Render
        public AsepriteAnimation AsepriteAnimation;
        #region smashEFX
        private Vector2 _PositionSmash;
        private Point _BobySmash;
        
        public IEnumerator Squash()
        {
            _BobySmash.X = -15;
            _BobySmash.Y = 5;
            _PositionSmash.X = 2;
            _PositionSmash.Y = -2;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public IEnumerable Stretch()
        {
            _BobySmash.X = 10;
            _BobySmash.Y = -5;
            _PositionSmash.X = 2;
            _PositionSmash.Y = 3;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public void ResetSpriteSizes()
        {
            _BobySmash = new Point(0, 0);
            _PositionSmash = new Vector2(0, 0);
        }

        #endregion

        public void Flip(bool right) => spriteEffect = right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Body = AsepriteAnimation.Body;
            BeginDraw(spriteBatch);
            spriteBatch.Draw(
                Sprite,
                new Rectangle(Vector2.Subtract(Position, _PositionSmash).ToPoint(),
                Vector2.Subtract(Body.Size.ToVector2(), _BobySmash.ToVector2()).ToPoint()),
                Body, SpriteColor * Transparent, Rotation, Origin, spriteEffect, 0);
            EndDraw(spriteBatch);
        }
        #endregion
    }
}
