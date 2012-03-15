﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Nobots
{
    public class Box : Element
    {
        Body body;
        Texture2D texture;

        public override Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
            }
        }

        public Box(Game game, Scene scene)
            : base(game, scene)
        {
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("wooden-box");
            body = BodyFactory.CreateRectangle(scene.World, 0.01f * texture.Width, 0.01f * texture.Height, 1.0f);
            body.Position = new Vector2(5.812996f, 4.583698f);
            body.BodyType = BodyType.Dynamic;
            body.Rotation = -2.236696f;
            body.Mass = 50.0f;
            body.Friction = 100.0f;

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, 50.0f * body.Position, null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
