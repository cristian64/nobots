﻿using System;
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
    public class Forklift : Element, IActivable
    {
        Body body;
        Texture2D texture;
        bool isActive;
        Vector2 finalPosition;
        float speed;

        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public override float Width
        {
            get
            {
                return texture.Width;
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return texture.Height;
            }
            set
            {
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
                finalPosition = body.Position - new Vector2(0, 3f);
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

        public Forklift(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            isActive = false;
            ZBuffer = 0f;
            texture = Game.Content.Load<Texture2D>("forklift2");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(744), Conversion.ToWorld(texture.Height), 150f);
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
            body.Friction = 100.0f;
            body.Mass = 1000f;
            finalPosition = body.Position - new Vector2(0, 3f);
            speed = 0.01f;

            body.UserData = this;
        }

        KeyboardState prev;
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                body.BodyType = BodyType.Static;
                if (body.Position.Y > finalPosition.Y)
                    body.Position -= speed * Vector2.UnitY;
                else
                    isActive = false;
            }     
            prev = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
