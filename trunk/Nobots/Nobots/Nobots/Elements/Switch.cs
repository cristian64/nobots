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
    public class Switch : Activator
    {
        Body body;
        Texture2D texture;

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

        public Switch(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("switch_off");
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 20f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = Category.None | ElementCategory.CHARACTER;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.UserData = this;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if(ActivableElement != null)
                texture = ActivableElement.Active ? Game.Content.Load<Texture2D>("switch_on") : Game.Content.Load<Texture2D>("switch_off");
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }

        public override void Activate()
        {
            base.Activate();

            if(ActivableElement != null)
                if(ActivableElement.Active)
                scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.SwitchOff, body.Position.X, body.Position.Y, 0f, false, false, false);
                 else
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.SwitchOn, body.Position.X, body.Position.Y, 0f, false, false, false);
        }
    }
}
