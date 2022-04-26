using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class CameraManagement
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get => new Vector2(Scene.Sizes.X / 2, Scene.Sizes.Y / 2) / 1; }
        public Vector2 Target { get; set; }
        public Vector2 ScreenSize { get; set; }
        public Vector2 ScreenTargetAreaLimits { get; set; }
        public Vector2 ScreenSizeLimits { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public Scene Scene;

        public CameraManagement()
        {
            Zoom = 1f;
            Scale = 1f;
            MoveSpeed = 8f;
            Rotation = 0f;
        }

        public void update(GameTime gameTime)
        {

            InitialPosition = _position;
            Shake(gameTime);

            if (Target != Vector2.Zero)
            {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveX(delta);
                moveY(delta);
                if (Scene.PixelArt) Position = Position.ToPoint().ToVector2();
            }

        }

        public void CheckActorAndSolids()
        {
            var actorCamera = new Collision.Actor();
            actorCamera.size = Scene.Sizes;
            actorCamera.Position = new Vector2(Position.X - Origin.X, Position.Y - Origin.Y);
            List<Collision.Actor> actorsList = new List<Collision.Actor>();
            actorsList.AddRange(Scene.AllActors);

            foreach (Collision.Actor actor in actorsList)
                if (actor.overlapCheck(actorCamera))
                    actor.Isvisible();
                else
                    actor.IsNotvisible();
        }

        public bool UseLevelLimits = true;
        public void moveX(float delta)
        {
            _position.X = MathHelper.Lerp(Position.X, Target.X, MoveSpeed * delta);
            if (UseLevelLimits)
            {
                float maxValue = Scene.LevelSize.X + Scene.ScreenOffset.X - Origin.X;
                float minValue = Scene.ScreenOffset.X + Origin.X;
                _position.X = Math.Max(_position.X, minValue);
                _position.X = Math.Min(_position.X, maxValue);
            }
        }

        public void moveY(float delta)
        {
            _position.Y = MathHelper.Lerp(Position.Y, Target.Y, MoveSpeed * delta);

            if (UseLevelLimits)
            {
                float maxValue = Scene.LevelSize.Y + Scene.ScreenOffset.Y - Origin.Y;
                float minValue = Scene.ScreenOffset.Y + Origin.Y;
                _position.Y = Math.Min(_position.Y, maxValue);
                _position.Y = Math.Max(_position.Y, minValue);

            }
        }

        #region shake

        public float TimeShake;
        public static readonly Random getRandom = new Random();
        public Vector2 InitialPosition;
        public float ShakeMagnitude = 0.05f;

        private void Shake(GameTime gameTime)
        {
            if (this.TimeShake > 0)
            {
                int randomX = getRandom.Next(-5, 5);
                int randomY = getRandom.Next(-5, 5);

                this.Target = new Vector2(
                    this.InitialPosition.X + randomX * this.ShakeMagnitude,
                    this.InitialPosition.Y + randomY * this.ShakeMagnitude
                );
                this.TimeShake -= 1;
            }
        }
        #endregion

        public Matrix TransformMatrix()
        {
            return Matrix.CreateRotationZ(this.Rotation) *
                Matrix.CreateTranslation(-(int)this.Position.X, -(int)this.Position.Y, 0) *
                Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, Zoom));

        }

    }
}
