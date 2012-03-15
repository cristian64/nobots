using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots
{
    class Background : Element
    {
        Texture2D texture;
        public int ScreenHeight;

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


        public Background(Game game, Scene scene)
            : base(game, scene)
        {
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
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(0 - scene.Camera.Position.X),
                ScreenHeight *8/7 + (int)Conversion.ToDisplay(0 - scene.Camera.Position.Y), texture.Width, ScreenHeight * 2),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0.0f,
                new Vector2(0, texture.Height), SpriteEffects.None, 1);
            
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
