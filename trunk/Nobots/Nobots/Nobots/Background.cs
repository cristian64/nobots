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
        public Texture2D Texture;
        public Vector2 Speed = Vector2.One;
        private Vector2 position;

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

        public Background(Game game, Scene scene)
            : base(game, scene)
        {
            Speed = new Vector2(0.1f, 0.01f);
            Texture = Game.Content.Load<Texture2D>("background");
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            scene.SpriteBatch.Draw(Texture, Conversion.ToDisplay(Position - Speed * scene.Camera.Position), Color.White);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
