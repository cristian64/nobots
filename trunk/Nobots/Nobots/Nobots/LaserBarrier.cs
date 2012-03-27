using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;

namespace Nobots
{
    class LaserBarrier : Element, IActivable
    {
        float height = Conversion.ToWorld(250);
        float width = Conversion.ToWorld(15);
        Body body;

        public bool Active;

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public override float Width
        {
            get { return width; }
            set { }
        }

        public override float Height
        {
            get { return height; }
            set { }
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

        public LaserBarrier(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 10f;
            body = BodyFactory.CreateRectangle(scene.World, width, height, 0);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.OnCollision += body_OnCollision;
            body.OnSeparation += body_OnSeparation;
            body.UserData = this;
            body.IsSensor = true;
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine("pollaca! ");
            if (Active && fixtureB.Body.UserData as Character != null)
            {
                ((Character)fixtureB.Body.UserData).body.ApplyLinearImpulse(Vector2.UnitX * -300);
                //TODO: change character state to "dying..."
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
            scene.LaserParticleSystem.AddParticle(Position - velocity * (Height / 2), velocity);
            scene.LaserParticleSystem.AddParticle(Position + velocity * (Height / 2), -velocity);
            scene.LaserParticleSystem.AddParticle(Position - velocity * (Height / 2), velocity);
            scene.LaserParticleSystem.AddParticle(Position + velocity * (Height / 2), -velocity);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            /*scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(emitterTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X), 
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y) - laserTexture.Height/4 - emitterTexture.Height/2,
                emitterTexture.Width/2, emitterTexture.Height), null, Color.White, body.Rotation, new Vector2(emitterTexture.Width / 2, emitterTexture.Height / 2), SpriteEffects.None, 0);

            scene.SpriteBatch.Draw(laserTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X),
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y), laserTexture.Width / 4, laserTexture.Height/2), 
                null, Color.White, body.Rotation, new Vector2(laserTexture.Width / 2, laserTexture.Height / 2), SpriteEffects.None, 0);

            scene.SpriteBatch.End();*/

            base.Draw(gameTime);
        }
    }
}
