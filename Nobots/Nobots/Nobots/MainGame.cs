using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Nobots
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SplashLogo splashLogo;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += preparingDeviceSettings;
            Content.RootDirectory = "Content";
            Window.Title = "Synergy";

#if !FINAL_RELEASE
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 365);
            graphics.PreferredBackBufferHeight = (int)(0.7 * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            Window.AllowUserResizing = true;
#else
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
#endif
            graphics.PreferMultiSampling = true;
        }

        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        } 

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new Scene(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            splashLogo = new SplashLogo(this, spriteBatch);
        }

        bool splash = true;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (splash)
            {
                if (Keyboard.GetState().GetPressedKeys().Count() > 0)
                    splash = false;

                if (splashLogo.Enabled)
                    splashLogo.Update(gameTime);
                else
                    splash = false;
            }
            else
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (splash)
            {
                GraphicsDevice.Clear(Color.Black);
                if (splashLogo.Enabled)
                    splashLogo.Draw(gameTime);
            }
            else
            {
                base.Draw(gameTime);
            }
        }
    }

    class SplashLogo : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D logo;

        public SplashLogo(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            Initialize();
            this.spriteBatch = spriteBatch;
            logo = Game.Content.Load<Texture2D>("icons\\logo");
        }

        float alpha = 0;
        float alphaTarget = 0;
        float initialDelay = 1;
        static float duration = 5;
        float counter = duration;
        float transitionDuration = 1;

        public override void Update(GameTime gameTime)
        {
            if (alpha != alphaTarget)
            {
                if (alpha < alphaTarget)
                    alpha = (float)Math.Min(1, alpha + gameTime.ElapsedGameTime.TotalSeconds / transitionDuration);
                else
                    alpha = (float)Math.Max(0, alpha - gameTime.ElapsedGameTime.TotalSeconds / transitionDuration);
            }

            if (counter < duration - initialDelay)
                alphaTarget = 1;

            counter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (counter < transitionDuration)
                alphaTarget = 0;

            if (alpha == 0 && counter < 0)
                Enabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(logo, position, null, Color.White * alpha, 0, new Vector2(logo.Width / 2.0f, logo.Height / 2.0f), 0.4f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
