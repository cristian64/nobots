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
        Texture2D texture;
        public int ScreenHeight;
        public Vector2 Speed = Vector2.One;


        public override Vector2 Position
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

        public override float Rotation
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

        public Background(Game game, Scene scene)
            : base(game, scene)
        {
            Speed = new Vector2(0.3f, 0.05f);
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("background");
            ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            //Position = new Vector2(0, ScreenHeight);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(0 - Speed.X * scene.Camera.Position.X),
                ScreenHeight *8/7 + (int)Conversion.ToDisplay(0 - Speed.Y * scene.Camera.Position.Y), texture.Width, ScreenHeight * 2),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0.0f,
                new Vector2(0, texture.Height), SpriteEffects.None, 1);
            
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
