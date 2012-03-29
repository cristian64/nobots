using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    public class Socket : Element
    {
        public Socket OtherSocket;

        Body body;
        Texture2D texture;

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
            get { return 0; }
            set { }
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

        public Socket(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("socket");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 20f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = Category.None | ElementCategory.ENERGY;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
