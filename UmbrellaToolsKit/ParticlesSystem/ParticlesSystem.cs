using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class ParticlesSystem : GameObject
    {
        public List<Particle> Particles = new List<Particle>();
        public List<Texture2D> Sprites = new List<Texture2D>();
        public int MaxParticles = 10;
        public Random Random = new Random();
        public float MaxSize = 5f;
        public Vector2 Velocity = new Vector2(1000f, 1000f);

        public override void Start()
        {
            UmbrellaToolsKit.Sprite.Square s = new UmbrellaToolsKit.Sprite.Square();
            s.Scene = Scene;
            s.size = new Point(1, 1);
            s.SquareColor = Color.White;
            s.Start();

            Sprites.Add(s.Sprite);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(gameTime);
                if (Particles[i].LifeTime <= 0)
                    Particles.RemoveAt(i);
            }

            for (int i = 0; i < MaxParticles; i++)
                Particles.Add(CreateParticle());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            foreach (var particle in Particles)
                particle.DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }

        public Particle CreateParticle()
        {
            return new Particle()
            {
                Position = this.Position + Vector2.One * Random.Next(0, 10),
                Scale = Random.Next(1, 10) * (MaxSize / 10f),
                Angle = new Vector2(Random.Next(-10, 10), Random.Next(-10, 10)),
                Velocity = new Vector2(Random.Next(-10, 10) / Velocity.X, Random.Next(-10, 10) / Velocity.Y),
                Sprite = Sprites[Random.Next(0, Sprites.Count - 1)]
            };
        }
    }
}