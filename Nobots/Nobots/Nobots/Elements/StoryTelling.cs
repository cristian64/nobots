using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace Nobots.Elements
{
    public class StoryTelling : Element, IActivable
    {
        SpriteFont hintfont;
        Texture2D blank;

        Color border = Color.Gray;
        Color foreground = Color.Black;
        Color background = Color.White * 0.4f;
        float margin = 50;

        float alpha = 0;
        float alphaTarget = 1;
        private bool isActive = true;
        public bool Active
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                alphaTarget = isActive ? 1 : 0;
            }
        }

        private String text = "";
        public String Text
        {
            get { return text; }
            set
            {
                try
                {
                    width = hintfont.MeasureString(value).X;
                    height = hintfont.MeasureString(value).Y;
                    text = value;
                }
                catch (Exception)
                {
                }
            }
        }

        private float height = 1;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
            }
        }

        private float width = 1;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
            }
        }

        Vector2 position;
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

        public StoryTelling(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 50;
            hintfont = Game.Content.Load<SpriteFont>("fonts\\storytelling");
            Text = "LOREM IPSUM\n\nLorem ipsum dolor sit amet, consectetur adipisicing elit,\nsed do eiusmod tempor incididunt ut labore et dolore magna\naliqua. Ut enim ad minim veniam, quis nostrud exercitation\nullamco laboris nisi ut aliquip ex ea commodo consequat.\nDuis aute irure dolor in reprehenderit in voluptate\nvelit esse cillum dolore eu fugiat nulla pariatur.\n\nExcepteur sint occaecat cupidatat non proident, sunt in\nculpa qui officia deserunt mollit anim id est laborum.";
            this.position = position;
            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] colors = new Color[1];
            for (int i = 0; i < 1; i++)
                colors[i] = Color.White;
            blank.SetData(colors);
        }

        public override void Update(GameTime gameTime)
        {
            if (alpha != alphaTarget)
            {
                if (alpha < alphaTarget)
                    alpha = (float)Math.Min(1, alpha + gameTime.ElapsedGameTime.TotalSeconds / 1);
                else
                    alpha = (float)Math.Max(0, alpha - gameTime.ElapsedGameTime.TotalSeconds / 1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (alpha > 0)
            {
                float scale = scene.Camera.Scale;
                scene.SpriteBatch.Draw(blank, scale * Conversion.ToDisplay(position - scene.Camera.Position), null, background * alpha, 0, new Vector2(0.5f, 0.5f), new Vector2(width + margin, height + margin) * scale, SpriteEffects.None, 0);

                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitX, border * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitX, border * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitY, border * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, border * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, border * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
                scene.SpriteBatch.DrawString(hintfont, Text, scale * Conversion.ToDisplay(position - scene.Camera.Position), foreground * alpha, 0, new Vector2(width / 2, height / 2), scale, SpriteEffects.None, 0);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
