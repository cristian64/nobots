using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class Lamp : Element, IActivable
    {       
        Texture2D texture;

        private bool isActive = true;
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
                    texture = isActive ? scene.Game.Content.Load<Texture2D>("lamp_on") : scene.Game.Content.Load<Texture2D>("lamp_off");
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

        public Lamp(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 10f;
            this.position = position;
            texture = scene.Game.Content.Load<Texture2D>("lamp_on");
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(position - scene.Camera.Position), null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0); 
        }
    }
}
