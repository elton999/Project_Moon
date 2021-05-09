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
        public override void Start()
        {
            base.Start();
            this.Sprite = this.Scene.Content.Load<Texture2D>(this.Scene.TileMapPath);
            this.Body = new Rectangle(new Point(64,80), new Point(8,8));
            this.tag = "bullet";
            
            this.size = new Point(8,8);
            this.Scene.AllActors.Add(this);
            this.Scene.Middleground.Add(this);
            this.gravity2D = new Vector2(0,0);
            this.velocityDecrecentX = 0;
            this.velocityDecrecentY = 0;


            this.Scene.Camera.TimeShake = 10;
            this.Scene.Camera.ShakeMagnitude = 3.5f;
        }

        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            foreach(Actor actor in this.Scene.AllActors)
                if(actor != this)
                    if (this.overlapCheck(actor))
                        this.OnCollision(actor.tag);

            if (this.EdgesIsCollision.ContainsValue(true))
                this.OnCollision("wall");
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
            this.DrawSprite(spriteBatch);
        }
    }
}
