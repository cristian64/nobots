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
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    public class Character : Element
    {
        Body body;
        Body torso;
        public Texture2D texture;
        RevoluteJoint revoluteJoint;
        SliderJoint sliderJoint;
        public SpriteEffects Effect;
        bool touchingBox;
        Body touchedBox;
        public CharacterState State;

        public override Vector2 Position
        {
            get
            {
                return torso.Position + Vector2.UnitY * body.FixtureList[0].Shape.Radius;
            }
            set
            {
                torso.Position = value - Vector2.UnitY * body.FixtureList[0].Shape.Radius;
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

        public Character(Game game, Scene scene)
            : base(game, scene)
        {
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("girl");
            State = new IdleCharacterState(scene, this);

            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 2.0f), 1);
            body.Position = new Vector2(2.812996f, 2.083698f);
            body.BodyType = BodyType.Dynamic;
            body.Friction = float.MaxValue;
            body.Mass = 10000.0f;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height - texture.Width), 1.0f);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2), body.Position.Y + Conversion.ToWorld(texture.Width / 2 - texture.Height));
            torso.BodyType = BodyType.Dynamic;
            torso.Mass = 20;
            torso.FixedRotation = true;

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(0, texture.Height / 2)), Vector2.Zero);
            scene.World.AddJoint(revoluteJoint);

            base.LoadContent();
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body == touchedBox)
                touchingBox = false;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData as Box != null)
            {
                touchingBox = true;
                touchedBox = fixtureB.Body;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            State.Update();
            processKeyboard();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(Position - scene.Camera.Position),
                new Rectangle(State.textureXmin, State.textureYmin, State.characterWidth, State.characterHeight),
                Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, Effect, 0);
            //scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(Position - scene.Camera.Position), null, Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, Effect, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }

        KeyboardState previousState;
        private void processKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Left))
            {
                if(previousState.IsKeyUp(Keys.Left))
                    State = new RunningCharacterState(scene, this);

                Effect = SpriteEffects.FlipHorizontally;
                if (body.ContactList != null)
                {
                    body.FixedRotation = false;
                    body.AngularVelocity = -20.0f;
                }
                else
                {
                    body.LinearVelocity = new Vector2(-Math.Abs(body.LinearVelocity.X), body.LinearVelocity.Y);
                    torso.LinearVelocity = new Vector2(-Math.Abs(torso.LinearVelocity.X), torso.LinearVelocity.Y);
                }

                if (keybState.IsKeyDown(Keys.LeftControl) && touchingBox && !scene.World.JointList.Contains(sliderJoint))
                {
                    touchedBox.Friction = 0.0f;
                    sliderJoint = new SliderJoint(torso, touchedBox, Vector2.Zero, Vector2.Zero, 0, Conversion.ToWorld(texture.Width * 3 / 2));
                    sliderJoint.CollideConnected = true;
                    scene.World.AddJoint(sliderJoint);
                }
            }
            else if (keybState.IsKeyDown(Keys.Right))
            {
                if (previousState.IsKeyUp(Keys.Right))
                    State = new RunningCharacterState(scene, this);

                Effect = SpriteEffects.None;
                if (body.ContactList != null)
                {
                    body.FixedRotation = false;
                    body.AngularVelocity = 20.0f;
                }
                else
                {
                    body.LinearVelocity = new Vector2(Math.Abs(body.LinearVelocity.X), body.LinearVelocity.Y);
                    torso.LinearVelocity = new Vector2(Math.Abs(torso.LinearVelocity.X), torso.LinearVelocity.Y);
                }

                if (keybState.IsKeyDown(Keys.LeftControl) && touchingBox && !scene.World.JointList.Contains(sliderJoint))
                {
                    touchedBox.Friction = 0.0f;
                    sliderJoint = new SliderJoint(torso, touchedBox, Vector2.Zero, Vector2.Zero, 0, Conversion.ToWorld(texture.Width * 3 / 2));
                    sliderJoint.CollideConnected = true;
                    scene.World.AddJoint(sliderJoint);
                }
            }
            else
            {
                if (previousState.IsKeyDown(Keys.Right) || previousState.IsKeyDown(Keys.Left))
                    State = new IdleCharacterState(scene, this);
                body.FixedRotation = true;
                body.AngularVelocity = 0.0f;
            }

            if (keybState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
            {
                torso.ApplyForce(new Vector2(0, -130));
            }

            if (previousState.IsKeyDown(Keys.LeftControl) && keybState.IsKeyUp(Keys.LeftControl))
            {
                if (scene.World.JointList.Contains(sliderJoint))
                {
                    scene.World.RemoveJoint(sliderJoint);
                    touchedBox.Friction = 100.0f;
                }
            }

            previousState = keybState;
        }
    }
}
