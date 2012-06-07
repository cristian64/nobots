using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    public class ElectricityBox : Activator
    {
        Body body;
        Texture2D texture;
        Texture2D texture2;
        ParticleSystem.ParticleEmitter particleEmitter;

        public override void Activate()
        {
            base.Activate();
            if (ActivableElement != null)
            {
                if (ActivableElement.Active)
                {
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerUp[2], body.Position.X, body.Position.Y, 0f, false, false, false);
                }
                else
                {
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerDown[2], body.Position.X, body.Position.Y, 0f, false, false, false);
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
                particleEmitter = new ParticleSystem.ParticleEmitter(scene.SmokePlumeParticleSystem, 300, new Vector3(body.Position.X, body.Position.Y, 0));
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

        public ElectricityBox(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            EnergyElement = true;
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("electricitybox");
            texture2 = Game.Content.Load<Texture2D>("electricitybox2");
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 20f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = Category.None | ElementCategory.ENERGY;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.UserData = this;

            particleEmitter = new ParticleSystem.ParticleEmitter(scene.SmokePlumeParticleSystem, 300, new Vector3(body.Position.X, body.Position.Y, 0));
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (ActivableElement == null || !ActivableElement.Active)
                particleEmitter.Update(gameTime, new Vector3(body.Position.X - 0.25f, body.Position.Y - 0.2f, 0));
        }

        public override void Draw(GameTime gameTime)
        {
            if (ActivableElement != null && ActivableElement.Active)
                scene.SpriteBatch.Draw(texture2, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
            else
                scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
