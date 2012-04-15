using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    public class PressurePlate : Activator
    {
        Body body;
        Texture2D texture;
        float offset;
        int collisionsNumber = 0;

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width);
            }
            set
            {
                throw new NotImplementedException();
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
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
            }
        }

        public PressurePlate(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 6f;
            texture = Game.Content.Load<Texture2D>("weight");
            Height = Conversion.ToWorld(19.0f);
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Height, 150f);
            body.Position = position;// +new Vector2(0, Height * 3 / 4);
            body.BodyType = BodyType.Static;
            body.Rotation = 0.0f;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
            body.CollidesWith = Category.All & ~Category.Cat11;
            offset = Height * 3 / 4;
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
           // Height = Conversion.ToWorld(texture.Height);
            //offset = Height * 3 / 4;
            if (ActivableElement != null && collisionsNumber == 1)
                ActivableElement.Active = true;
            collisionsNumber--;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //Height = Conversion.ToWorld(texture.Height / 4);    
            //offset = Height * 3 / 2;
            if(ActivableElement != null && collisionsNumber == 0)
                ActivableElement.Active = false;
            collisionsNumber++;

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Begin(); 
            texture = (ActivableElement.Active == true)?Game.Content.Load<Texture2D>("weight"):Game.Content.Load<Texture2D>("weight2");
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)),
                (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)), (int)Conversion.ToDisplay(scale * Width), (int)Conversion.ToDisplay(scale * Conversion.ToWorld(texture.Height))), null, Color.White, 
                body.Rotation, new Vector2(texture.Width / 2, texture.Height - 19.0f/2.0f), SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
