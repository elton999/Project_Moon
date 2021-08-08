using System;
using System.Linq;
using UmbrellaToolKit.Sprite;
using UmbrellaToolKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectMoon.Gameplay;

namespace ProjectMoon.Entities.Player
{
    public class Player : Actor
    {
        public Jetpack Jetpack;
        public Weapon Weapon;
        public override void Start()
        {
            base.Start();
            this.Scene.AllActors.Add(this);
            this.size = new Point(10, 32);
            this.gravity2D.Y = 0f;
            this.tag = "player";

            this.Jetpack = new Jetpack(this);
            this.Weapon = new Weapon(this);

            this.Sprite = Content.Load<Texture2D>("Sprites/Player/Regina");
            this.AsepriteAnimation = new AsepriteAnimation(Content.Load<AsepriteDefinitions>("Sprites/Player/ReginaAnimations"));
            this.Origin = new Vector2(27, 16);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Animation(gameTime);

            this.Input();

            this.DamageFX(gameTime);
        }

        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);

            this.move(gameTime);

            this.jump();

            this.Jetpack.Update(gameTime);

            this.Weapon.Update(gameTime, this.CFire);

            this.CheckGrounded();

            this.Scene.Camera.Target = new Vector2(this.Position.X + this.size.X / 2, this.Position.Y + this.size.Y / 2);
        }

        #region input
        bool CRight = false;
        bool CLeft = false;
        bool CUp = false;
        bool CDown = false;

        bool CJump = false;
        public bool CFly = false;
        public bool CFire = false;

        private void Input()
        {
            // Arrows moviments
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                CRight = true;
            if (Keyboard.GetState().IsKeyUp(Keys.Right))
                CRight = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                CLeft = true;
            if (Keyboard.GetState().IsKeyUp(Keys.Left))
                CLeft = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                CUp = true;
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                CUp = false;
                this.Weapon.UpShoot = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                CDown = true;
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                CDown = false;

            // jump
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                CJump = true;
            if (Keyboard.GetState().IsKeyUp(Keys.Z))
                CJump = false;

            // fly
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                CFly = true;
            if (Keyboard.GetState().IsKeyUp(Keys.C))
                CFly = false;

            // fire
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                CFire = true;
            if (Keyboard.GetState().IsKeyUp(Keys.X))
                CFire = false;
        }
        #endregion

        #region physics
        #region move
        public float GravityY = 0.0008f;

        private float _SpeedIncrement = -0.05f;

        private void move(GameTime gameTime)
        {
            if (!this.Weapon.IsFire && !this.Weapon.UpShoot)
            {
                // ground control
                if (this._isGrounded && !this.CFire)
                {
                    if (this.CRight)
                    {
                        this.velocity.X = (-this._SpeedIncrement);
                        this.Flip(true);
                    }
                    else if (this.CLeft)
                    {
                        this.velocity.X = (this._SpeedIncrement);
                        this.Flip(false);
                    }
                }

                // flying control
                if (this.Jetpack.isFly)
                {
                    if (this.CRight)
                    {
                        this.velocity.X = -(this._SpeedIncrement);
                        this.Flip(true);
                    }
                    else if (this.CLeft)
                    {
                        this.velocity.X = (this._SpeedIncrement);
                        this.Flip(false);
                    }

                    if (this.CUp)
                        this.velocity.Y = this._SpeedIncrement;
                    else if (this.CDown)
                        this.velocity.Y = -(this._SpeedIncrement);
                }
            }


            if ((IsNotPressLeftAndRight && this._isGrounded) ||
                (IsNotPressLeftAndRight && this.Jetpack.isFly) || (this.CFire || this.Weapon.UpShoot))
                this.velocity.X = 0;

            if (!this.CDown && !this.CUp && this.Jetpack.isFly)
                this.velocity.Y = 0;
        }

        private bool IsNotPressLeftAndRight { get => !this.CLeft && !this.CRight; }
        #endregion

        #region Jump
        private int _JumpPressedForce = 0;
        private bool _JumpPressed = false;
        private float _JumpForce = 0.012f;
        private float _DashJumpForce = 0.095f;

        private void jump()
        {
            if (this._isGrounded && this.CJump)
            {
                this._JumpPressed = true;

                if (this.velocity.X > 0)
                    this.velocity.X = _DashJumpForce;
                else if (this.velocity.X < 0)
                    this.velocity.X = -_DashJumpForce;
            }

            if (this._JumpPressed && this._JumpPressedForce < 1)
            {
                float g = (2f * this._JumpForce) / (MathF.Pow(0.08f, 2f));
                float initJumpVelocity = MathF.Sqrt(2.0f * g * this._JumpForce);
                this.velocity.Y = -initJumpVelocity;

                this._JumpPressedForce += 1;
            }
            else
                this._JumpPressed = false;

            if (this._isGrounded && !this._JumpPressed && !this.CJump)
            {
                this._JumpPressed = false;
                this._JumpPressedForce = 0;
            }
        }
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

        private bool _isGrounded = false;
        private void CheckGrounded()
        {
            this._isGrounded = false;
            foreach (Solid solid in this.Scene.AllSolids)
                if (solid.check(this.size, new Vector2(this.Position.X, this.Position.Y + 1)))
                    this._isGrounded = true;

            if (this.Scene.Grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y + 1), this))
                this._isGrounded = true;

        }
        #endregion

        #region Animation and Render
        public AsepriteAnimation AsepriteAnimation;
        private void Animation(GameTime gameTime)
        {
            if (!this._isGrounded && !this.Jetpack.isFly)
                this.AsepriteAnimation.Play(gameTime, "jump-jetpack", AsepriteAnimation.AnimationDirection.LOOP);
            else if (this._isGrounded && this.CUp)
            {
                this.Weapon.UpShoot = true;
                this.AsepriteAnimation.Play(gameTime, "shoot-jetpack", AsepriteAnimation.AnimationDirection.FORWARD);
            }
            else if (this.Weapon.IsFire)
            {
                if (this.AsepriteAnimation.lastFrame)
                    this.Weapon.IsFire = false;
                else
                {
                    if (!this.Weapon.UpShoot)
                        this.AsepriteAnimation.Play(gameTime, "jump-jetpack", AsepriteAnimation.AnimationDirection.FORWARD);
                }
            }
            else if (this.Jetpack.isFly)
                this.AsepriteAnimation.Play(gameTime, "fly", AsepriteAnimation.AnimationDirection.LOOP);
            else if (CRight || CLeft)
                this.AsepriteAnimation.Play(gameTime, "walk-jetpack", AsepriteAnimation.AnimationDirection.LOOP);
            else
                this.AsepriteAnimation.Play(gameTime, "idle-jetpack", AsepriteAnimation.AnimationDirection.LOOP);

            this.Body = this.AsepriteAnimation.Body;
        }

        #region smashEFX
        private Vector2 _PositionSmash;
        private Point _BobySmash;
        private bool _GroundHit = false;
        private bool _ShootSmashEFX = false;
        private bool _last_isgrounded = false;
        public void SmashEfx(bool _shooting = false)
        {
            if (!_last_isgrounded && this._isGrounded && !_GroundHit && !_shooting)
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

            this._last_isgrounded = this._isGrounded;
        }
        #endregion

        private void Flip(bool right)
        {
            this.spriteEffect = right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.SmashEfx();
            BeginDraw(spriteBatch);
            spriteBatch.Draw(
                this.Sprite,
                new Rectangle(Vector2.Subtract(this.Position, _PositionSmash).ToPoint(),
                Vector2.Subtract(this.Body.Size.ToVector2(), _BobySmash.ToVector2()).ToPoint()),
                this.Body, this.SpriteColor * this.Transparent, this.Rotation, this.Origin, this.spriteEffect, 0);
            EndDraw(spriteBatch);
        }
        #endregion
    }
}
