using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;

namespace Nobots.Elements
{
    public class Socket : Element
    {
        private Socket otherSocket;
        public Socket OtherSocket
        {
            get
            {
                if (otherSocket == null)
                    foreach (Element e in scene.Elements)
                    {
                        if (e as Socket != null && e.Id == otherSocketId)
                        {
                            otherSocket = (Socket)e;
                            break;
                        }
                    }
                Debug.Assert(otherSocket == null, "The socket " + Id + " looked for " + otherSocketId + " but that ID wasn't in the list.");
                return otherSocket;
            }
        }

        private String otherSocketId;
        public String OtherSocketId
        {
            get { return otherSocketId; }
            set { otherSocketId = value; otherSocket = null; }
        }

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
            texture = Game.Content.Load<Texture2D>("wires_end");
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
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
