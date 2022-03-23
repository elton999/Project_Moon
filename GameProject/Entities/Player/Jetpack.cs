using Microsoft.Xna.Framework;
using GameProject.Entities.Player.Interfaces;

namespace GameProject.Entities.Player
{
    public class Jetpack
    {
        public static float RechargeFuelTime = 0.2f;
        public static float FuelDecrement = 20f;

        public static void CheckFuel(GameTime gameTime, Player player)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.Scene.GameManagement.Values["POWER"] = player.Scene.GameManagement.Values["POWER"] - FuelDecrement * deltaTime;

            if (player.Scene.GameManagement.Values["POWER"] <= 0) 
                NoFuelOnFlight(player);
        }

        public static void NoFuelOnFlight(Player player)
        {
            player.SwitchBehavior(new Behavior.PlayerOnGrounded(player, player, player.AsepriteAnimation));
            player.SwitchState(new States.PlayerStateFall());
        }

        public static void RechargeFuel(Player player)
        {
            if (player.Scene.GameManagement.Values["POWER"] < 100)
            {
                player.Scene.GameManagement.Values["POWER"] += 1;
                player.wait(RechargeFuelTime, () => RechargeFuel(player));
            }
        }

    }
}