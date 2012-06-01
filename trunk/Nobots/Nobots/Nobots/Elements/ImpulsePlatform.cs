using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IrrKlang;

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
        ISound sound;

        private int stepsNumber;
        public int StepsNumber
        {
            get
            {
                return stepsNumber;
            }
            set
            {
                stepsNumber = value;
                createBody();
            }
        }

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

                if (value)
                {
                    sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.ImpulsePlatform, body.Position.X, body.Position.Y, 0f, true, false, false);
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerUp[3], body.Position.X, body.Position.Y, 0f, false, false, false);
                }
                else
                    if (sound != null)
                    {
                        scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerDown[3], body.Position.X, body.Position.Y, 0f, false, false, false);
                        sound.Stop();
                    }

                if (bodies != null)
                    foreach (Body i in bodies)
                        i.Awake = true;
            }
        }

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width) * stepsNumber;
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
                position = body.Position = value;
                body2.Position = value - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
            }
        }

        float rotation = 0;
        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                rotation = body.Rotation = body2.Rotation = value;
                body2.Position = body.Position - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));

                direction = new Vector2((float)Math.Cos(body.Rotation), (float)Math.Sin(body.Rotation));
                drawingIncrement = direction * Conversion.ToWorld(texture.Width);
                drawingShift = direction * (Width / 2) - drawingIncrement / 2.0f;
            }
        }

        public ImpulsePlatform(Game game, Scene scene, Vector2 position, int stepsNumber = 1)
            : base(game, scene)
        {
            ZBuffer = 6f;
            texture = Game.Content.Load<Texture2D>("impulseplatform");
            texture2 = Game.Content.Load<Texture2D>("impulseplatformactive");
            texture3 = Game.Content.Load<Texture2D>("impulseplatformsensor");
            this.stepsNumber = stepsNumber;
            this.position = position;
            createBody();
        }

        void createBody()
        {
            if (body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Conversion.ToWorld(texture.Height), 1);
            body.Position = position;
            body.Rotation = rotation;
            body.BodyType = BodyType.Static;
            body.CollisionCategories = ElementCategory.FLOOR;

            if (body2 != null)
                body2.Dispose();
            body2 = BodyFactory.CreateRectangle(scene.World, Width, Height, 1);
            body2.IsSensor = true;
            body2.Position = position - (height / 2 - Conversion.ToWorld(texture.Height) / 2) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));
            body2.Rotation = rotation;
            body2.BodyType = BodyType.Static;
            body2.OnCollision += new OnCollisionEventHandler(body2_OnCollision);
            body2.OnSeparation += new OnSeparationEventHandler(body2_OnSeparation);
            bodies = new List<Body>();

            direction = new Vector2((float)Math.Cos(body.Rotation), (float)Math.Sin(body.Rotation));
            drawingIncrement = direction * Conversion.ToWorld(texture.Width);
            drawingShift = direction * (Width / 2) - drawingIncrement / 2.0f;
        }

        private Vector2 direction;
        private Vector2 drawingIncrement;
        private Vector2 drawingShift;

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

        public bool IsTouchingElement(Element o)
        {
            return Math.Abs(body2.Position.X - o.Position.X) <= 0.5 * (Width + o.Width) &&
                   Math.Abs(body2.Position.Y - o.Position.Y) <= 0.5 * (Height + o.Height);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = Vector2.Normalize(body2.Position - body.Position);
            if (Active)
            {
                foreach (Body i in bodies)
                {
                    if (!i.IsDisposed && IsTouchingElement((Element)i.UserData))
                    {
                        float forceToApply = Acceleration * i.Mass / i.FixtureList.Count;
                        i.ApplyForce(direction * forceToApply);
                    }
                }
            }

            if (isActive && alpha < 1.0f)
                alpha = (float)Math.Min(1.0f, alpha + gameTime.ElapsedGameTime.TotalSeconds);

            if (!isActive && alpha > 0.0f)
                alpha = (float)Math.Max(0.0f, alpha - gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;

            if (alpha > 0)
            {
                Rectangle rectangle = new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (body2.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (body2.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(texture.Width * stepsNumber * scale), (int)Math.Round(Conversion.ToDisplay(Height * scale) - texture.Height * scale));
                scene.SpriteBatch.Draw(texture3, rectangle, null, Color.White * alpha, body2.Rotation, new Vector2(texture3.Width / 2.0f, texture3.Height / 2.0f), SpriteEffects.None, 0);
            }

            Vector2 auxiliarPosition = body.Position - drawingShift;
            for (int i = 0; i < stepsNumber; i++)
            {
                scene.SpriteBatch.Draw(Active ? texture2 : texture, scene.Camera.Scale * Conversion.ToDisplay(auxiliarPosition - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
                auxiliarPosition += drawingIncrement;
            }
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
