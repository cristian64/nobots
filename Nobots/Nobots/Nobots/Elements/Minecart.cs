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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Collision.Shapes;

namespace Nobots.Elements
{
    public class Minecart : Element, IActivable
    {
        Body body;
        Body leftWheel;
        Body rightWheel;
        Texture2D texture;
        Texture2D textureWheel;
        LineJoint leftJoint;
        LineJoint rightJoint;
        int collisionsNumber = 0;

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
                leftWheel.FixedRotation = rightWheel.FixedRotation = !value;
                if (!isActive)
                    leftWheel.AngularVelocity = rightWheel.AngularVelocity = 0;
                leftJoint.MotorEnabled = rightJoint.MotorEnabled = value;
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
                leftWheel.Position = value + new Vector2(-Width / 3, Height / 2);
                rightWheel.Position = value + new Vector2(Width / 3, Height / 2);
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
            }
        }

        public Minecart(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 1f;
            texture = Game.Content.Load<Texture2D>("minecart");
            textureWheel = Game.Content.Load<Texture2D>("minecartwheel");
            body = new Body(scene.World);
            
            //adding the floor of the cart
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(0.46f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.04f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.24f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.26f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            PolygonShape p = new PolygonShape(vertices, 100);
            Fixture f1 = new Fixture(body, p);

            //adding the left wall of the cart
            vertices.Clear();
            vertices.Add(new Vector2(0.46f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.26f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.46f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            p = new PolygonShape(vertices, 100);
            Fixture f2 = new Fixture(body, p);

            //adding the right wall of the cart
            vertices.Clear();
            vertices.Add(new Vector2(2.04f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.04f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.5f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.24f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            p = new PolygonShape(vertices, 100);
            Fixture f3 = new Fixture(body, p);

            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.All & ~ElementCategory.ENERGY;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
            body.Mass = 1000;
            body.UserData = this;

            //creating the left wheel
            leftWheel = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(textureWheel.Height/2), 200);
            leftWheel.Position = Position + new Vector2(-Width / 3, Height / 2);
            leftWheel.BodyType = BodyType.Dynamic;
            leftWheel.Friction = 1;
            leftWheel.UserData = this;
            leftWheel.CollidesWith = Category.All & ~ElementCategory.CHARACTER & ~ElementCategory.ENERGY;

            leftJoint = new LineJoint(body, leftWheel, leftWheel.Position, new Vector2(0.8f, 0.6f));
            leftJoint.MotorSpeed = 0.0f;
            leftJoint.MaxMotorTorque = 20.0f;
            leftJoint.MotorEnabled = true;
            leftJoint.Frequency = 100;
            scene.World.AddJoint(leftJoint);

            //creating the right wheel
            rightWheel = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(textureWheel.Height/2), 200);
            rightWheel.Position = Position + new Vector2(Width / 3, Height / 2);
            rightWheel.BodyType = BodyType.Dynamic;
            rightWheel.Friction = 1;
            rightWheel.UserData = this;
            rightWheel.CollidesWith = Category.All & ~ElementCategory.CHARACTER & ~ElementCategory.ENERGY;

            rightJoint = new LineJoint(body, rightWheel, rightWheel.Position, new Vector2(0.8f, 0.6f));
            rightJoint.MotorSpeed = 0.0f;
            rightJoint.MaxMotorTorque = 20.0f;
            rightJoint.MotorEnabled = true;
            rightJoint.Frequency = 100;
            scene.World.AddJoint(rightJoint);
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            collisionsNumber--;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            collisionsNumber++;

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(textureWheel, scene.Camera.Scale * Conversion.ToDisplay(leftWheel.Position - scene.Camera.Position), null, Color.White, leftWheel.Rotation, new Vector2(textureWheel.Width / 2.0f, textureWheel.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(textureWheel, scene.Camera.Scale * Conversion.ToDisplay(rightWheel.Position - scene.Camera.Position), null, Color.White, rightWheel.Rotation, new Vector2(textureWheel.Width / 2.0f, textureWheel.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            scene.World.RemoveJoint(rightJoint);
            scene.World.RemoveJoint(leftJoint);
            body.Dispose();
            leftWheel.Dispose();
            rightWheel.Dispose();
            base.Dispose(disposing);
        }
    }
}
