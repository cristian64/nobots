using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IrrKlang;

namespace Nobots.Elements
{
    public class Radio : Element, IActivable, IBreakable
    {
        Body body;
        Texture2D texture;
        ISound ost;

        private bool isActive = false;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                if (ost != null)
                {
                    ost.Stop();
                    ost.Dispose();
                    ost = null;
                }
                if (isActive)
                {
                    ost = scene.SoundManager.ISoundEngine.Play2D(scene.SoundManager.Credits, false, false, false);
                    scene.AmbienceSound.FadeOut(10);
                }
                else
                {
                    ost = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Credits, body.Position.X, body.Position.Y, 0.0f, false, false, false);
                    scene.AmbienceSound.FadeIn(10);
                }
            }
        }

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width);
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(texture.Height);
            }
            set
            {
            }
        }

        public override Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
            }
        }

        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
            }
        }

        public Radio(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            texture = Game.Content.Load<Texture2D>("radio");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 100f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Friction = 1000.0f;
            body.CollidesWith = Category.All & ~ElementCategory.CHARACTER & ~ElementCategory.ENERGY;
            body.UserData = this;
            body.SleepingAllowed = false;
            Active = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isActive)
            {
                ost.Position = new Vector3D(body.Position.X, body.Position.Y, 0);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        public void Break()
        {
            for (int j = 0; j < 150; j++)
            {
                Vector2 increment = new Vector2((float)scene.Random.NextDouble() - 0.5f, (float)scene.Random.NextDouble() - 0.5f);
                scene.ExplosionSmokeParticleSystem.AddParticle(Position + increment, Vector2.Zero);
            }
            scene.GarbageElements.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (isActive)
            {
                scene.AmbienceSound.FadeIn(10);
            }

            if (ost != null)
            {
                ost.Stop();
                ost.Dispose();
            }
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
