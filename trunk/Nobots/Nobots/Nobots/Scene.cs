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
        private int screenWidth;

        public Scene(Game game)
            : base(game)
        {
            Camera = new Camera(Game);
            Elements = new List<Element>();
            World = new World(new Vector2(0, 9.81f));
        }

        public override void Initialize()
        {
            Elements.Add(new Background(Game, this));
            Elements.Add(new Box(Game, this));
            Elements.Add(new Character(Game, this));
            Camera.Target = Elements[2];

            Platform platform1 = new Platform(Game, this);
            Elements.Add(platform1);
            Platform platform2 = new Platform(Game, this);
            Elements.Add(platform2);
            Platform platform3 = new Platform(Game, this);
            Elements.Add(platform3);
            Platform platform4 = new Platform(Game, this);
            Elements.Add(platform4);
            Platform platform5 = new Platform(Game, this);
            Elements.Add(platform5);
            Platform platform6 = new Platform(Game, this);
            Elements.Add(platform6);
            Platform platform7 = new Platform(Game, this);
            Elements.Add(platform7);

            Camera.Initialize();
            foreach (Element i in Elements)
                i.Initialize();

            platform1.Position = new Vector2(1.85f, 2.5f);
            platform2.Position = new Vector2(3.8f, 3.75f);
            platform3.Position = new Vector2(5.0f, 5f);
            platform4.Position = new Vector2(9.7f, 4.4f);
            platform5.Position = new Vector2(13.8f, 3.5f);
            platform6.Position = new Vector2(17.8f, 3.25f);
            platform7.Position = new Vector2(21.8f, 3.25f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            BackgroundTexture = Game.Content.Load<Texture2D>("background");
            screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;

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
            
            base.Update(gameTime);
        }

    /*    private void drawBackground()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BackgroundTexture, new Rectangle(0, ScreenHeight, BackgroundTexture.Width, ScreenHeight * 2),
                new Rectangle(0,0, BackgroundTexture.Width, BackgroundTexture.Height), Color.White, 0.0f, 
                new Vector2(0, BackgroundTexture.Height), SpriteEffects.None, 1);
            SpriteBatch.End();       
        }*/
    }
}
