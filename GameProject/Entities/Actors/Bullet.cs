using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Actors
{
    class Bullet : Actor
    {
        public bool FromEnemy = false;
        public override void Start()
        {
            base.Start();
            Sprite = Scene.Content.Load<Texture2D>(Scene.TileMapPath);
            Body = new Rectangle(new Point(64, 80), new Point(8, 8));
            tag = "bullet";

            size = new Point(8, 8);
            Scene.AllActors.Add(this);
            Scene.Middleground.Add(this);
            gravity2D = new Vector2(0, 0);

            if (!FromEnemy)
            {
                Scene.Camera.TimeShake = 10;
                Scene.Camera.ShakeMagnitude = 3.5f;

                int randomY = getRandom.Next(-2, 2);
                Position += Vector2.UnitY * randomY;
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            foreach (Actor actor in Scene.AllActors)
            {
                if (actor != this && actor.active && overlapCheck(actor))
                {
                    if (actor.tag == "player" && FromEnemy || actor.tag != "player" && !FromEnemy)
                    {
                        OnCollision(actor.tag);
                        actor.OnCollision(this.tag);
                    }
                }
            }

            if (EdgesIsCollision.ContainsValue(true) || CheckSolidOverlap())
                OnCollision("wall");
        }

        public static Bullet CreateBullet(Actor actor, Vector2 bulletVelocity)
        {
            var bullet = new Bullet();
            bullet.Scene = actor.Scene;
            bullet.spriteEffect = actor.spriteEffect;
            bullet.velocity = bulletVelocity;
            return bullet;
        }

        public bool CheckSolidOverlap()
        {
            var checkPosition = Position;
            checkPosition += new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y));

            foreach (Solid solid in this.Scene.AllSolids)
                if (solid.check(size, checkPosition))
                    return true;

            var actor = new Actor();
            actor.Position = checkPosition;
            actor.size = size;

            if (Scene.Grid.checkOverlapActor(actor, true))
                return true;

            return false;
        }

        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);
            RemoveFromScene = true;
        }

        public override void IsNotvisible()
        {
            base.IsNotvisible();
            Destroy();
        }

        public override void Destroy()
        {
            base.Destroy();
            Scene.AllActors.Remove(this);
        }
    }
}
