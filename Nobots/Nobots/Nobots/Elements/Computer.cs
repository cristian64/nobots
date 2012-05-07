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
    public class Computer : Element
    {
        Body body;
        Texture2D texture;
        ISound sound;
        Vector3D pos = new Vector3D(0f, 0f, 0f);
        

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

                pos.X = value.X;
                pos.Y = value.Y;
                sound.Position = pos;
                
                
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

        public Computer(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("computer");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 100f);
            body.Position = position;
            body.IsSensor = true;
            body.BodyType = BodyType.Static;
            body.CollidesWith = ElementCategory.CHARACTER | ElementCategory.ENERGY;
            /*body.Rotation = -2.236696f;
            body.ApplyAngularImpulse(0.1f);*/
            body.Friction = 100.0f;

            sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Computer, body.Position.X, body.Position.Y, 0.0f, true, false, false);
            
                       

            
            
                 
            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            sound.Stop();
            sound.Dispose();
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
