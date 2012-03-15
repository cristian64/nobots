﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace Nobots
{
    public class Character : Element
    {
        Body body;
        Texture2D texture;
        public SpriteEffects Effect;

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

        public Character(Game game, Scene scene)
            : base(game, scene)
        {
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("girl");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 1.0f);
            body.Position = new Vector2(2.812996f, 2.083698f);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
         //   body.Mass = 50.0f;
            body.Friction = 10000.0f;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            processKeyboard();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, Effect, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }

        KeyboardState previousState;
        private void processKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();

            if (keybState.IsKeyDown(Keys.Left))
            {
                body.LinearVelocity = new Vector2(-9, body.LinearVelocity.Y);
                Effect = SpriteEffects.FlipHorizontally;
               /* body.FixedRotation = false;
                body.AngularVelocity = -20.0f;*/
            }
            else if (keybState.IsKeyDown(Keys.Right))
            {
                body.LinearVelocity = new Vector2(9, body.LinearVelocity.Y);
                Effect = SpriteEffects.None;
               /* body.FixedRotation = false;
                body.AngularVelocity = 20.0f;*/
            }
            
            if (keybState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
            {
                body.ApplyForce(new Vector2(0, -100));
            }
            

            previousState = keybState;
        }
    }
}
