using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace Nobots
{
    public class Platform : Element
    {
        public Body body;
        Texture2D texture;
        Vector2 position;

        private float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                createBody();
            }
        }

        private float width;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                createBody();
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
                position = value;
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

        public Platform(Game game, Scene scene, Vector2 position, Vector2 size)
            : base(game, scene)
        {
            ZBuffer = 5f;
            height = size.Y;
            width = size.X;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("platform");
            createBody();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, new Rectangle( (int)Conversion.ToDisplay(body.Position.X - scene.Camera.Position.X), (int)Conversion.ToDisplay(body.Position.Y - scene.Camera.Position.Y), 
                (int)Conversion.ToDisplay(Width), (int)Conversion.ToDisplay(Height)), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            // body.Position = new Vector2(1.812996f, 3.583698f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.CollisionCategories = Category.Cat11;
        }
    }
}
