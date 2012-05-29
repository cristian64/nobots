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
    public class Closet : Element, IPullable, IPushable, IBreakable
    {
        Body body;
        Texture2D texture;
        ISound sound;

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

        public Closet(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            texture = Game.Content.Load<Texture2D>("closet");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 150f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Friction = 1000.0f;
            body.SleepingAllowed = false;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

            sound = scene.SoundManager.ISoundEngine.Play3D("Content\\sounds\\effects\\woodencratefall.wav", body.Position.X, body.Position.Y, 0.0f, false, true);
            

            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {

            
            float velocity = body.LinearVelocity.Length();

            if (velocity > 1f && !fixtureB.CollisionCategories.HasFlag(ElementCategory.LEG))
            {
                sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.woodenBox, body.Position.X, body.Position.Y, 0.0f, false, false, false);
                sound.Volume = velocity * 0.5f * scene.SoundManager.woodenBox.DefaultVolume;

            }
            return true;
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
            sound.Dispose();
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
