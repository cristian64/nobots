﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using IrrKlang;

namespace Nobots.Elements
{
    public class Drop : Element
    {
        Body body;
        Texture2D texture;
        ISound sound;

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width);
            }
            set
            {
            }
        }

        private float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
               // createBody();
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

        public Drop(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            this.position = position;
            height = 6;
            dropPosition = position - new Vector2(0, height / 2);
            texture = Game.Content.Load<Texture2D>("drop");
            //createBody();
        }

        private void createBody()
        {
            if (body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Height, 100f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.UserData = this;
        }

        Vector2 dropPosition;
        float delay = 0;
        float seconds = 0;
        bool canDraw = true;
        float speed;
        public override void Update(GameTime gameTime)
        {
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds > delay)
            {
                seconds -= delay;

                if (dropPosition.Y < position.Y + height / 2 - Conversion.ToWorld(texture.Height/2))
                {
                    speed = 0.6f * seconds;
                    dropPosition += new Vector2(0, speed > 0.4f ? 0.4f : speed);
                    Console.WriteLine("Speed: " + 0.6f * seconds);
                    delay = 0;
                }
                else
                {
                    dropPosition = position - new Vector2(0, height / 2);
                    delay = 4;
                }

                canDraw = delay > 0 ? false : true;
            }
           
        }

        public override void Draw(GameTime gameTime)
        {
            if(canDraw)
                scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(dropPosition - scene.Camera.Position), null, Color.White, Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }
    }
}