using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;

namespace Nobots
{
    public class Circuit : Element
    {
        Body body1;
        Body body2;
        Texture2D texture;

        public override Vector2 Position
        {
            get
            {
                return body1.Position;
            }
            set
            {
                body1.Position = value;
            }
        }

        public Vector2 StartPosition
        {
            get
            {
                return body1.Position;
            }
            set
            {
                body1.Position = value;
            }
        }

        public Vector2 EndPosition
        {
            get
            {
                return body1.Position;
            }
            set
            {
                body1.Position = value;
            }
        }

        public override float Rotation
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override float Width
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Circuit(Game game, Scene scene, Vector2 startPosition, Vector2 endPosition)
            : base(game, scene)
        {
            texture = Game.Content.Load<Texture2D>("socket");
            body1 = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 20f);
            body1.Position = startPosition;
            body1.BodyType = BodyType.Static;
            body1.CollisionCategories = Category.None;
            body1.CollidesWith = Category.None;

            body2 = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 20f);
            body2.Position = endPosition;
            body2.BodyType = BodyType.Static;
            body2.CollisionCategories = Category.None;
            body2.CollidesWith = Category.None;

            body1.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body1.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body2.Position - scene.Camera.Position), null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
