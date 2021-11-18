using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Actors
{
    public abstract class Enemy : Actor
    {

        public UmbrellaToolsKit.Sprite.Square Box;
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

        public override void OnCollision(string tag = null)
        {
            base.OnCollision(tag);
            if (tag == "bullet")
            {
                this.live -= 10;
                this.active = false;
            }
        }

        public float GravityY = 200f;
        public float _Speed = 90;
    }
}
