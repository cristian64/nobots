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
    public class Battery : Element, IActivable
    {
         public Body body;
        Texture2D texture;
        Random random = new Random();

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

        public Battery(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 5f;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("batteryempty");
            createBody();
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)), (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)),
                (int)Conversion.ToDisplay(Width * scale), (int)Conversion.ToDisplay(Height * scale)), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.UserData = this;
            body.IsSensor = true;
            body.CollidesWith = ElementCategory.ENERGY;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Active)
            {
                texture = Game.Content.Load<Texture2D>("batteryfull");
                for (int j = 0; j < 50; j++)
                {
                    scene.PlasmaExplosionParticleSystem.AddParticle(Position - Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                    scene.PlasmaExplosionParticleSystem.AddParticle(Position + Vector2.UnitY * (float)random.NextDouble() / 2, Vector2.Zero);
                }
                scene.GarbageElements.Add((Energy)fixtureB.Body.UserData);
                foreach (Element el in scene.Elements)
                    if (el is Character && !(el is Energy))
                        ((Character)el).State = new DyingCharacterState(scene, (Character)el);
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
