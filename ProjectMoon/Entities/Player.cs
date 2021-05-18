﻿using System;
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
        }
        
        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);

            this.move(gameTime);

            this.jump();

            this.Fly(gameTime);

            this.Fire(gameTime);

            this.CheckGrounded();

            this.Scene.Camera.Target = new Vector2(this.Position.X + this.size.X / 2, this.Position.Y + this.size.Y / 2);
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

        private float _TotalSpeedFly = 100;
        private float VertivalSpeed = 0;
        private void move(GameTime gameTime)
        {
            if (!this._isFire)
            {
                // ground control
                if (this._isGrounded && !this.CFire)
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
                        this.velocity.Y = -this.VertivalSpeed;
                        this.speedIncrement_Y();
                    }
                    else if (this.CDown)
                    {
                        this.velocity.Y = (this.VertivalSpeed);
                        this.speedIncrement_Y();
                    }
                }
            }


            if (((!this.CLeft && !this.CRight) && this._isGrounded) || 
                ((!this.CLeft && !this.CRight) && this._isFly) || this.CFire)
            {
                this.velocity.X = 0;
                this.HorizontalSpeed = 0;
            }

            if (!this.CDown && !this.CUp && this._isFly || this.CFire)
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

		    if(this._JumpPressed && this._JumpPressedForce< 20){
                this.velocity = new Vector2(this.velocity.X, this._JumpForce);
                this._JumpPressedForce += 1;
            } else
                this._JumpPressed = false;

		    if(this._isGrounded && !this._JumpPressed && !this.CJump){
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
            } else if(this.CFly && this._CFlyPressed && this._onFly || this.Scene.GameManagement.Values["POWER"] <= 0)
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
                this.moveY((-15 +this.velocity.Y )*(float)gametime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                
                this.gravity2D = new Vector2(0, 0);
                this._isFly = true;
            }
            else
            {
                this.velocityDecrecentY = 2050;
                this.gravity2D = new Vector2(0, this.GravityY);
                this._isFly = false;
            }
        }

        private float RechargeFuelTime = 0.2f;
        private void RechargeFuel()
        {
            if (this.Scene.GameManagement.Values["POWER"] < 100)
                this.Scene.GameManagement.Values["POWER"] = this.Scene.GameManagement.Values["POWER"] + 1;

            wait(this.RechargeFuelTime, RechargeFuel);
        }

        #endregion

        private string[] _enemyTags= new string[4] { "soldier", "spider", "bat", "damage" };
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
                this.Scene.GameManagement.Values["CURRENT_LIFES"] = this.Scene.GameManagement.Values["CURRENT_LIFES"] - 1;

                wait(5, new Action(() => {
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
                else { 
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

        #region shooting
        private float BulletVelocity = 150f;
        private bool _isFire = false;
        private int _swingBullet = 2;
        
        // Cadence
        private float Cadence = 0.5f;
        private bool FirePressed = false;
        private float timerPressed = 0;
        private bool firstBullet = false;

        public void Fire(GameTime gameTime)
        {
            if (this.CFire)
            {
                this._isFire = true;
                timerPressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timerPressed >= Cadence || !firstBullet)
                {
                    firstBullet = true;
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
                firstBullet = false;
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
        public AsepriteAnimation AsepriteAnimation;
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


        #region smashEFX
        private Vector2 _PositionSmash;
        private Point _BobySmash;
        private bool _GroundHit = false;
        private bool _last_isgrounded = false;
        public void SmashEfx()
        {
            if (!_last_isgrounded && this._isGrounded && !_GroundHit)
            {
                _BobySmash.X = -15;
                _BobySmash.Y = 5;
                _PositionSmash.X = 2;
                _PositionSmash.Y = -2;
                _GroundHit = true;
                wait(0.2f, () => {
                    _BobySmash = new Point(0,0);
                    _PositionSmash = new Vector2(0,0);
                    _GroundHit = false;
                });
            }
            /*else if (this.isHeadHit && !isGrounded)
            {
                _widthSmash = -10;
                _heightSmash = 5;
                _positionXSmash = -7;
                _positionYSmash = 0;
            }*/
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
            if (right)
                this.spriteEffect = SpriteEffects.None;
            else
                this.spriteEffect = SpriteEffects.FlipHorizontally;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //this.DrawSprite(spriteBatch);
            this.SmashEfx();

            spriteBatch.Draw(
                this.Sprite, 
                new Rectangle((int)(this.Position.X - _PositionSmash.X), (int)(this.Position.Y - _PositionSmash.Y),
                this.Body.Width - _BobySmash.X, this.Body.Height - _BobySmash.Y), 
                this.Body, this.SpriteColor * this.Transparent, this.Rotation, this.Origin, this.spriteEffect, 0);
        }
        #endregion
    }
}
