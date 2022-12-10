using UmbrellaToolsKit;
using Microsoft.Xna.Framework;
using System.Collections;

namespace GameProject.Entities.Components
{
    public class DemageEfxComponent : Component
    {
        public GameObject GameObject;
        public bool IsTakingDamage = false;

        public DemageEfxComponent(GameObject gameObject) => GameObject = gameObject;
        public CoroutineManagement CoroutineManagement = new();

        public override void Update(GameTime gameTime)
        {
            CoroutineManagement.Update(gameTime);
            base.Update(gameTime);
        }

        public void DamageEfx() => CoroutineManagement.StarCoroutine(_damageEfxAnimation());

        private IEnumerator _damageEfxAnimation()
        {
            IsTakingDamage = true;
            for (int i = 0; i < 60 * 4; i++)
            {
                if (CoroutineManagement.GameTime.TotalGameTime.TotalMilliseconds % 8 > 4)
                {
                    GameObject.SpriteColor = Color.Red;
                    GameObject.Transparent = 0.7f;
                }
                else
                {
                    GameObject.SpriteColor = Color.White;
                    GameObject.Transparent = 1;
                }
                yield return null;
            }

            IsTakingDamage = false;
            GameObject.SpriteColor = Color.White;
            GameObject.Transparent = 1f;

            yield return null;
        }
    }
}