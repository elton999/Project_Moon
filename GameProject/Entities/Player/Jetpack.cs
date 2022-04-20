using System;
using Microsoft.Xna.Framework;
using GameProject.Entities.Player.Interfaces;

namespace GameProject.Entities.Player
{
    public class Jetpack : IPlayerReference
    {
        public Player Player { get; set; }

        public float TimeOfFly = 0;
        public float TimeOnGround = 0;
        public bool CanTurnOffJetpack { get => TimeOfFly >= 1000f; }
        public bool CanTurnOnJetpack { get => TimeOnGround >= 1000f; }

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
            Power = Math.Clamp(Power - FuelDecrement * deltaTime, 0f, 100f);

            NoFuelOnFlight();
        }

        public void NoFuelOnFlight()
        {
            if (Power > 0)
                return;
            TurnOffJetPack();
        }

        public void TurnOffJetPack()
        {
            TimeOfFly = 0;
            Player.SwitchBehavior(new Behavior.PlayerOnGrounded(Player, Player, Player.AsepriteAnimation));
            Player.SwitchState(new States.PlayerStateFall());
        }

        public void RechargeFuel()
        {
            if (Player.IsFlying)
                return;

            Power = Math.Clamp(Power + 1f, 0f, 100f);
            Player.wait(RechargeFuelTime, RechargeFuel);
        }

    }
}