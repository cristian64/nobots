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
        float height, width;

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
                    state.Exit();
                state = value;
                state.Enter();
                Console.WriteLine(state.GetType().Name);
            }
        }

        public override float Height
        {
            get { return height; }
            set { }
        }

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

        public Character(Game game, Scene scene)
            : base(game, scene)
        {
            texture = Game.Content.Load<Texture2D>("girl");
            ZBuffer = 0f;

            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height);

            body = BodyFactory.CreateCircle(scene.World, Conversion.ToWorld(texture.Width / 2f), 15);
            body.Position = new Vector2(1, 10.5f);
            //body.Position = new Vector2(34.60955f, 16f);
            body.BodyType = BodyType.Dynamic;
            body.Friction = float.MaxValue;
            body.UserData = this;
            body.CollisionCategories = ElementCategory.CHARACTER;
            body.CollidesWith = Category.All & ~ElementCategory.ENERGY;


            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);

            torso = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height - texture.Width + texture.Width / 2), 15);
            torso.Position = new Vector2(body.Position.X - Conversion.ToWorld(texture.Width / 2), body.Position.Y + Conversion.ToWorld(texture.Width / 2 - texture.Height));
            torso.BodyType = BodyType.Dynamic;
            torso.FixedRotation = true;
            torso.Friction = 0.0f;
            torso.UserData = this;
            torso.CollisionCategories = ElementCategory.CHARACTER;
            torso.CollidesWith = Category.All & ~ElementCategory.ENERGY;

            torso.OnCollision += new OnCollisionEventHandler(torso_OnCollision);
            torso.OnSeparation += new OnSeparationEventHandler(torso_OnSeparation);

            revoluteJoint = new RevoluteJoint(torso, body, Conversion.ToWorld(new Vector2(0, texture.Height / 2 - texture.Width / 4)), Vector2.Zero);
            scene.World.AddJoint(revoluteJoint);

            State = new IdleCharacterState(scene, this);
        }

        protected void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            contactsNumber--;
        }

        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            contactsNumber++;
            return true;
        }

        protected void torso_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body == touchedBox)
                touchingBox = false;
        }

        protected bool torso_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
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

        public bool IsTouchingElement(Element o)
        {
            return Math.Abs(Position.X - o.Position.X) <= 0.5 * (Width + o.Width) &&
                   Math.Abs(Position.Y - o.Position.Y) <= 0.5 * (Height + o.Height);
        }

        protected void updateLadder()
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
                Color.White, torso.Rotation, new Vector2(State.characterWidth/2, State.characterHeight / 2), 1.0f, Effect, 0);
            //scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(Position - scene.Camera.Position), null, Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, Effect, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
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
