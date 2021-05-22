using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;
using UmbrellaToolKit.Collision;

namespace ProjectMoon.Entities.Actors.Enemies
{
    public class Shooter : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "shoot";

            this.Scene.AllActors.Add(this);
            this.size = new Point(12, 12);

            this.InitialPosition = this.Position;

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Blue;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            //base.UpdateData(gameTime);
            if (this.isLive)
            {
                Actor _player = this.Scene.AllActors[0];
                if (this.overlapCheckPixel(this.Scene.AllActors[0]))
                    _player.OnCollision(this.tag);

                if (_isVisible && !_isWaitingToShoot)
                {
                    _isWaitingToShoot = true;
                    this.Shoot();
                    wait(_timeToNextShoot, () => { _isWaitingToShoot = false; });
                }
            }
        }

        private float _timeToNextShoot = 2f;
        private bool _isWaitingToShoot = false;
        private void Shoot()
        {
            Bullet _bullet = new Bullet();
            _bullet.FromEnemy = true;
            _bullet.Scene = this.Scene;
            _bullet.Start();
            _bullet.Position = new Vector2(this.Position.X - 10, this.Position.Y + 5);
            _bullet.velocity = new Vector2(150, 0);
            _bullet.Body = new Rectangle(new Point(72, 80), new Point(8, 8));
        }


        bool _isVisible = false;
        public override void Isvisible()
        {
            _isVisible = true;
        }

        public override void IsNotvisible()
        {
            _isVisible = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isLive)
            {
                base.Draw(spriteBatch);
                if (this.Scene.GameManagement.Values["DEBUG"])
                {
                    this.Box.Position = this.Position;
                    this.Box.Draw(spriteBatch);
                }
            }
        }
    }
}
