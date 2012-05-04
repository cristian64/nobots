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
    public class ExperimentalTube : Element, IActivable
    {
        Body body;
        Body sensor;
        Texture2D texture;

        public String OtherTubeId = "";

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
            get { return sensor.Position; }
            set { sensor.Position = value; body.Position = sensor.Position + Vector2.UnitY * Conversion.ToWorld(245 + 60) / 2; }
        }

        public override float Rotation
        {
            get { return 0; }
            set { }
        }

        public ExperimentalTube(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 1f;
            texture = Game.Content.Load<Texture2D>("experimental_tube");

            sensor = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(30), Conversion.ToWorld(245), 150f);
            sensor.Position = position;
            sensor.BodyType = BodyType.Static;
            sensor.IsSensor = true;
            sensor.CollidesWith = ElementCategory.CHARACTER;
            sensor.OnCollision += new OnCollisionEventHandler(sensor_OnCollision);

            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(175), Conversion.ToWorld(60), 150f);
            body.Position = sensor.Position + Vector2.UnitY * Conversion.ToWorld(245 + 60) / 2;
            body.BodyType = BodyType.Static;
            body.FixedRotation = true;
            body.CollisionCategories = ElementCategory.FLOOR;

            body.UserData = this;
        }

        bool sensor_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (isActive && OtherTubeId != "")
            {
                if (fixtureB.Body.UserData is Character)
                {
                    foreach (Element i in scene.Elements)
                    {
                        if (i.Id == OtherTubeId)
                        {
                            ((Character)fixtureB.Body.UserData).State = new ComaCharacterState(scene, (Character)fixtureB.Body.UserData, i.Position);
                            isActive = false;
                            break;
                        }
                    }
                }
            }
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - Vector2.UnitY * (texture.Height - 60) / 2.0f), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            sensor.Dispose();
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
