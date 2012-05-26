#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
#endregion

namespace Nobots.ParticleSystem
{
    class LightningParticle
    {
        public int TextureIndex = 0;
        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0.0f;
        public Vector2 Scale = Vector2.One;
        public static float DefaultTimeToLive = 0.33f;
        public float TimeToLive = DefaultTimeToLive;
    }

    public class LightningParticleSystem : DrawableGameComponent
    {
        public static LightningParticleSystem LastInstance = null;
        Scene scene;
        List<LightningParticle> particles;
        List<Texture2D> textures;
        Vector2 origin;
        Vector2 size;
        static Random random = new Random();

        public LightningParticleSystem(Game game, Scene scene)
            : base(game)
        {
            Initialize();
            LastInstance = this;
            this.scene = scene;
            particles = new List<LightningParticle>(100);
            textures = new List<Texture2D>(15);
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning1"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning2"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning3"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning4"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning5"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning6"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning7"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning8"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning9"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning10"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning11"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning12"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning13"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning14"));
            textures.Add(Game.Content.Load<Texture2D>("particlesystem\\lightning15"));
            origin = new Vector2(textures[0].Width / 2.0f, textures[0].Height / 2.0f);
            size = Conversion.ToWorld(new Vector2(textures[0].Width, textures[0].Height));
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (LightningParticle i in particles)
                i.TimeToLive -= elapsed;

            for (int i = particles.Count - 1; i >= 0; i--)
                if (particles[i].TimeToLive < 0)
                    particles.RemoveAt(i);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            float scale = scene.Camera.Scale;
            foreach (LightningParticle i in particles)
            {
                scene.SpriteBatch.Draw(textures[i.TextureIndex], scale * Conversion.ToDisplay(i.Position - scene.Camera.Position), null, Color.White * (i.TimeToLive / LightningParticle.DefaultTimeToLive), i.Rotation, origin, scale * i.Scale, SpriteEffects.None, 0);
            }
            scene.SpriteBatch.End();
        }

        public void AddParticle(Vector2 startPosition, Vector2 endPosition)
        {
            LightningParticle particle = new LightningParticle();
            particle.Position = (endPosition + startPosition) / 2.0f;
            particle.Rotation = (float)Math.Atan2((endPosition - startPosition).X, -(endPosition - startPosition).Y) - MathHelper.PiOver2;
            particle.Scale = new Vector2(Vector2.Distance(startPosition, endPosition) / size.X, 1);
            particle.TextureIndex = random.Next(15);
            particles.Add(particle);
        }
    }
}
