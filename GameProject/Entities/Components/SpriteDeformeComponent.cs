using UmbrellaToolsKit;
using System.Collections;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Components
{
    public class SpriteDeformeComponent : Component
    {
        public Vector2 PositionSmash;
        public Point BodySmash;

        public CoroutineManagement CoroutineManagement = new();

        public override void Update(GameTime gametime)
        {
            CoroutineManagement.Update(gametime);
            base.Update(gametime);
        }

        public void Squash() => CoroutineManagement.StarCoroutine(SquashAnimation());
        public void Stretch() => CoroutineManagement.StarCoroutine(StretchAnimation());

        public IEnumerator SquashAnimation()
        {
            BodySmash.X = -15;
            BodySmash.Y = 5;
            PositionSmash.X = 2;
            PositionSmash.Y = -2;

            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public IEnumerator StretchAnimation()
        {
            BodySmash.X = 10;
            BodySmash.Y = -5;
            PositionSmash.X = 2;
            PositionSmash.Y = 3;
            yield return CoroutineManagement.Wait(200f);

            ResetSpriteSizes();
            yield return null;
        }

        public void ResetSpriteSizes()
        {
            BodySmash = new Point(0, 0);
            PositionSmash = new Vector2(0, 0);
        }

    }
}