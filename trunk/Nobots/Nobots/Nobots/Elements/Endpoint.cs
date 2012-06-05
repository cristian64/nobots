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
    public class Endpoint : Element
    {
        Body body;
        Texture2D texture;

        private bool alsoEnergy;
        public bool AlsoEnergy
        {
            get { return alsoEnergy; }
            set
            {
                alsoEnergy = value;
                if (body != null)
                    body.CollidesWith = (alsoEnergy == true) ? ElementCategory.CHARACTER | ElementCategory.ENERGY : ElementCategory.CHARACTER;
            }

        }

        public String NextLevel = "";

        public override float Width
        {
            get { return Conversion.ToWorld(texture.Width); }
            set { }
        }

        public override float Height
        {
            get { return Conversion.ToWorld(texture.Height); }
            set { }
        }

        public override Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = value; }
        }

        public override float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
        }

        public Endpoint(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("endpoint");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 150f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.CollidesWith = ElementCategory.CHARACTER;
            body.IsSensor = true;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

            body.UserData = this;
            AlsoEnergy = false;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (NextLevel != "")
            {
                scene.CleanAndLoad(NextLevel);
            }

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            //scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
