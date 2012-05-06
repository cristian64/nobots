using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IrrKlang;

namespace Nobots.Elements
{
    public class Crate : Element, IPullable, IPushable
    {
        Body body;
        Texture2D texture;
        ISound sound;

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

        private String color = "Blue";
        public String Color
        {
            get { return color; }
            set { color = value; texture = Game.Content.Load<Texture2D>("crate_" + color.ToLower()); }
        }

        public Crate(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            texture = Game.Content.Load<Texture2D>("crate_blue");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 90f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Friction = 1000.0f;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

          
            

            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {

            
            float velocity = body.LinearVelocity.Length();

            if (velocity > 1f)
            {
                sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.woodenBox, body.Position.X, body.Position.Y, 0.0f, false, false, false);
                sound.Volume = velocity * 0.15f * scene.SoundManager.woodenBox.DefaultVolume;

            }
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Microsoft.Xna.Framework.Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
