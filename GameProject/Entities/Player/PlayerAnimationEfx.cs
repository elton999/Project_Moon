using System.Collections;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit;
using UmbrellaToolsKit.Collision;

namespace GameProject.Entities.Player
{
    public class PlayerAnimationEfx : Actor
    {
        public CoroutineManagement CoroutineManagement = new();

        protected string[] _enemyTags = new string[6] {
             "soldier",
             "spider",
             "bat",
             "jumper",
             "damage",
             "bullet"
        };

        public bool IsTakingDamage = false;

        public void TakeDamage()
        {
            if (IsTakingDamage)
                return;

            IsTakingDamage = true;
            Scene.GameManagement.Values["CURRENT_LIFES"]--;

            CoroutineManagement.ClearCoroutines();
            CoroutineManagement.StarCoroutine(DamageFX(CoroutineManagement.GameTime));
        }

        public IEnumerator DamageFX(GameTime gameTime)
        {
            for (int i = 0; i < 60 * 4; i++)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds % 8 > 4)
                {
                    SpriteColor = Color.Red;
                    Transparent = 0.7f;
                }
                else
                {
                    SpriteColor = Color.White;
                    Transparent = 1;
                }
                yield return null;
            }

            IsTakingDamage = false;
            SpriteColor = Color.White;
            Transparent = 1f;

            yield return null;
        }
    }
}