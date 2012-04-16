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

namespace Nobots.Elements
{
    public class LaserBarrier : Element, IActivable
    {
        float height = Conversion.ToWorld(550);
        float width = Conversion.ToWorld(15);
        Body body;

        private bool isActive = true;

        public bool Active
        {
            get 
            {
                return isActive;
            }

            set
            {
                isActive = value;
                body.CollidesWith = Category.None;
                if (isActive)
                    body.CollidesWith = Category.None | ElementCategory.CHARACTER;
            }
        }

        public override float Width
        {
            get { return width; }
            set { }
        }

        public override float Height
        {
            get { return height; }
            set 
            {
                height = value;
                createBody();
            }
        }

        Vector2 position;
        public override Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
                position = value;
            }
        }

        float rotation;
        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
                rotation = value;
            }
        }

        public LaserBarrier(Game game, Scene scene, Vector2 position, float? height = null)
            : base(game, scene)
        {
            ZBuffer = 10f;
            if (height != null)
            {
                this.height = height.Value;
            }
            createBody();
        }

        private void createBody()
        {
            if (body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 0);
            body.Position = position;
            body.Rotation = rotation;
            body.BodyType = BodyType.Static;
            body.OnCollision += body_OnCollision;
            body.OnSeparation += body_OnSeparation;
            body.UserData = this;
            body.CollidesWith = Category.None | ElementCategory.CHARACTER;

        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Active && fixtureB.Body.UserData as Character != null)
            {
                //((Character)fixtureB.Body.UserData).body.ApplyLinearImpulse(Vector2.UnitX * -300);
                //TODO: change character state to "dying..."
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Vector2 velocity = new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
                //velocity = (velocity / 2) * height;
                scene.LaserParticleSystem.AddParticle(Position - velocity * (Height / 2), (velocity / 2) * height);
                scene.LaserParticleSystem.AddParticle(Position + velocity * (Height / 2), -(velocity / 2) * height);
                scene.LaserParticleSystem.AddParticle(Position - velocity * (Height / 2), (velocity / 2) * height);
                scene.LaserParticleSystem.AddParticle(Position + velocity * (Height / 2), -(velocity / 2) * height);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            scene.SpriteBatch.Draw(emitterTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X), 
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y) - laserTexture.Height/4 - emitterTexture.Height/2,
                emitterTexture.Width/2, emitterTexture.Height), null, Color.White, body.Rotation, new Vector2(emitterTexture.Width / 2, emitterTexture.Height / 2), SpriteEffects.None, 0);

            scene.SpriteBatch.Draw(laserTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X),
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y), laserTexture.Width / 4, laserTexture.Height/2), 
                null, Color.White, body.Rotation, new Vector2(laserTexture.Width / 2, laserTexture.Height / 2), SpriteEffects.None, 0);
            */
        }
    }
}
