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
        public int contactsNumber = 0;
        public Body body;
        public Body torso;
        public Texture2D texture;
        public RevoluteJoint revoluteJoint;
        public SpriteEffects Effect;

        public bool touchingBox;
        public Body touchedBox;
        public float touchedBoxMass;
        public float touchedBoxFriction;
        public SliderJoint sliderJoint;

        public Ladder Ladder;

        private CharacterState state;
        public CharacterState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != null)
                    state.Exit();
                state = value;
                state.Enter();
                Console.WriteLine(state.GetType().Name);
            }
        }

        public override float Height
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

        public override float Width
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
                return torso.Position + Vector2.UnitY * body.FixtureList[0].Shape.Radius / 2;
            }
            set
            {
                torso.Position = value - Vector2.UnitY * body.FixtureList[0].Shape.Radius / 2;
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
            ZBuffer = 0f;

            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 2f), 30);
           // body.Position = new Vector2(1f, 0);
            body.Position = new Vector2(34.60955f, -2.803332f);
            body.BodyType = BodyType.Dynamic;
            body.Friction = float.MaxValue;
            //body.UserData = this;

            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height - texture.Width + texture.Width / 2), 30);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2), body.Position.Y + Conversion.ToWorld(texture.Width / 2 - texture.Height));
            torso.BodyType = BodyType.Dynamic;
            torso.FixedRotation = true;
            torso.Friction = 0.0f;
            torso.UserData = this;

            torso.OnCollision += new OnCollisionEventHandler(torso_OnCollision);
            torso.OnSeparation += new OnSeparationEventHandler(torso_OnSeparation);

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(0, texture.Height / 2 - texture.Width / 4)), Vector2.Zero);
            scene.World.AddJoint(revoluteJoint);

            State = new IdleCharacterState(scene, this);
        }

        void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            contactsNumber--;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            contactsNumber++;
            return true;
        }

        void torso_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body == touchedBox)
                touchingBox = false;
        }

        bool torso_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData as Box != null || fixtureB.Body.UserData as Stone != null)
            {
                touchingBox = true;
                touchedBox = fixtureB.Body;
            }

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            updateLadder();
            processKeyboard();
            State.Update(gameTime);
            base.Update(gameTime);
        }

        public bool IsLadderInRange(Ladder ladder)
        {
            Vector2 headPosition = torso.Position;

            if (Math.Abs(headPosition.X - ladder.Position.X) < Conversion.ToWorld(15))
            {
                if (ladder.Position.Y - ladder.Height / 2 <= headPosition.Y && headPosition.Y <= ladder.Position.Y + ladder.Height / 2)
                {
                    return true;
                }
            }
            return false;
        }

        void updateLadder()
        {
            if (Ladder != null && !IsLadderInRange(Ladder))
                Ladder = null;

            if (Ladder == null)
            {
                foreach (Element i in scene.Elements)
                {
                    if (i as Ladder != null && IsLadderInRange((Ladder)i))
                    {
                        Ladder = (Ladder)i;
                        break;
                    }
                }
            }
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

        private void AActionStart()
        {
            State.AActionStart();
        }

        private void AAction()
        {
            State.AAction();
        }

        private void AActionStop()
        {
            State.AActionStop();
        }

        private void BActionStart()
        {
            State.BActionStart();
        }

        private void BAction()
        {
            State.BAction();
        }

        private void BActionStop()
        {
            State.BActionStop();
        }

        private void XActionStart()
        {
            State.XActionStart();
        }

        private void XAction()
        {
            State.XAction();
        }

        private void XActionStop()
        {
            State.XActionStop();
        }

        private void YActionStart()
        {
            State.YActionStart();
        }

        private void YAction()
        {
            State.YAction();
        }

        private void YActionStop()
        {
            State.YActionStop();
        }

        private void RightActionStart()
        {
            State.RightActionStart();
        }

        private void RightAction()
        {
            State.RightAction();
        }

        private void RightActionStop()
        {
            State.RightActionStop();
        }

        private void LeftActionStart()
        {
            State.LeftActionStart();
        }

        private void LeftAction()
        {
            State.LeftAction();
        }

        private void LeftActionStop()
        {
            State.LeftActionStop();
        }

        private void UpActionStart()
        {
            State.UpActionStart();
        }

        private void UpAction()
        {
            State.UpAction();
        }

        private void UpActionStop()
        {
            State.UpActionStop();
        }

        private void DownActionStart()
        {
            State.DownActionStart();
        }

        private void DownAction()
        {
            State.DownAction();
        }

        private void DownActionStop()
        {
            State.DownActionStop();
        }

        KeyboardState previousKeyboardState;
        GamePadState previosGamepadState;
        private void processKeyboard()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if ((currentGamepadState.Buttons.A == ButtonState.Pressed && previosGamepadState.Buttons.A == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A)))
            {
                AActionStart();
            }
            if (((currentGamepadState.Buttons.B == ButtonState.Pressed && previosGamepadState.Buttons.B == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.B) && previousKeyboardState.IsKeyUp(Keys.B))) && touchingBox && !scene.World.JointList.Contains(sliderJoint))
            {
                BActionStart();
            }
            if ((currentGamepadState.Buttons.X == ButtonState.Pressed && previosGamepadState.Buttons.X == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.X) && previousKeyboardState.IsKeyUp(Keys.X)))
            {
                XActionStart();
            }
            if ((currentGamepadState.Buttons.Y == ButtonState.Pressed && previosGamepadState.Buttons.Y == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y)))
            {
                YActionStart();
            }
            if ((currentGamepadState.DPad.Left == ButtonState.Pressed && previosGamepadState.DPad.Left == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)))
            {
                LeftActionStart();
            }
            if ((currentGamepadState.DPad.Right == ButtonState.Pressed && previosGamepadState.DPad.Right == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)))
            {
                RightActionStart();
            }
            if ((currentGamepadState.DPad.Up == ButtonState.Pressed && previosGamepadState.DPad.Up == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
            {
                UpActionStart();
            }
            if ((currentGamepadState.DPad.Down == ButtonState.Pressed && previosGamepadState.DPad.Down == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down)))
            {
                DownActionStart();
            }
            if ((currentGamepadState.Buttons.A == ButtonState.Released && previosGamepadState.Buttons.A == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.A) && previousKeyboardState.IsKeyDown(Keys.A)))
            {
                AActionStop();
            }
            if (((currentGamepadState.Buttons.B == ButtonState.Released && previosGamepadState.Buttons.B == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.B) && previousKeyboardState.IsKeyDown(Keys.B))) && scene.World.JointList.Contains(sliderJoint))
            {
                BActionStop();
            }
            if ((currentGamepadState.Buttons.X == ButtonState.Released && previosGamepadState.Buttons.X == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.X) && previousKeyboardState.IsKeyDown(Keys.X)))
            {
                XActionStop();
            }
            if ((currentGamepadState.Buttons.Y == ButtonState.Released && previosGamepadState.Buttons.Y == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.Y) && previousKeyboardState.IsKeyDown(Keys.Y)))
            {
                YActionStop();
            }
            if ((currentGamepadState.DPad.Left == ButtonState.Released && previosGamepadState.DPad.Left == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left)))
            {
                LeftActionStop();
            }
            if ((currentGamepadState.DPad.Right == ButtonState.Released && previosGamepadState.DPad.Right == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right)))
            {
                RightActionStop();
            }
            if ((currentGamepadState.DPad.Up == ButtonState.Released && previosGamepadState.DPad.Up == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.Up) && previousKeyboardState.IsKeyDown(Keys.Up)))
            {
                UpActionStop();
            }
            if ((currentGamepadState.DPad.Down == ButtonState.Released && previosGamepadState.DPad.Down == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyUp(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down)))
            {
                DownActionStop();
            }
            if ((currentGamepadState.Buttons.A == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.A)))
            {
                AAction();
            }
            if (((currentGamepadState.Buttons.B == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.B))) && scene.World.JointList.Contains(sliderJoint))
            {
                BAction();
            }
            if ((currentGamepadState.Buttons.X == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.X)))
            {
                XAction();
            }
            if ((currentGamepadState.Buttons.Y == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Y)))
            {
                YAction();
            }
            if ((currentGamepadState.DPad.Left == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Left)))
            {
                LeftAction();
            }
            if ((currentGamepadState.DPad.Right == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Right)))
            {
                RightAction();
            }
            if ((currentGamepadState.DPad.Up == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Up)))
            {
                UpAction();
            }
            if ((currentGamepadState.DPad.Down == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Down)))
            {
                DownAction();
            }

            previousKeyboardState = currentKeyboardState;
            previosGamepadState = currentGamepadState;
        }
    }
}
