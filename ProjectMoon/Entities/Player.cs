using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaToolKit.Sprite;
using UmbrellaToolKit.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectMoon.Entities
{
    class Player : Actor
    {
        public override void Start()
        {
            base.Start();
            this.Scene.AllActors.Add(this);
            this.size = new Point(10, 32);
            this.tag = "player";

            this.gravity2D = new Vector2(0, this.GravityY);
            this.velocityDecrecentY = 2050;
            this.velocityDecrecentX = 0;

            this.Sprite = Content.Load<Texture2D>("Sprites/Player/Regina");
            this.AsepriteAnimation = new AsepriteAnimation(Content.Load<AsepriteDefinitions>("Sprites/Player/ReginaAnimations"));
            this.Origin = new Vector2(27, 16);

            wait(this.RechargeFuelTime, RechargeFuel);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Animation(gameTime);

            this.DamageFX(gameTime);

            this.Input();

            this.Scene.Camera.Target = new Vector2(this.Position.X + this.size.X / 2, this.Position.Y + this.size.Y / 2);

        }
        
        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);

            this.jump();

            this.Fly(gameTime);

            this.move(gameTime);

            this.Fire(gameTime);

            this.CheckGrounded();
        }

        #region input
        bool CRight = false;
        bool CLeft = false;
        bool CUp = false;
        bool CDown = false;

        bool CJump = false;
        bool CFly = false;
        bool CFire = false;

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
                CUp = false;

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
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                CFly = true;
            if (Keyboard.GetState().IsKeyUp(Keys.X))
                CFly = false;

            // fire
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                CFire = true;
            if (Keyboard.GetState().IsKeyUp(Keys.C))
                CFire = false;
        }
        #endregion

        #region physics
        #region move
        private float GravityY = -200f;

        private float HorizontalSpeed = 0;
        private float _TotalSpeedOnGrounded = 75;
        private float _SpeedIncrement = 5;

        private float _TotalSpeedFly = 70;
        private float VertivalSpeed = 0;
        private void move(GameTime gameTime)
        {
            if (!this._isFire)
            {
                // ground control
                if (this._isGrounded)
                {
                    if (this.CRight)
                    {
                        if (this.velocity.X > 0)
                            this.HorizontalSpeed = 0;
                        this.velocity.X = (-this.HorizontalSpeed);
                        this.Flip(true);
                        this.speedIncrement_X();
                    }
                    else if (this.CLeft)
                    {
                        if (this.velocity.X < 0)
                            this.HorizontalSpeed = 0;
                        this.velocity.X = (this.HorizontalSpeed);
                        this.Flip(false);
                        this.speedIncrement_X();
                    }
                }

                // flying control
                if (this._isFly)
                {
                    if (this.CRight)
                    {
                        if (this.velocity.X > 0)
                            this.HorizontalSpeed = 0;
                        this.velocity.X = -(this.HorizontalSpeed);
                        this.Flip(true);
                        this.speedIncrement_X();
                    }
                    else if (this.CLeft)
                    {
                        if (this.velocity.X < 0)
                            this.HorizontalSpeed = 0;
                        this.velocity.X = (this.HorizontalSpeed);
                        this.Flip(false);
                        this.speedIncrement_X();
                    }

                    if (this.CUp)
                    {
                        this.velocity.Y = this._TotalSpeedFly;
                        this.speedIncrement_Y();
                    }
                    else if (this.CDown)
                    {
                        this.velocity.Y = -(this._TotalSpeedFly);
                        this.speedIncrement_Y();
                    }
                }
            }


            if (((!this.CLeft && !this.CRight) && this._isGrounded) || 
                ((!this.CLeft && !this.CRight) && this._isFly) || this._isFire)
            {
                this.velocity.X = 0;
                this.HorizontalSpeed = 0;
            }

            if (!this.CDown && !this.CUp && this._isFly || this._isFire)
            {
                this.VertivalSpeed = 0;
                this.velocity.Y = 0;
            }
        }


        /// <summary>
        /// Speed Increment Horizontal
        /// </summary>
        private void speedIncrement_X()
        {
            if (this.HorizontalSpeed < this._TotalSpeedOnGrounded)
                this.HorizontalSpeed += this._SpeedIncrement;

            if (this.HorizontalSpeed >  this._TotalSpeedOnGrounded)
                this.HorizontalSpeed = this._TotalSpeedOnGrounded;
        }

        /// <summary>
        /// Speed Increment Vertival
        /// </summary>
        private void speedIncrement_Y()
        {
            if (this.VertivalSpeed <this._TotalSpeedFly)
                this.VertivalSpeed += this._SpeedIncrement;

            if (this.VertivalSpeed > this._TotalSpeedFly)
                this.VertivalSpeed = this._TotalSpeedFly;
        }
        #endregion

        #region Jump
        private int _JumpPressedForce = 0;
	    private bool _JumpPressed = false;
        private float _JumpForce = 330f; //390f;

	    private void jump(){
		    if(this._isGrounded && this.CJump){
                this._JumpPressed = true;
            }

		    if(this._JumpPressed && this._JumpPressedForce< 17){
                this.velocity = new Vector2(this.velocity.X, this._JumpForce);
                this._JumpPressedForce += 1;
            } else
                this._JumpPressed = false;

		    if(this._isGrounded && !this._JumpPressed){
                this._JumpPressed = false;
                this._JumpPressedForce = 0;
            }
        }
        #endregion

        #region Fly
        private float Power { 
            get => this.Scene.GameManagement.Values["POWER"]; 
            set => this.Scene.GameManagement.Values["POWER"] = value; 
        }
        private float _FuelDecrement = 20f;
        private bool _isFly = false;
        private bool _CFlyPressed = false;
        private bool _onFly = false;
        private void Fly(GameTime gametime)
        {
            if (this.CFly && !this._CFlyPressed && !this._onFly)
            {
                this._onFly = true;
                moveY(1, null);
            } else if(!this.CFly && !this._CFlyPressed && this._onFly)
            {
                this._CFlyPressed = true;
            } else if(this.CFly && this._CFlyPressed && this._onFly)
            {
                this._onFly = false;
            } else if (!this.CFly && this._CFlyPressed && !this._onFly)
            {
                this._CFlyPressed = false;
            }

            if (this._onFly && this.Power > 0)
            {
                this.Power -= this._FuelDecrement * (float)gametime.ElapsedGameTime.TotalSeconds;
                this.velocityDecrecentY = 0;
                this.gravity2D = new Vector2(0, 60);
                this._isFly = true;
            }
            else
            {
                this.velocityDecrecentY = 2050;
                this.gravity2D = new Vector2(0, this.GravityY);
                this._isFly = false;
            }
        }

        private float RechargeFuelTime = 1f;
        private void RechargeFuel()
        {
            if (this.Scene.GameManagement.Values["POWER"] < 100)
                this.Scene.GameManagement.Values["POWER"] = this.Scene.GameManagement.Values["POWER"] + 1;

            wait(this.RechargeFuelTime, RechargeFuel);
        }

        #endregion

        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);

            if (tag == "enemy")
                this.TakeDamage();
        }

        private bool _StartDamage = false;
        private bool _DamageFX = false;
        public void TakeDamage()
        {
            if (!this._StartDamage)
            {
                this._StartDamage = true;
                this._DamageFX = true;
                this.Scene.GameManagement.Values["CURRENT_LIFES"] = this.Scene.GameManagement.Values["CURRENT_LIFES"] - 1;

                wait(5, new Action(() => {
                    this._StartDamage = false;
                    this._DamageFX = false;
                    this.SpriteColor = Color.White;
                }));
            }
        }

        private void DamageFX(GameTime gameTime)
        {
            if (this._DamageFX)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds % 8 > 4)
                    this.SpriteColor = Color.Red;
                else
                    this.SpriteColor = Color.White;
            }
        }

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

        #region shooting
        private float BulletVelocity = 150f;
        private bool _isFire = false;
        private int _swingBullet = 2;
        
        // Cadence
        private float Cadence = 0.5f;
        private bool FirePressed = false;
        private float timerPressed = 0;

        public void Fire(GameTime gameTime)
        {
            if (this.CFire)
            {
                this._isFire = true;
                timerPressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timerPressed >= Cadence)
                {
                    timerPressed = 0;
                    this.Shoot();
                }
                else if (timerPressed == 0)
                    this.Shoot();
            }
            else
            {
                this._isFire = false;
                timerPressed = 0;
            }
        }

        public void Shoot()
        {
            Actors.Bullet _bullet = new Actors.Bullet();
            _bullet.Scene = this.Scene;
            _bullet.spriteEffect = this.spriteEffect;
            _bullet.velocity = new Vector2(this.spriteEffect == SpriteEffects.None ? -this.BulletVelocity : this.BulletVelocity, 0);

            if (this.spriteEffect == SpriteEffects.None)
                _bullet.Position = new Vector2(this.Position.X + 20, this.Position.Y + 10 + (new Random()).Next(-this._swingBullet, this._swingBullet));
            else
                _bullet.Position = new Vector2(this.Position.X - 20, this.Position.Y + 10 + (new Random()).Next(-this._swingBullet, this._swingBullet));

            _bullet.Start();

            this.velocity = new Vector2(this.velocity.X + (-_bullet.velocity.X / 2.5f), this.velocity.Y);
        }
        #endregion


        #region Animation and Render
        private void Animation(GameTime gameTime)
        {
            if (!this._isGrounded && !this._isFly)
                this.AsepriteAnimation.Play(gameTime, "jump", AsepriteAnimation.AnimationDirection.LOOP);
            else if (this._isFire)
            {
                if (this.AsepriteAnimation.lastFrame)
                    this._isFire = false;
                else
                    this.AsepriteAnimation.Play(gameTime, "jump", AsepriteAnimation.AnimationDirection.FORWARD);
            }
            else if (this._isFly)
                this.AsepriteAnimation.Play(gameTime, "jet-pack", AsepriteAnimation.AnimationDirection.LOOP);
            else if (CRight || CLeft)
                this.AsepriteAnimation.Play(gameTime, "walk", AsepriteAnimation.AnimationDirection.LOOP);
            else
                this.AsepriteAnimation.Play(gameTime, "idle", AsepriteAnimation.AnimationDirection.LOOP);

            this.Body = this.AsepriteAnimation.Body;
        }


        private void Flip(bool right)
        {
            if (right)
                this.spriteEffect = SpriteEffects.None;
            else
                this.spriteEffect = SpriteEffects.FlipHorizontally;
        }

        public AsepriteAnimation AsepriteAnimation;

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch);
        }
        #endregion
    }
}
