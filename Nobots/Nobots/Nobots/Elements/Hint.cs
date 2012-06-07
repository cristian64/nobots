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
        //Texture2D arrow;

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
            hintfont = Game.Content.Load<SpriteFont>("fonts\\hintfont");
            Text = "Write your comments here";
            this.position = position;
            //blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //blank.SetData(new[] { Color.White });
           // arrow = Game.Content.Load<Texture2D>("hintarrow");
        }

        float alpha = 0;
        public override void Draw(GameTime gameTime)
        {
            if (scene.Camera.Target != null)
            {
                if (Vector2.DistanceSquared(scene.Camera.Target.Position, Position) < 30)
                    alpha += alpha >= 1 ? 0 : (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    alpha -= alpha <= 0 ? 0 : (float)gameTime.ElapsedGameTime.TotalSeconds;
               // Console.WriteLine(alpha);
                if (alpha > 0)
                { 
                    //scene.SpriteBatch.Draw(blank, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - new Vector2((width + margin) / 2, height + margin / 2), null, Color.White, 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0);
                    /*scene.SpriteBatch.Draw(arrow, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White * alpha, 0, new Vector2(arrow.Width, arrow.Height), 1, SpriteEffects.None, 0);

                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitX, Color.Black * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitX, Color.Black * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), Color.Yellow * alpha, 0, new Vector2(width / 2, height + arrow.Height), 1, SpriteEffects.None, 0);
                    */
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitX, Color.Black * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitX, Color.Black * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) + Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position) - Vector2.UnitY, Color.Black * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);
                    scene.SpriteBatch.DrawString(hintfont, Text, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), Color.White * alpha, 0, new Vector2(width / 2, height), 1, SpriteEffects.None, 0);

                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
