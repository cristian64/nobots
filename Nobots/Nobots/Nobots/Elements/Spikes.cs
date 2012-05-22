using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    public class Spikes : Element
    {
        public Body body;
        Texture2D texture;

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
            }
        }

        public Spikes(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 5f;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("spikes");
            createBody();
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if (body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollisionCategories = ElementCategory.FLOOR;
            body.CollidesWith = ElementCategory.CHARACTER | ElementCategory.ENERGY;
            body.OnCollision +=new OnCollisionEventHandler(body_OnCollision);
        }

        Random random = new Random();
        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Energy)
            {
                Energy energy = (Energy)fixtureB.Body.UserData;
                for (int j = 0; j < 50; j++)
                {
                    scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                    scene.PlasmaExplosionParticleSystem.AddParticle(energy.Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                }
                scene.GarbageElements.Add(energy);
                foreach (Element el in scene.Elements)
                    if (el is Character && !(el is Energy) && !(((Character)el).State is DyingCharacterState))
                    {
                        ((Character)el).State = new DyingCharacterState(scene, (Character)el);
                        break;
                    }
            }
            else
            {
                if (!(((Character)fixtureB.Body.UserData).State is DyingCharacterState))
                {
                    ((Character)fixtureB.Body.UserData).State = new DyingCharacterState(scene, (Character)fixtureB.Body.UserData);
                }
            }

            return true;
        }


        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
