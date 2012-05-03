using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    public class Transitioner : DrawableGameComponent
    {
        private Scene scene;
        private Texture2D blank;

        private float duration = 0.5f;
        private Color color = Color.Black;
        private float alpha = 1;
        public float AlphaTarget = 0;

        public Transitioner(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            Initialize();
            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            if (alpha != AlphaTarget)
            {
                if (alpha < AlphaTarget)
                    alpha = (float)Math.Min(1, alpha + gameTime.ElapsedGameTime.TotalSeconds / duration);
                else
                    alpha = (float)Math.Max(0, alpha - gameTime.ElapsedGameTime.TotalSeconds / duration);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (alpha > 0)
            {
                scene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                scene.SpriteBatch.Draw(blank, GraphicsDevice.Viewport.Bounds, color * alpha);
                scene.SpriteBatch.End();
            }
        }

        public bool InTransition
        {
            get { return alpha != AlphaTarget; }
        }
    }
}
