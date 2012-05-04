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
        Texture2D texture3;
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
            ZBuffer = 6f;
            texture = Game.Content.Load<Texture2D>("impulseplatform");
            texture2 = Game.Content.Load<Texture2D>("impulseplatformactive");
            texture3 = Game.Content.Load<Texture2D>("impulseplatformsensor");
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

        private float alpha = 0.0f;
        public float Acceleration = 20;

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = Vector2.Normalize(body2.Position - body.Position);
            if (Active)
            {
                foreach (Body i in bodies)
                {
                    float forceToApply = Acceleration * i.Mass / i.FixtureList.Count;
                    i.ApplyForce(direction * forceToApply);
                }
            }

            if (isActive && alpha < 1.0f)
                alpha = (float)Math.Min(1.0f, alpha + gameTime.ElapsedGameTime.TotalSeconds);

            if (!isActive && alpha > 0.0f)
                alpha = (float)Math.Max(0.0f, alpha - gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(GameTime gameTime)
        {
            if (alpha > 0)
            {
                float scale = scene.Camera.Scale;
                Rectangle rectangle = new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (body2.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (body2.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(Width * scale)), (int)Math.Round(Conversion.ToDisplay(Height * scale)));
                scene.SpriteBatch.Draw(texture3, rectangle, null, Color.White * alpha, body2.Rotation, new Vector2(texture3.Width / 2.0f, texture3.Height / 2.0f), SpriteEffects.None, 0);
            }
            scene.SpriteBatch.Draw(Active ? texture2 : texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
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
