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
    public class ElectricityBox : Element
    {
        Body body;
        Texture2D texture;
        public IActivable activableElement;

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

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(texture.Height);
            }
            set
            {
                throw new NotImplementedException();
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

        public ElectricityBox(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -6f;
            texture = Game.Content.Load<Texture2D>("electricitybox");
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 20f);
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
