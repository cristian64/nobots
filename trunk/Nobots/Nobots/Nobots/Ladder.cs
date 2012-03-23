﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace Nobots
{
    public class Ladder : Element
    {
        private int stepsNumber;

        Body body;
        Texture2D texture;
        int currentElementPosition;

        public override int Height
        {
            get
            {
                return texture.Height * stepsNumber;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Width
        {
            get
            {
                return texture.Width;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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

        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
            }
        }

        public Ladder(Game game, Scene scene, int stepsNumber, Vector2 position)
            : base(game, scene)
        {
            this.stepsNumber = stepsNumber;
            texture = Game.Content.Load<Texture2D>("ladder");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height) * stepsNumber, 20f);
            body.Position = position;
            //body.Position = new Vector2(5.812996f, 0.583698f);
            body.BodyType = BodyType.Static;
            body.CollisionCategories = Category.Cat10;
            body.CollidesWith = Category.All & ~Category.Cat1;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            currentElementPosition = texture.Height / 2 * (stepsNumber -1);
            for (int i = 0; i < stepsNumber; i++)
            {
                scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position) + new Vector2(0, currentElementPosition),
                    null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
                currentElementPosition -= texture.Height;
            }
            scene.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
