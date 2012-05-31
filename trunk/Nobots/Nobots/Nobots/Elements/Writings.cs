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
    public class Writings : Activator
    {
        Body body;
        Texture2D texture;
        Texture2D texture2;

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

        public Writings(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            EnergyElement = true;
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("writings");
            texture2 = Game.Content.Load<Texture2D>("writings");
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 20f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = Category.None | ElementCategory.CHARACTER | ElementCategory.ENERGY;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
            body.UserData = this;
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (ActivableElement != null)
                ActivableElement.Active = false;
        }

        public override void Activate()
        {
            //base.Activate(); not to active or deactive anything with the B button
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (ActivableElement != null)
                ActivableElement.Active = true;
            return true;
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
