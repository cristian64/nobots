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
            }
        }

        private Texture2D notexture;
        public Texture2D Texture;
        public Vector2 Speed = Vector2.One;
        private Vector2 position;

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(Texture.Width);
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(Texture.Height);
            }
            set
            {
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
            Speed = Vector2.One;
            this.position = position;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(Texture, scene.Camera.Scale * Conversion.ToDisplay(Position - Speed * scene.Camera.Position), null, Color.White, rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
