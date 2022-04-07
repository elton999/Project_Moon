using System;
using Microsoft.Xna.Framework;
using GameProject.Entities.Player.Interfaces;

namespace GameProject.Entities.Player
{
    public class Jetpack : IPlayerReference
    {
        public Player Player { get; set; }
        public float Power
        {
            get => Player.Scene.GameManagement.Values["POWER"];
            set => Player.Scene.GameManagement.Values["POWER"] = value;
        }
        public readonly float RechargeFuelTime = 0.2f;
        public readonly float FuelDecrement = 20f;

        public void CheckFuel(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Power -= FuelDecrement * deltaTime;
            Power = Math.Clamp(Power - FuelDecrement * deltaTime, 0f, 100f);

            if (Power == 0) 
                NoFuelOnFlight();
        }

        public  void NoFuelOnFlight()
        {
            Player.SwitchBehavior(new Behavior.PlayerOnGrounded(Player, Player, Player.AsepriteAnimation));
            Player.SwitchState(new States.PlayerStateFall());
        }

        public  void RechargeFuel()
        {
            if (Player.IsFlying)
                return;
                
            Power = Math.Clamp(Power + 1f, 0f, 100f);
            Player.wait(RechargeFuelTime, RechargeFuel);
        }

    }
}