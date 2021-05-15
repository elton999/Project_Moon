using System;
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
    public class Enemy : Actor
    {

        public UmbrellaToolKit.Sprite.Square Box;
        public bool isLive { get => live > 0; }
        public float live = 10;
        public override void Start()
        {
            base.Start();

            this.tag = "enemy";
        }

        public override void restart()
        {
            base.restart();
        }

        public float GravityY = -200f;
        public float _Speed = 60;
    }
}
