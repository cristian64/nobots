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

namespace Nobots.Elements
{
    public class Character : Element, IControllable, IActivable
    {
        public int contactsNumber = 0;
        public Body body;
        public Body torso;
        public Texture2D texture;
        public RevoluteJoint revoluteJoint;
        public SpriteEffects Effect;

        public Body touchedBody;
        public Body lastContact;

        public Ladder Ladder;
        public Ladder LastLadder;

        protected CharacterState state;
        public CharacterState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != null)
                    state.Exit(value);
                state = value;
                Console.WriteLine(state.GetType().Name);
                state.Enter();
            }
        }

        private bool isActive = true;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }

        float height;
        public override float Height
        {
            get { return height; }
            set { }
        }

        float width;
        public override float Width
        {
            get { return width; }
            set { }
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
                body.Position = value - Vector2.UnitY * body.FixtureList[0].Shape.Radius / 2 + Vector2.UnitY * height / 2;
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

        public Character(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            texture = Game.Content.Load<Texture2D>("girl");
            ZBuffer = 0f;

            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height);

            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 2f), 40);
            body.Position = position - Vector2.UnitY * body.FixtureList[0].Shape.Radius / 2 + Vector2.UnitY * height / 2;
            body.BodyType = BodyType.Dynamic;
            body.Friction = float.MaxValue;
            body.UserData = this;
            body.CollisionCategories = ElementCategory.CHARACTER;
            body.CollidesWith = Category.All & ~ElementCategory.ENERGY;


            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height - texture.Width + texture.Width / 2), 40);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2), body.Position.Y + Conversion.ToWorld(texture.Width / 2 - texture.Height));
            torso.Position = position - Vector2.UnitY * body.FixtureList[0].Shape.Radius / 2;
            torso.BodyType = BodyType.Dynamic;
            torso.FixedRotation = true;
            torso.Friction = 0.0f;
            torso.UserData = this;
            torso.CollisionCategories = ElementCategory.CHARACTER;
            torso.CollidesWith = Category.All & ~ElementCategory.ENERGY;

            torso.OnCollision += new OnCollisionEventHandler(torso_OnCollision);
            torso.OnSeparation += new OnSeparationEventHandler(torso_OnSeparation);

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(0, texture.Height / 2 - texture.Width / 4)), Vector2.Zero);
            revoluteJoint.CollideConnected = false;
            scene.World.AddJoint(revoluteJoint);

            State = new IdleCharacterState(scene, this);
        }

        float countToFall = 0;

        protected void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (!fixtureB.IsSensor)
            {
                contactsNumber--;
                if (fixtureB.Body == lastContact)
                    lastContact = null;
            }
        }

        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!fixtureB.IsSensor)
            {
                contactsNumber++;
                countToFall = 0;
                lastContact = fixtureB.Body;
            }
            return true;
        }

        protected void torso_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body == touchedBody)
            {
                touchedBody = null;
                Console.WriteLine("untouched!");
            }
        }

        protected bool torso_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (touchedBody == null && (fixtureB.Body.UserData as IPullable != null || fixtureB.Body.UserData as IPushable != null))
            {
                Console.WriteLine("changed touched body!");
                touchedBody = fixtureB.Body;
            }

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (contactsNumber == 0)
            {
                countToFall += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countToFall > 0.1f && !(state is FallingCharacterState) && !(state is JumpingCharacterState) && !(state is ComaCharacterState) && !(state is DyingCharacterState) && !(state is ClimbingCharacterState) && GraphicsDevice != null) //TODO: same thing in onseparation with GraphicsDevice, since it's creating a new object after being disposed.
                    State = new FallingCharacterState(scene, this);
            }
            updateLadder();
            State.Update(gameTime);
        }

        public bool IsTouchingElement(Element o)
        {
            return Math.Abs(Position.X - o.Position.X) <= 0.5 * (Width + o.Width) &&
                   Math.Abs(Position.Y - o.Position.Y) <= 0.5 * (Height + o.Height);
        }

        protected void updateLadder()
        {
            if (Ladder != null && !IsLadderInRange(Ladder))
            {
                Ladder = null;
                LastLadder = null;
            }

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

        public bool IsLadderInRange(Ladder ladder)
        {
            Vector2 headPosition = torso.Position;

            if (Math.Abs(headPosition.X - ladder.Position.X) < Conversion.ToWorld(40))
            {
                if (ladder.Position.Y - ladder.Height / 2 <= headPosition.Y - Height / 2 + 0.5f && headPosition.Y - 0.5f <= ladder.Position.Y + ladder.Height / 2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsLadderInRangeToGoUp(Ladder ladder)
        {
            Vector2 headPosition = torso.Position;
            if (Math.Abs(headPosition.X - ladder.Position.X) < Conversion.ToWorld(40))
                if (ladder.Position.Y - ladder.Height / 2 <= headPosition.Y - Height / 2)
                    return true;
            return false;
        }

        public bool IsLadderInRangeToGoDown(Ladder ladder)
        {
            Vector2 headPosition = torso.Position;
            if (Math.Abs(headPosition.X - ladder.Position.X) < Conversion.ToWorld(40))
                if (headPosition.Y <= ladder.Position.Y + ladder.Height / 2)
                    return true;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (scene.Camera.Target == this)
                scene.Camera.Target = null;
            if (scene.InputManager.Character == this)
                scene.InputManager.Character = null;
            if (scene.SelectionManager.Selection == this)
                scene.SelectionManager.Selection = null;
            scene.World.RemoveJoint(revoluteJoint);
            torso.Dispose();
            body.Dispose();
            base.Dispose(disposing);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * (Conversion.ToDisplay(Position - scene.Camera.Position) - Vector2.UnitY * 10), // this -10 is to make the character a bit up, not to be below the floor
                new Rectangle(State.textureXmin, State.textureYmin, State.characterWidth, State.characterHeight),
                Color.White, torso.Rotation, new Vector2(State.characterWidth / 2.0f, State.characterHeight / 2.0f), scene.Camera.Scale, Effect, 0);
        }

        public virtual void AActionStart()
        {
            State.AActionStart();
        }

        public virtual void AAction()
        {
            State.AAction();
        }

        public virtual void AActionStop()
        {
            State.AActionStop();
        }

        public virtual void BActionStart()
        {
            foreach (Element i in scene.Elements)
            {
                Activator activator = i as Activator;
                if (activator != null && !activator.EnergyElement && IsTouchingElement(i))
                {
                    activator.Activate();
                }
            }

            State.BActionStart();
        }

        public virtual void BAction()
        {
            State.BAction();
        }

        public virtual void BActionStop()
        {
            State.BActionStop();
        }

        public virtual void XActionStart()
        {
            State.XActionStart();
        }

        public virtual void XAction()
        {
            State.XAction();
        }

        public virtual void XActionStop()
        {
            State.XActionStop();
        }

        public virtual void YActionStart()
        {
            State.YActionStart();
        }

        public virtual void YAction()
        {
            State.YAction();
        }

        public virtual void YActionStop()
        {
            State.YActionStop();
        }

        public virtual void RightActionStart()
        {
            State.RightActionStart();
        }

        public virtual void RightAction()
        {
            State.RightAction();
        }

        public virtual void RightActionStop()
        {
            State.RightActionStop();
        }

        public virtual void LeftActionStart()
        {
            State.LeftActionStart();
        }

        public virtual void LeftAction()
        {
            State.LeftAction();
        }

        public virtual void LeftActionStop()
        {
            State.LeftActionStop();
        }

        public virtual void UpActionStart()
        {
            State.UpActionStart();
        }

        public virtual void UpAction()
        {
            State.UpAction();
        }

        public virtual void UpActionStop()
        {
            State.UpActionStop();
        }

        public virtual void DownActionStart()
        {
            State.DownActionStart();
        }

        public virtual void DownAction()
        {
            State.DownAction();
        }

        public virtual void DownActionStop()
        {
            State.DownActionStop();
        }
    }
}
