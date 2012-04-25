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
    public class Lever : Activator
    {
        Body body;
        Texture2D texture;
        Texture2D texture2;
        float stickRotation = 0;

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

        public Lever(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("leverbase");
            texture2 = Game.Content.Load<Texture2D>("lever");
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 100f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.None | ElementCategory.FLOOR;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (ActivableElement != null && ActivableElement.Active)
            {
                if (stickRotation < 0.9f)
                    stickRotation += 0.1f;
            }
            else
            {
                if (stickRotation > -0.9f)
                    stickRotation -= 0.1f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position);
            Vector2 stickPosition = position + (texture.Height / 4.0f) * new Vector2((float)Math.Cos(body.Rotation + MathHelper.PiOver2), (float)Math.Sin(body.Rotation + MathHelper.PiOver2));

            scene.SpriteBatch.Draw(texture2, stickPosition, null, Color.White, body.Rotation + stickRotation, new Vector2(texture2.Width / 2, texture2.Height), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, position, null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
