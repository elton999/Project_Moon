using System.Collections;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities
{
    public class SpriteDeforme : Actor
    {
        protected Vector2 _PositionSmash;
        protected Point _BobySmash;

        public CoroutineManagement CoroutineManagement = new();
        
        public IEnumerator Squash()
        {
            _BobySmash.X = -15;
            _BobySmash.Y = 5;
            _PositionSmash.X = 2;
            _PositionSmash.Y = -2;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public IEnumerable Stretch()
        {
            _BobySmash.X = 10;
            _BobySmash.Y = -5;
            _PositionSmash.X = 2;
            _PositionSmash.Y = 3;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public void ResetSpriteSizes()
        {
            _BobySmash = new Point(0, 0);
            _PositionSmash = new Vector2(0, 0);
        }
    }
}