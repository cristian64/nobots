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
using IrrKlang;

namespace Nobots.Elements
{
    public class PressurePlate : Activator
    {
        Body body;
        Texture2D texture;
        Texture2D texture2;
        Texture2D texture3;
        Texture2D texture4;
        int rotation = 0;
        int targetRotation = 0;
        int collisionsNumber = 0;

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

        private float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
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
                return 0;
            }
            set
            {
            }
        }

        public PressurePlate(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            texture = Game.Content.Load<Texture2D>("weight");
            texture2 = Game.Content.Load<Texture2D>("weight2");
            texture3 = Game.Content.Load<Texture2D>("weight3");
            texture4 = Game.Content.Load<Texture2D>("weight4");
            Height = Conversion.ToWorld(17f);
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width) / 2 + 0.4f, -height / 2));
            vertices.Add(new Vector2(Conversion.ToWorld(texture.Width) / 2 - 0.4f, -height / 2));
            vertices.Add(new Vector2(Conversion.ToWorld(texture.Width) / 2, height / 2));
            vertices.Add(new Vector2(-Conversion.ToWorld(texture.Width) / 2, height / 2));
            body = BodyFactory.CreatePolygon(scene.World, vertices, 100.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (ActivableElement != null && collisionsNumber == 1)
                ActivableElement.Active = false;
            if (collisionsNumber == 1)
                targetRotation = 0;
            collisionsNumber--;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (ActivableElement != null && collisionsNumber == 0)
            {
                ActivableElement.Active = true;
                
            }
            if (collisionsNumber == 0)
                targetRotation = 500;
            collisionsNumber++;

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (targetRotation > rotation)
                rotation += 25;
            else if (targetRotation < rotation)
                rotation -= 25;

            if(rotation == 250)
                scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Weight, body.Position.X, body.Position.Y, 0f, false, false, false);
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - (Vector2.UnitY * (texture.Height - 17) / 2.0f)), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
            if (rotation > 0)
                scene.SpriteBatch.Draw(texture2, scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - (Vector2.UnitY * (texture.Height - 17) / 2.0f)), null, Color.White * (rotation / 500.0f), 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture3, scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - Vector2.UnitY * 230), null, Color.White, rotation / 100.0f, new Vector2(texture3.Width / 2.0f, texture3.Height / 2.0f), scale, SpriteEffects.None, 0);
            if (rotation > 0)
                scene.SpriteBatch.Draw(texture4, scale * (Conversion.ToDisplay(body.Position - scene.Camera.Position) - Vector2.UnitY * 230), null, Color.White, rotation / 100.0f, new Vector2(texture3.Width / 2.0f, texture3.Height / 2.0f), scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
