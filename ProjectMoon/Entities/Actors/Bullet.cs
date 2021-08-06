﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit;
using UmbrellaToolKit.Collision;

namespace ProjectMoon.Entities.Actors
{
    class Bullet : Actor
    {
        static readonly Random getRandom = new Random();

        public bool FromEnemy = false;
        public override void Start()
        {
            base.Start();
            this.Sprite = this.Scene.Content.Load<Texture2D>(this.Scene.TileMapPath);
            this.Body = new Rectangle(new Point(64, 80), new Point(8, 8));
            this.tag = "bullet";

            this.size = new Point(8, 8);
            this.Scene.AllActors.Add(this);
            this.Scene.Middleground.Add(this);
            this.gravity2D = new Vector2(0, 0);

            if (!FromEnemy)
            {
                this.Scene.Camera.TimeShake = 10;
                this.Scene.Camera.ShakeMagnitude = 3.5f;

                int randomY = getRandom.Next(-3, 3);
                this.Position = new Vector2(this.Position.X, this.Position.Y + randomY);
            }
        }

        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            foreach (Actor actor in this.Scene.AllActors)
            {
                if (actor != this && actor.active && this.overlapCheck(actor))
                {
                    if (actor.tag == "player" && this.FromEnemy || actor.tag != "player" && !this.FromEnemy)
                    {
                        this.OnCollision(actor.tag);
                        actor.OnCollision(this.tag);
                    }
                }
            }

            if (this.EdgesIsCollision.ContainsValue(true) || this.CheckSolidOverlap())
                this.OnCollision("wall");
        }

        private bool CheckSolidOverlap()
        {
            var checkPosition = this.Position;
            checkPosition.X += MathF.Sign(this.velocity.X);

            foreach (Solid solid in this.Scene.AllSolids)
                if (solid.check(this.size, checkPosition))
                    return true;

            var actor = new Actor();
            actor.Position = checkPosition;
            actor.size = this.size;
            if (this.Scene.Grid.checkOverlapActor(actor, true))
                return true;

            return false;
        }

        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);
            this.RemoveFromScene = true;
        }

        public override void IsNotvisible()
        {
            base.IsNotvisible();
            this.Destroy();
        }

        public override void Destroy()
        {
            base.Destroy();
            this.Scene.AllActors.Remove(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
