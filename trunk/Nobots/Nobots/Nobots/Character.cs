using System;
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

        public override int Height
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
            texture = Game.Content.Load<Texture2D>("girl");
            State = new IdleCharacterState(scene, this);

            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 2.0f), 30);
            body.Position = Conversion.ToWorld(new Vector2(-50, GraphicsDevice.PresentationParameters.BackBufferHeight));
            //body.Position = new Vector2(2.812996f, 2.083698f);
            body.BodyType = BodyType.Dynamic;
            body.Friction = float.MaxValue;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height - texture.Width), 30);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2), body.Position.Y + Conversion.ToWorld(texture.Width / 2 - texture.Height));
            torso.BodyType = BodyType.Dynamic;
            torso.FixedRotation = true;

            body.CollisionCategories = Category.Cat1;
            torso.CollisionCategories = Category.Cat1;

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(0, texture.Height / 2)), Vector2.Zero);
            scene.World.AddJoint(revoluteJoint);

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
            State.Update(gameTime);
            processKeyboard();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(Position - scene.Camera.Position),
                new Rectangle(State.textureXmin, State.textureYmin, State.characterWidth, State.characterHeight),
                Color.White, 0.0f, new Vector2(State.characterWidth/2, State.characterHeight / 2), 1.0f, Effect, 0);
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
                if (previousState.IsKeyUp(Keys.Left))
                    State = new RunningCharacterState(scene, this);

                torso.ApplyForce(new Vector2(-10.0f, 0));

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
                    sliderJoint = new SliderJoint(torso, touchedBox, Vector2.Zero, Vector2.Zero, 0, Vector2.Distance(torso.Position, touchedBox.Position));
                    sliderJoint.CollideConnected = true;
                    scene.World.AddJoint(sliderJoint);
                }
            }
            else if (keybState.IsKeyDown(Keys.Right))
            {
                if (previousState.IsKeyUp(Keys.Right))
                    State = new RunningCharacterState(scene, this);

                torso.ApplyForce(new Vector2(10.0f, 0));

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
                    sliderJoint = new SliderJoint(torso, touchedBox, Vector2.Zero, Vector2.Zero, 0, Vector2.Distance(torso.Position, touchedBox.Position));
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
                torso.LinearVelocity = new Vector2(0, torso.LinearVelocity.Y);
            }

            if (keybState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
            {
                // State = new JumpingCharacterState(scene, this);
                torso.ApplyForce(new Vector2(0, -4500));
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
