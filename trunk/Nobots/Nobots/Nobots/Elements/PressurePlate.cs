using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;

namespace Nobots.Elements
{
    public class PressurePlate : Activator
    {
        Body body;
        Texture2D texture;
        int collisionsNumber = 0;

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

        private float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
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

        public PressurePlate(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            texture = Game.Content.Load<Texture2D>("weight");
            Height = Conversion.ToWorld(17f);
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width) / 2 + 0.4f, -height / 2));
            vertices.Add(new Vector2(Conversion.ToWorld(texture.Width) / 2 - 0.4f, -height / 2));
            vertices.Add(new Vector2(Conversion.ToWorld(texture.Width) / 2, height / 2));
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width) / 2, height / 2));
            body = BodyFactory.CreatePolygon(scene.World, vertices, 100.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (ActivableElement != null && collisionsNumber == 1)
                ActivableElement.Active = false;
            collisionsNumber--;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(ActivableElement != null && collisionsNumber == 0)
                ActivableElement.Active = true;
            collisionsNumber++;

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            texture = (collisionsNumber == 0) ? Game.Content.Load<Texture2D>("weight") : Game.Content.Load<Texture2D>("weight2");
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)),
                (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)), (int)Conversion.ToDisplay(scale * Width), (int)Conversion.ToDisplay(scale * Conversion.ToWorld(texture.Height))), null, Color.White, 
                body.Rotation, new Vector2(texture.Width / 2, texture.Height - 19.0f/2.0f), SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
