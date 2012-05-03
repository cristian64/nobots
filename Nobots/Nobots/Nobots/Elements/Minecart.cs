﻿using System;
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

namespace Nobots.Elements
{
    public class Minecart : Element, IActivable
    {
        Body body;
        Body leftWheel;
        Body rightWheel;
        Texture2D texture;
        Texture2D textureWheel;
        RevoluteJoint leftJoint;
        RevoluteJoint rightJoint;
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

        public Minecart(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            texture = Game.Content.Load<Texture2D>("minecart");
            textureWheel = Game.Content.Load<Texture2D>("minecartwheel");
            Height = Conversion.ToWorld(texture.Height);
            Vertices vertices = new Vertices(8);
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.26f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.24f - Conversion.ToWorld(texture.Width / 2), 1.4f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.5f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.3f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(2.04f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.46f - Conversion.ToWorld(texture.Width / 2), 1.25f - Conversion.ToWorld(texture.Height / 2)));
            vertices.Add(new Vector2(0.2f - Conversion.ToWorld(texture.Width / 2), -Conversion.ToWorld(texture.Height / 2)));
            body = BodyFactory.CreatePolygon(scene.World, vertices, 100.0f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            leftWheel = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(textureWheel.Height/2), 100);
            leftWheel.Position = Position + new Vector2(-Width / 3, Height / 2);
            leftWheel.BodyType = BodyType.Dynamic;
            leftJoint = new RevoluteJoint(body, leftWheel, new Vector2(-0.8f, 0.6f), Vector2.Zero);
            leftWheel.Friction = 10000;
            leftJoint.MotorEnabled = false;
            leftJoint.MotorSpeed = 0f;
            leftJoint.MaxMotorTorque = 0f;
            scene.World.AddJoint(leftJoint);

            rightWheel = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(textureWheel.Height/2), 100);
            rightWheel.Position = Position + new Vector2(Width / 3, Height / 2);
            rightWheel.BodyType = BodyType.Dynamic;
            rightJoint = new RevoluteJoint(body, rightWheel, new Vector2(0.8f, 0.6f), Vector2.Zero);
            rightWheel.Friction = 10000;
            rightJoint.MotorEnabled = false;
            rightJoint.MotorSpeed = 0f;
            rightJoint.MaxMotorTorque = 0f;
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
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)),
                (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)), (int)Conversion.ToDisplay(scale * Width), (int)Conversion.ToDisplay(scale * Conversion.ToWorld(texture.Height))), null, Color.White, 
                body.Rotation, new Vector2(texture.Width/2, texture.Height/2), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(textureWheel, scene.Camera.Scale * Conversion.ToDisplay(leftWheel.Position - scene.Camera.Position), null, Color.White, leftWheel.Rotation, new Vector2(textureWheel.Width / 2, textureWheel.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(textureWheel, scene.Camera.Scale * Conversion.ToDisplay(rightWheel.Position - scene.Camera.Position), null, Color.White, rightWheel.Rotation, new Vector2(textureWheel.Width / 2, textureWheel.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}