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
using IrrKlang;

namespace Nobots.Elements
{
    public class LaserBarrier : Element, IActivable
    {
        float height = Conversion.ToWorld(550);
        float width = Conversion.ToWorld(15);
        Body body;
        ISound sound;
        Vector3D pos = new Vector3D(0f, 0f, 0f);
        
        Random rand = new Random();

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
                {
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerUp[3], body.Position.X, body.Position.Y, 0f,false,false,false);
                    body.CollidesWith = Category.None | ElementCategory.CHARACTER;
                }else
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerDown[1], body.Position.X, body.Position.Y, 0f, false, false, false);
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


                pos.X = value.X;
                pos.Y = value.Y;
                sound.Position = pos;
               
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
            this.position = position;

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

            sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.laserBarrierLoop, body.Position.X, body.Position.Y, 0.0f, true, false,false);
           
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
                scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.laserBarrierShocks[rand.Next(scene.SoundManager.laserBarrierShocks.Count)], body.Position.X, body.Position.Y, 0.0f, false, false,false);
               
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

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            sound.Dispose();
            base.Dispose(disposing);
        }
    }
}
