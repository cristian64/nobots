﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework.Input;

namespace Nobots
{
    public class Character : Element
    {
        Body body;
        Body torso;
        Texture2D texture;
        RevoluteJoint revoluteJoint;
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

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width/2), Conversion.ToWorld(texture.Height - texture.Width/8),
                1.0f);
            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 4.0f), 1.0f);

            body.Position = new Vector2(2.812996f, 2.083698f);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2),
                body.Position.Y + Conversion.ToWorld(texture.Width / 4 - texture.Height));

            torso.BodyType = BodyType.Dynamic;
            torso.Mass = 0;
            torso.FixedRotation = true;

            body.BodyType = BodyType.Dynamic;
            body.Friction = 1000.0f;

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(texture.Width/12, 
                texture.Height/(2.2f))), Vector2.Zero);
           scene.World.AddJoint(revoluteJoint);
           
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
            //scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, Effect, 0);
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(torso.Position - scene.Camera.Position), null, Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height * 5/11), 1.0f, Effect, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }

        KeyboardState previousState;
        private void processKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Left))
            {
                body.FixedRotation = false;
                Effect = SpriteEffects.FlipHorizontally;
                body.AngularVelocity = -20.0f;
            }
            else if (keybState.IsKeyDown(Keys.Right))
            {
                body.FixedRotation = false;
                Effect = SpriteEffects.None;
                body.AngularVelocity = 20.0f;
            }
            else
            {
                body.FixedRotation = true;
                body.AngularVelocity = 0.0f;
            }

            if (keybState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
            {
                body.ApplyForce(new Vector2(0, -75));
              //  torso.ApplyForce(new Vector2(0, -60));
            }

            previousState = keybState;
        }
    }
}
