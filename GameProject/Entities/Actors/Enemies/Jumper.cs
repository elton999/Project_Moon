using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Actors.Enemies
{
    public class Jumper : Enemy
    {
        public override void Start()
        {
            base.Start();
            this.tag = "jumper";

            this.Scene.AllActors.Add(this);
            this._Speed = 90;

            this.size = new Point(16, 16);
            this.InitialPosition = this.Position;

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                this.Box = new UmbrellaToolsKit.Sprite.Square();
                this.Box.Position = this.Position;
                this.Box.size = this.size;
                this.Box.SquareColor = Color.Purple;
                this.Box.Scene = this.Scene;

                this.Box.Start();
            }
        }

        private bool _StopWall = false;
        public override void UpdateData(GameTime gameTime)
        {
            if (this.isLive)
            {
                if (!_StopWall)
                {
                    this.moveX(this._Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                    Actor _jumperActorCheckLeft = new Actor();
                    _jumperActorCheckLeft.Position = new Vector2(this.Position.X - 1, this.Position.Y);
                    _jumperActorCheckLeft.size = this.size;

                    Actor _jumperActorCheckRight = new Actor();
                    _jumperActorCheckRight.Position = new Vector2(this.Position.X + 1, this.Position.Y);
                    _jumperActorCheckRight.size = this.size;

                    if (this.Scene.Grid.checkOverlapActor(_jumperActorCheckLeft) || this.Scene.Grid.checkOverlapActor(_jumperActorCheckRight))
                    {
                        this._Speed = -this._Speed;
                        this._StopWall = true;
                        wait(0.5f, () => { this._StopWall = false; });
                    }
                }

                Actor _player = this.Scene.AllActors[0];
                if (_player.overlapCheckPixel(this))
                    _player.OnCollision(this.tag);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isLive)
            {
                base.Draw(spriteBatch);
                if (this.Scene.GameManagement.Values["DEBUG"])
                {
                    this.Box.Scene = this.Scene;
                    this.Box.Position = this.Position;
                    this.Box.Draw(spriteBatch);
                }
            }

        }
    }
}
