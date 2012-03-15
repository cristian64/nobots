using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    public class Scene : DrawableGameComponent
    {
        public SpriteBatch SpriteBatch;
        public Camera Camera;
        public World World;
        public List<Element> Elements;
        public Texture2D BackgroundTexture;
        public int ScreenHeight;

        public Scene(Game game)
            : base(game)
        {
            Camera = new Camera(Game);
            Elements = new List<Element>();
            World = new World(new Vector2(0, 9.81f));
        }

        public override void Initialize()
        {
            Elements.Add(new Box(Game, this));
            Elements.Add(new Character(Game, this));

            foreach (Element i in Elements)
                i.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            BackgroundTexture = Game.Content.Load<Texture2D>("background");
            ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update(gameTime);
            foreach (Element i in Elements)
                i.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Element i in Elements)
                i.Draw(gameTime);
            drawBackground();
            base.Update(gameTime);
        }

        private void drawBackground()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BackgroundTexture, new Rectangle(0, ScreenHeight, BackgroundTexture.Width, ScreenHeight * 2),
                new Rectangle(0,0, BackgroundTexture.Width, BackgroundTexture.Height), Color.White, 0.0f, 
                new Vector2(0, BackgroundTexture.Height), SpriteEffects.None, 1);
            SpriteBatch.End();       
        }
    }
}
