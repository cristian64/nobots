using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class Background : Element
    {
        public String TextureName
        {
            get
            {
                return (Texture != null) ? Texture.Name : "";
            }
            set
            {
                Texture = Game.Content.Load<Texture2D>(value);
            }
        }

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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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

        public Background(Game game, Scene scene, Texture2D texture)
            : base(game, scene)
        {
            Speed = Vector2.One;
            Texture = texture;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            scene.SpriteBatch.Draw(Texture, scene.Camera.Scale * Conversion.ToDisplay(Position - Speed * scene.Camera.Position), null, Color.White, 0, Vector2.Zero, scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
