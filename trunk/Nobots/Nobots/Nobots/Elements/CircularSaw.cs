using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Joints;
using IrrKlang;

namespace Nobots.Elements
{
    public class CircularSaw : Element, IActivable
    {
        public Body body;
        Texture2D texture;
        Texture2D texture2;
        bool isStartPosition = true;
        ISound sound;

        float angularVelocityTarget = 0;
        const float maxAngularVelocity = 3;
        const float startSpeed = 10;
        const float pulloverSpeed = 2;

        private bool isActive = false;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                if (isActive == true && value == false)
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.CircularSawStop, body.Position.X, body.Position.Y, 0.0f, false, false, false);
                isActive = value;
                if (!isActive)
                    body.LinearVelocity = Vector2.Zero;

                angularVelocityTarget = isActive ? maxAngularVelocity : 0.0f;
            }
        }

        Vector2 initialPosition, finalPosition;
        public Vector2 InitialPosition
        {
            get { return initialPosition; }
            set
            {
                initialPosition = value;
                createLine();
            }
        }

        public Vector2 FinalPosition
        {
            get { return finalPosition; }
            set
            {
                finalPosition = value;
                createLine();
            }
        }

        Vector2 linePosition;
        float lineRotation;
        float lineWidth;
        float lineHeight;
        void createLine()
        {
            linePosition = InitialPosition + (FinalPosition - InitialPosition) / 2;
            lineRotation = (float)Math.Atan2((FinalPosition - InitialPosition).Y, (FinalPosition - InitialPosition).X);
            lineWidth = Vector2.Distance(InitialPosition, FinalPosition);
            lineHeight = 0.08f;
        }

        public float Speed = 2f;
        
        private float height;
        public override float Height
        {
            get
            {
                return height;
            }
            set
            {
            }
        }

        private float width;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
            }
        }
        
        Vector2 position;
        public override Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
                position = value;
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
            }
        }

        public CircularSaw(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -1;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("circularsaw");
            texture2 = Game.Content.Load<Texture2D>("chain");
            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height);
            createBody();

            initialPosition = body.Position;
            finalPosition = body.Position - Vector2.UnitY * 0 + Vector2.UnitX * 5;
            createLine();

            sound = scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.CircularSaw, body.Position.X, body.Position.Y, 0.0f, true, true, false);
            sound.Volume = 0;
            sound.Paused = false;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body.UserData is Energy)
            {
                scene.GarbageElements.Add((Energy)fixtureB.Body.UserData);
                foreach (Element el in scene.Elements)
                    if (el is Character && !(el is Energy) && !(((Character)el).State is DyingCharacterState))
                    {
                        ((Character)el).State = new DyingCharacterState(scene, (Character)el);
                        break;
                    }
            }
            else if (fixtureB.Body.UserData is Character)
            {
                Character c = (Character)fixtureB.Body.UserData;
                if(!(c.State is DyingCharacterState))
                    c.State = new DyingCharacterState(scene, c);
            }
            else if (fixtureB.Body.UserData is IBreakable)
            {
                ((IBreakable)fixtureB.Body.UserData).Break();
            }
            return true;
        }

        double delay = 0;
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                delay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (delay <= 0)
                {
                    Vector2 targetPosition = isStartPosition ? finalPosition : initialPosition;
                    if (targetPosition != Position)
                    {
                        if (Vector2.DistanceSquared(targetPosition, Position) > Speed * Speed * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
                        {
                            Vector2 direction = Vector2.Normalize(targetPosition - Position);
                            body.LinearVelocity = Speed * direction;
                        }
                        else
                        {
                            body.LinearVelocity = Vector2.Zero;
                            Position = targetPosition;
                            isStartPosition = !isStartPosition;
                            delay = 3;
                        }
                    }
                    else
                    {
                        isStartPosition = !isStartPosition;
                    }
                }

                sound.Position = new Vector3D(body.Position.X, body.Position.Y, 0.0f);
            }

            if (angularVelocityTarget != body.AngularVelocity)
            {
                if (body.AngularVelocity > angularVelocityTarget)
                    body.AngularVelocity = Math.Max(0, body.AngularVelocity - pulloverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                else
                    body.AngularVelocity = Math.Min(maxAngularVelocity, body.AngularVelocity + startSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                sound.Volume = (body.AngularVelocity / maxAngularVelocity) * (body.AngularVelocity / maxAngularVelocity);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture2, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (linePosition.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (linePosition.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(lineWidth * scale)), (int)Math.Round(Conversion.ToDisplay(lineHeight * scale))), null, Color.White, lineRotation, new Vector2(texture2.Width / 2.0f, texture2.Height / 2.0f), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateCircle(scene.World, Height / 2.0f, 1.0f);
            body.Position = position;
            body.IsSensor = true;
            body.BodyType = BodyType.Kinematic;
            body.OnCollision += body_OnCollision;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
