using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using IrrKlang;

namespace Nobots.Elements
{
    public class Checkpoint : Element, IActivable
    {
        Body body;
        Texture2D texture;
        Texture2D texture2;
        Texture2D shinyBallTexture;
        ISound sound;

        private bool isActive = false;

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
            get { return body.Position; }
            set { body.Position = value; }
        }

        public override float Rotation
        {
            get { return body.Rotation; }
            set { body.Rotation = value; }
        }

        public Checkpoint(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("checkpoint");
            texture2 = Game.Content.Load<Texture2D>("checkpointon");
            shinyBallTexture = Game.Content.Load<Texture2D>("checkpointon2");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 150f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.CollidesWith = ElementCategory.CHARACTER;
            body.IsSensor = true;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);

            sound = scene.ISoundEngine.Play3D("Content\\sounds\\effects\\checkpoint.wav", body.Position.X, body.Position.Y, 0.0f,false,true);
            sound.Volume = 0.10f;
            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!isActive)
            {
                sound = scene.ISoundEngine.Play3D("Content\\sounds\\effects\\checkpoint.wav", body.Position.X, body.Position.Y, 0.0f, false, true);
                sound.Volume = 0.10f;
                sound.Paused = false;
                
                foreach (Element i in scene.Elements)
                {
                    Checkpoint checkpoint = i as Checkpoint;
                    if (checkpoint != null)
                        checkpoint.Active = false;
                }
                Active = true;
            }

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(isActive ? texture2 : texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            if(isActive)
                scene.SpriteBatch.Draw(shinyBallTexture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position - new Vector2(0,0.8f)), null, Color.White, body.Rotation, new Vector2(shinyBallTexture.Width/2, shinyBallTexture.Height/2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
