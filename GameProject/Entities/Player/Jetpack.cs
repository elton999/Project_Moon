using Microsoft.Xna.Framework;
namespace GameProject.Entities.Player
{
    public class Jetpack
    {
        private Player _player;

        public Jetpack(Player player)
        {
            this._player = player;
            this._player.wait(this.RechargeFuelTime, RechargeFuel);
        }

        private float Power
        {
            get => this._player.Scene.GameManagement.Values["POWER"];
            set => this._player.Scene.GameManagement.Values["POWER"] = value;
        }
        private float _FuelDecrement = 20f;
        public bool isFly = false;
        private bool _CFlyPressed = false;
        private bool _onFly = false;

        public void Update(GameTime gametime)
        {
            updateFlyStatus();
            checkFuel(gametime);
        }

        private void updateFlyStatus()
        {
            if (this._player.CFly && !this._CFlyPressed && !this._onFly)
            {
                this._onFly = true;
                this._player.moveY(1, null);
            }
            else if (!this._player.CFly && !this._CFlyPressed && this._onFly)
            {
                this._CFlyPressed = true;
            }
            else if (this._player.CFly && this._CFlyPressed && this._onFly || this.Power <= 0)
            {
                this._onFly = false;
            }
            else if (!this._player.CFly && this._CFlyPressed && !this._onFly)
            {
                this._CFlyPressed = false;
            }
        }

        private void checkFuel(GameTime gametime)
        {
            if (this._onFly && this.Power > 0)
            {
                this.Power -= this._FuelDecrement * (float)gametime.ElapsedGameTime.TotalSeconds;
                this._player.velocityDecrecentY = 0;
                this._player.moveY((-15 + this._player.velocity.Y) * (float)gametime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                this._player.gravity2D = new Vector2(0, 0);
                this.isFly = true;
            }
            else this.noFuelOnFlight();
        }

        private void noFuelOnFlight()
        {
            this._player.velocityDecrecentY = 2050;
            this._player.gravity2D = new Vector2(0, this._player.GravityY);
            this.isFly = false;
        }

        private float RechargeFuelTime = 0.2f;
        private void RechargeFuel()
        {
            if (this.Power < 100)
                this.Power = this.Power + 1;

            this._player.wait(this.RechargeFuelTime, RechargeFuel);
        }
    }
}