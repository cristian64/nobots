using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class Foreground : Element
    {
        public float Scale = 1;

        private String textureName;
        public String TextureName
        {
            get
            {
                return textureName;
            }
            set
            {
                textureName = value;
                try
                {
                    Texture = Game.Content.Load<Texture2D>("backgrounds\\" + value);
                }
                catch (Exception)
                {
                    Texture = notexture;
                }
                width = Conversion.ToWorld(Texture.Width);
                height = Conversion.ToWorld(Texture.Height);
            }
        }

        private Texture2D notexture;
        public Texture2D Texture;
        public Vector2 Speed = Vector2.One;
        private Vector2 position;

        float width;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        private float rotation;
        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public Foreground(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            Texture = notexture = Game.Content.Load<Texture2D>("notexture");
            width = Conversion.ToWorld(Texture.Width);
            height = Conversion.ToWorld(Texture.Height);
            Speed = Vector2.One;
            this.position = position;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(Texture, scene.Camera.Scale * Conversion.ToDisplay(Position - scene.Camera.Position + (Position - scene.Camera.Position) * (Speed - Vector2.One)), null, Color.White, rotation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), scene.Camera.Scale * Scale * new Vector2(width / Conversion.ToWorld(Texture.Width), height / Conversion.ToWorld(Texture.Height)), SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
