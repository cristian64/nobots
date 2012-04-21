using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class Alarm : Element, IActivable
    {
        Texture2D texture;
        Texture2D textureOn;
        float alpha = 0;
        bool increasing = true;

        private bool isActive = false;
        public bool Active
        {
            get 
            {
                return isActive;
            }

            set
            {
                if (isActive != value)
                {
                    isActive = value;
                   // texture = isActive ? scene.Game.Content.Load<Texture2D>("alarmon") : scene.Game.Content.Load<Texture2D>("alarm");
                }
            }
        }

        public override float Width
        {
            get { return Conversion.ToWorld(texture.Width); }
            set { }
        }

        public override float Height
        {
            get { return Conversion.ToWorld(texture.Height); }
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

        float rotation = 0;
        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public Alarm(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -7f;
            this.position = position;
            texture = scene.Game.Content.Load<Texture2D>("alarm");
            textureOn = scene.Game.Content.Load<Texture2D>("alarmon");
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (increasing && alpha < 1)
                    alpha += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (!increasing && alpha > 0)
                    alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (increasing && alpha >= 1)
                    increasing = false;
                else
                    increasing = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            if (isActive)
                scene.SpriteBatch.Draw(textureOn, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White * alpha, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
        }
    }
}
