using System.Collections;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities
{
    public class SpriteDeforme : Actor
    {
        protected Vector2 _positionSmash;
        protected Point _bodySmash;

        public CoroutineManagement CoroutineManagement = new();

        public IEnumerator Squash()
        {
            _bodySmash.X = -15;
            _bodySmash.Y = 5;
            _positionSmash.X = 2;
            _positionSmash.Y = -2;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public IEnumerable Stretch()
        {
            _bodySmash.X = 10;
            _bodySmash.Y = -5;
            _positionSmash.X = 2;
            _positionSmash.Y = 3;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public void ResetSpriteSizes()
        {
            _bodySmash = new Point(0, 0);
            _positionSmash = new Vector2(0, 0);
        }
    }
}