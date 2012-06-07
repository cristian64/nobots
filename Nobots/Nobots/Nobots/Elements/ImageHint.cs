﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public class ImageHint : Element
    {
        public float Scale = 1;
        private Texture2D notexture;
        public Texture2D Texture;

        private String textureName;
        public String TextureName
        {
            get
            {
                return textureName;
            }
            set
            {
                textureName = value;
                try
                {
                    Texture = Game.Content.Load<Texture2D>("backgrounds\\" + value);
                }
                catch (Exception)
                {
                    Texture = notexture;
                }
                width = Conversion.ToWorld(Texture.Width);
                height = Conversion.ToWorld(Texture.Height);
            }
        }


        float width;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        private Vector2 position;
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

        private float rotation;
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

        public ImageHint(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 51;
            Texture = notexture = Game.Content.Load<Texture2D>("notexture");
            width = Conversion.ToWorld(Texture.Width);
            height = Conversion.ToWorld(Texture.Height);
            this.position = position;
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
                if (alpha > 0)
                    scene.SpriteBatch.Draw(Texture, scene.Camera.Scale * Conversion.ToDisplay(Position - scene.Camera.Position), null, Color.White * alpha, rotation, new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f), scene.Camera.Scale * Scale * new Vector2(width / Conversion.ToWorld(Texture.Width), height / Conversion.ToWorld(Texture.Height)), SpriteEffects.None, 0);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}