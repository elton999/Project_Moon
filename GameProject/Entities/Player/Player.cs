using System;
using System.Linq;
using UmbrellaToolsKit.Sprite;
using UmbrellaToolsKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Gameplay;

namespace GameProject.Entities.Player
{
    public class Player : Actor
    {
        public States.PlayerState CurrentState;
        public Jetpack Jetpack;
        public Weapon Weapon;

        public float JumpForce = 0.012f;
        public float DashJumpForce = 0.095f;
        public float GravityY = 0.0008f;
        public float SpeedIncrement = -0.05f;

        public override void Start()
        {
            base.Start();
            Scene.AllActors.Add(this);
            size = new Point(10, 32);
            tag = "player";

            Jetpack = new Jetpack(this);
            Weapon = new Weapon(this);

            Sprite = Content.Load<Texture2D>("Sprites/Player/Regina");
            AsepriteAnimation = new AsepriteAnimation(Content.Load<AsepriteDefinitions>("Sprites/Player/ReginaAnimations"));
            Origin = new Vector2(27, 16);

            SwitchState(new States.PlayerStateIdle());

            velocityDecrecentY = 2050;
            gravity2D = new Vector2(0, GravityY);
        }

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
            CurrentState.LogicUpdate(gameTime);
            CurrentState.InputUpdate();

            //DamageFX(gameTime);
        }

        public override void UpdateData(GameTime gameTime)
        {
            CheckGrounded();
            
            CurrentState.PhysicsUpdate(gameTime);

            Scene.Camera.Target = Position + size.ToVector2() / 2f;
            base.UpdateData(gameTime);
        }

        #region physics

        #endregion


        private string[] _enemyTags = new string[6] {
             "soldier",
             "spider",
             "bat",
             "jumper",
             "damage",
             "bullet"
        };
        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);

            if (_enemyTags.Contains(tag))
                this.TakeDamage();
        }

        #region damage
        private bool _StartDamage = false;
        private bool _DamageFX = false;
        public void TakeDamage()
        {
            if (!this._StartDamage)
            {
                this._StartDamage = true;
                this._DamageFX = true;
                this.Scene.GameManagement.Values["CURRENT_LIFES"]--;

                wait(5, new Action(() =>
                {
                    this._StartDamage = false;
                    this._DamageFX = false;
                    this.SpriteColor = Color.White;
                    this.Transparent = 1f;
                }));
            }
        }

        private void DamageFX(GameTime gameTime)
        {
            if (this._DamageFX)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds % 8 > 4)
                {
                    this.SpriteColor = Color.Red;
                    this.Transparent = 0.7f;
                }
                else
                {
                    this.SpriteColor = Color.White;
                    this.Transparent = 1;
                }
            }
        }
        #endregion

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

        #region Animation and Render
        public AsepriteAnimation AsepriteAnimation;
        #region smashEFX
        private Vector2 _PositionSmash;
        private Point _BobySmash;
        private bool _GroundHit = false;
        private bool _ShootSmashEFX = false;
        private bool _last_isgrounded = false;
        public void SmashEfx(bool _shooting = false)
        {
            if (!_last_isgrounded && this.IsGrounded && !_GroundHit && !_shooting)
            {
                _BobySmash.X = -15;
                _BobySmash.Y = 5;
                _PositionSmash.X = 2;
                _PositionSmash.Y = -2;
                _GroundHit = true;
                wait(0.2f, () =>
                {
                    _BobySmash = new Point(0, 0);
                    _PositionSmash = new Vector2(0, 0);
                    _GroundHit = false;
                });
            }

            if (_shooting && !_ShootSmashEFX)
            {
                _ShootSmashEFX = true;

                _BobySmash.X = 10;
                _BobySmash.Y = -5;
                _PositionSmash.X = 2;
                _PositionSmash.Y = 3;
                _GroundHit = true;
                wait(0.2f, () =>
                {
                    _BobySmash = new Point(0, 0);
                    _PositionSmash = new Vector2(0, 0);
                    _ShootSmashEFX = false;
                    _GroundHit = false;
                });
            }
            else if (!_GroundHit)
            {
                _BobySmash = new Point(0, 0);
                _PositionSmash = new Vector2(0, 0);
            }

            this._last_isgrounded = this.IsGrounded;
        }
        #endregion

        public void Flip(bool right)
        {
            spriteEffect = right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Body = AsepriteAnimation.Body;
            SmashEfx();
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
