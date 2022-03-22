using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Actors.Enemies
{
    public class Soldier : Enemy
    {
        private bool _StartAttack = false;
        private bool _waitAttack = false;
        private bool _Attack = false;
        private float _TimeToAttack = 1;

        public override void Start()
        {
            base.Start();
            tag = "soldier";

            Scene.AllActors.Add(this);
            size = new Point(10, 32);

            if (this.Scene.GameManagement.Values["DEBUG"])
            {
                Box = new UmbrellaToolsKit.Sprite.Square();
                Box.Position = Position;
                Box.size = size;
                Box.SquareColor = Color.Magenta;
                Box.Scene = Scene;

                Box.Start();
            }

            gravity2D = new Vector2(0, GravityY);
            velocityDecrecentY = 2050;
            velocityDecrecentX = 0;
            _Speed = 200;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isLive) return;
            
            base.Update(gameTime);

            if (_StartAttack && !_waitAttack)
            {
                _waitAttack = true;
                _Speed = Scene.AllActors[0].Position.X < Position.X ? -_Speed : _Speed;
                wait(_TimeToAttack, () => { _Attack = true; });
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            if (!isLive) return;

            if (_Attack)
            {
                velocity.X = _Speed;
                if (CheckPath())
                    _Speed = -_Speed;
            }

            if (overlapCheckPixel(Scene.AllActors[0]))
                Scene.AllActors[0].OnCollision(tag);

            base.UpdateData(gameTime);
        }

        public bool CheckPath()
        {
            Actor _actor = new Actor();
            _actor.Position = Position;
            _actor.size = size;

            if (!Scene.Grid.checkOverlap(_actor.size, _actor.Position + new Vector2(_actor.size.X * Math.Sign(_Speed), 16), _actor))
                return true;

            if (Scene.Grid.checkOverlap(_actor.size, _actor.Position + Vector2.UnitX * Math.Sign(_Speed), _actor, false))
                return true;

            return false;
        }


        public override void Isvisible()
        {
            if (!isLive) return;
            _StartAttack = true;
            base.Isvisible();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isLive) return;

            base.Draw(spriteBatch);
            
            if (!Scene.GameManagement.Values["DEBUG"]) return;
            Box.Scene = this.Scene;
            Box.Position = this.Position;
            Box.Draw(spriteBatch);
        }
    }
}
