using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    class LaserBarrier : Element, IActivable
    {
        Body body;
        Texture2D emitterTexture;
        Texture2D laserTexture;
        bool isActive;

        void IActivable.Activate()
        {
            isActive = true;
        }

        void IActivable.Deactivate()
        {
            isActive = false;
        }

        public override float Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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

        public LaserBarrier(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 10f;
            emitterTexture = Game.Content.Load<Texture2D>("laserEmitter");
            laserTexture = Game.Content.Load<Texture2D>("laser");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(laserTexture.Width/4), Conversion.ToWorld(laserTexture.Height/2), 150f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
            body.UserData = this;
            body.IsSensor = true;
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData as Character != null)
            {
                ((Character)fixtureB.Body.UserData).body.ApplyLinearImpulse(Vector2.UnitX * -300);
                //TODO: change character state to "dying..."
            }
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(emitterTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X), 
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y) - laserTexture.Height/4 - emitterTexture.Height/2,
                emitterTexture.Width/2, emitterTexture.Height), null, Color.White, body.Rotation, new Vector2(emitterTexture.Width / 2, emitterTexture.Height / 2), SpriteEffects.None, 0);

            scene.SpriteBatch.Draw(laserTexture, new Rectangle((int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X),
                (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y), laserTexture.Width / 4, laserTexture.Height/2), 
                null, Color.White, body.Rotation, new Vector2(laserTexture.Width / 2, laserTexture.Height / 2), SpriteEffects.None, 0);

            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
