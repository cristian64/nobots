using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using IrrKlang;
using FarseerPhysics.Dynamics.Joints;

namespace Nobots.Elements
{
    public class Elevator : Element, IActivable
    {
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

        public Vector2 InitialPosition;
        public Vector2 FinalPosition;
        public float Speed = 1f;
        public bool playSound = true;

        Body body;
        Body sensor;
        RevoluteJoint joint;
        Texture2D texture;
        Texture2D chainsTexture;
        Texture2D thingTexture;
        ISound sound;
        Vector3D pos = new Vector3D(0f, 0f, 0f);

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

        public Elevator(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 6f;
            chainsTexture = Game.Content.Load<Texture2D>("elevator_chains");
            texture = Game.Content.Load<Texture2D>("elevator");
            thingTexture = Game.Content.Load<Texture2D>("elevator_upperthing");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width - 30), Conversion.ToWorld(texture.Height - 15), 150f);

            body.Position = position;
            body.BodyType = BodyType.Kinematic;
            body.CollisionCategories = ElementCategory.FLOOR;

            sensor = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width - 50), Conversion.ToWorld(texture.Height - 40), 150f);
            sensor.Position = position;
            sensor.BodyType = BodyType.Dynamic;
            sensor.IsSensor = true;
            sensor.OnCollision += new OnCollisionEventHandler(sensor_OnCollision);
            sensor.CollisionCategories = ElementCategory.FLOOR;

            joint = new RevoluteJoint(body, sensor, Vector2.Zero, Vector2.Zero);
            joint.CollideConnected = false;
            scene.World.AddJoint(joint);

            InitialPosition = body.Position;
            FinalPosition = body.Position - Vector2.UnitY * 3;
        }

        bool sensor_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if(fixtureB.Body.UserData is Energy)
            {
                scene.GarbageElements.Add((Energy)fixtureB.Body.UserData);
                foreach (Element el in scene.Elements)
                    if (el is Character && !(el is Energy) && !(((Character)el).State is DyingCharacterState))
                    {
                        ((Character)el).State = new DyingCharacterState(scene, (Character)el);
                        break;
                    }
            }
            else if (fixtureB.Body.UserData is Character)
            {
                Character c = (Character)fixtureB.Body.UserData;
                c.State = new DyingCharacterState(scene, c);
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 targetPosition = Active ? FinalPosition : InitialPosition;
            if (targetPosition != Position)
            {
                if (playSound)
                {
                    sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.elevatorBegin, Position.X, Position.Y, 0f, false, false, false);
                    playSound = false;

                }

                pos.X = Position.X;
                pos.Y = Position.Y;
                sound.Position = pos;
                



                if (Vector2.DistanceSquared(targetPosition, Position) > Speed * Speed * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
                {
                    Vector2 direction = Vector2.Normalize(targetPosition - Position);
                    body.LinearVelocity = Speed * direction;
                }
                else
                {
                    body.LinearVelocity = Vector2.Zero;
                    Position = targetPosition;
                }
            }
            else {
                if (!playSound)
                {
                    sound.Stop();
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.elevatorEnd, Position.X, Position.Y, 0f, false, false, false);
                    playSound = true;
                }

            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 machinePosition = InitialPosition.Y > FinalPosition.Y ? FinalPosition : InitialPosition;
            scene.SpriteBatch.Draw(chainsTexture, scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - new Vector2(-6, chainsTexture.Height + 30)), null, Color.White, 0, new Vector2(texture.Width / 2.0f, 0), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(thingTexture, scene.Camera.Scale * (Conversion.ToDisplay(machinePosition - scene.Camera.Position) - new Vector2(80, thingTexture.Height + 300)), null, Color.White, 0, new Vector2(texture.Width / 2.0f, 0), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f - 3, texture.Height / 2.0f + 7), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (sound != null)
                sound.Dispose();
            body.Dispose();
            sensor.Dispose();
            scene.World.RemoveJoint(joint);
            base.Dispose(disposing);
        }
    }
}
