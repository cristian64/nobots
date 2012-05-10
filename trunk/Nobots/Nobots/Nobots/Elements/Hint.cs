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
    public class Hint : Element
    {
        SpriteFont hintfont;
        //Texture2D blank;
        Texture2D arrow;

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

        public Hint(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 50;
            hintfont = Game.Content.Load<SpriteFont>("hintfont");
            Text = "Write here your hint";
            this.position = position;
            //blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //blank.SetData(new[] { Color.White });
            arrow = Game.Content.Load<Texture2D>("hintarrow");
        }

        public override void Draw(GameTime gameTime)
        {
            //scene.SpriteBatch.Draw(blank, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - new Vector2((width + margin) / 2, height + margin / 2), null, Color.White, 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(arrow, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, 0, new Vector2(arrow.Width, arrow.Height), 1, SpriteEffects.None, 0);

            scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitX, Color.Black, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
            scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitX, Color.Black, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
            scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitY, Color.Black, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
            scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, Color.Black, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
            scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), Color.Yellow, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
