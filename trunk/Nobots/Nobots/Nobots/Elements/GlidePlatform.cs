using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace Nobots.Elements
{
    public class GlidePlatform : Element, IActivable
    {
        public Body body;
        Texture2D texture;
        Texture2D texture2;
        List<Body> bodies;

        public float Velocity = 5;

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
                body.Friction = isActive ? 0 : float.MaxValue;
                alphaTarget = isActive ? 1 : 0;
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
            }
        }

        private float width;
        public override float Width
        {
            get
            {
                return width * stepsNumber;
            }
            set
            {
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

        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
                createBody();
            }
        }

        public GlidePlatform(Game game, Scene scene, Vector2 position, int stepsNumber)
            : base(game, scene)
        {
            ZBuffer = 6f;
            this.position = position;
            this.stepsNumber = stepsNumber;
            texture = Game.Content.Load<Texture2D>("glideplatform");
            texture2 = Game.Content.Load<Texture2D>("glideplatform2");
            height = Conversion.ToWorld(texture.Height);
            width = Conversion.ToWorld(texture.Width);
            createBody();
        }

        float alpha = 0;
        float alphaTarget = 1;
        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            Vector2 auxiliarPosition = body.Position - drawingShift;
            for (int i = 0; i < stepsNumber; i++)
            {
                scene.SpriteBatch.Draw(texture, scale * Conversion.ToDisplay(auxiliarPosition - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, Velocity > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                if (alpha > 0)
                    scene.SpriteBatch.Draw(texture2, scale * Conversion.ToDisplay(auxiliarPosition - scene.Camera.Position), null, Color.White * alpha, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, Velocity > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                auxiliarPosition += drawingIncrement;
            }
        }

        private Vector2 direction;
        private Vector2 drawingIncrement;
        private Vector2 drawingShift;

        private void createBody()
        {
            float previousRotation = 0;
            if (body != null)
            {
                previousRotation = body.Rotation;
                body.Dispose();
            }
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.Rotation = previousRotation;
            body.Friction = 0;
            body.CollisionCategories = ElementCategory.FLOOR;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
            body.UserData = this;

            direction = new Vector2((float)Math.Cos(body.Rotation), (float)Math.Sin(body.Rotation));
            drawingIncrement = direction * Conversion.ToWorld(texture.Width);
            drawingShift = direction * (Width / 2) - drawingIncrement / 2.0f;

            bodies = new List<Body>();
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            bodies.Remove(fixtureB.Body);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            bodies.Add(fixtureB.Body);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                foreach (Body i in bodies)
                {
                    if (!i.IsDisposed) //TODO: like in impulse platform, it can be improved by checking the object is really "touching" the glideplatform (touching or very very close to it).
                    {
                        i.LinearVelocity = direction * Velocity;
                    }
                }
            }

            if (alpha != alphaTarget)
            {
                if (alpha < alphaTarget)
                    alpha = (float)Math.Min(1, alpha + gameTime.ElapsedGameTime.TotalSeconds * Math.Abs(Velocity));
                else
                    alpha = (float)Math.Max(0, alpha - gameTime.ElapsedGameTime.TotalSeconds * Math.Abs(Velocity));
            }

            if (Active)
            {
                if (alpha == 1)
                    alphaTarget = 0;
                if (alpha == 0 && alpha == alphaTarget)
                    alphaTarget = 1;
            }
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
