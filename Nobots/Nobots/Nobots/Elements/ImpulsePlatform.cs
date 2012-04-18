using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Nobots.Elements
{
    public class ImpulsePlatform : Element, IActivable
    {
        Body body;
        Body body2;
        Texture2D texture;
        Texture2D texture2;
        List<Body> bodies;

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

        private float height = 7;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                body2.Dispose();
                body2 = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), height, 1);
                body2.IsSensor = true;
                body2.Position = body.Position - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
                body2.BodyType = BodyType.Static;
                body2.CollisionCategories = ElementCategory.FLOOR;
                body2.OnCollision += new OnCollisionEventHandler(body2_OnCollision);
                body2.OnSeparation += new OnSeparationEventHandler(body2_OnSeparation);
                bodies = new List<Body>();
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
                body2.Position = value - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
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
                body.Rotation = body2.Rotation = value;
                body2.Position = body.Position - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
            }
        }

        public ImpulsePlatform(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            texture = Game.Content.Load<Texture2D>("impulseplatform");
            texture2 = Game.Content.Load<Texture2D>("impulseplatformactive");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 1);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.CollisionCategories = ElementCategory.FLOOR;

            body2 = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), height, 1);
            body2.IsSensor = true;
            body2.Position = position - new Vector2(0, height / 2 - Conversion.ToWorld(texture.Height) / 2);
            body2.Position = position - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
            body2.BodyType = BodyType.Static;
            body2.CollisionCategories = ElementCategory.FLOOR;
            body2.OnCollision += new OnCollisionEventHandler(body2_OnCollision);
            body2.OnSeparation += new OnSeparationEventHandler(body2_OnSeparation);

            bodies = new List<Body>();
            Active = true;
        }

        void body2_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            bodies.Remove(fixtureB.Body);
        }

        bool body2_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            bodies.Add(fixtureB.Body);
            return true;
        }

        public float Force = 20;

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = Vector2.Normalize(body2.Position - body.Position);
            if (Active)
            {
                foreach (Body i in bodies)
                {
                    float forceToApply = Force * i.Mass;
                    i.ApplyForce(direction * forceToApply);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(Active ? texture2 : texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            body2.Dispose();
            bodies.Clear();
            base.Dispose(disposing);
        }
    }
}
