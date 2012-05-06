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
using IrrKlang;

namespace Nobots.Elements
{
    public class Socket : Element
    {
        private Socket otherSocket;
        public Socket OtherSocket
        {
            get
            {
                ISound aux = scene.SoundManager.ISoundEngine.Play2D(sounds[rand.Next(3)], false, true);
                aux.Volume = 0.05f;
                aux.Paused = false;
                if (otherSocket == null)
                    foreach (Element e in scene.Elements)
                    {
                        if (e as Socket != null && e.Id == otherSocketId)
                        {
                            otherSocket = (Socket)e;
                            break;
                        }
                    }

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
        private List<String> sounds = new List<String>();
        Random rand = new Random();

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
            get { return body.Rotation; }
            set 
            {
                body.Rotation = value;
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

            sounds.Add("Content\\sounds\\effects\\travelcord1.wav");
            sounds.Add("Content\\sounds\\effects\\travelcord2.wav");
            sounds.Add("Content\\sounds\\effects\\travelcord3.wav");
            

            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
