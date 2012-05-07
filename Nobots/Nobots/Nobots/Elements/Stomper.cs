using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots.Elements
{
    public class Stomper : Element, IActivable
    {
        Body stomperBase;
        bool isMovingDown = true;
        public Body body;
        public float SpeedUp = 1;
        public float SpeedDown = 15;
        Texture2D texture;
        Texture2D texture2;

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
                createBody();
                createBaseBody();
            }
        }

        private float width = 2;
        public override float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                createBody();
                createBaseBody();
            }
        }
        
        Vector2 position;
        public override Vector2 Position
        {
            get
            {
                return stomperBase.Position;
            }
            set
            {
                stomperBase.Position = value;
                body.Position = stomperBase.Position;
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

        public Stomper(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 5f;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("stomper1");
            texture2 = Game.Content.Load<Texture2D>("stomper2");
            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height);
            createBody();
            createBaseBody();
        }

        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!fixtureB.IsSensor)
            {
                isMovingDown = false;
                if (fixtureB.Body.UserData is Character && body.LinearVelocity.Y > 0)
                {
                    if (!(((Character)fixtureB.Body.UserData).State is DyingCharacterState))
                    {
                        ((Character)fixtureB.Body.UserData).State = new DyingCharacterState(scene, (Character)fixtureB.Body.UserData);
                    }
                }
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive && scene.World.Enabled)
            {
                if (isMovingDown)
                {
                    body.LinearVelocity = SpeedDown * new Vector2(0, 1);
                    if (body.Position.Y - stomperBase.Position.Y > height * 0.9f)
                        isMovingDown = false;
                }
                else
                {
                    Vector2 targetPosition = stomperBase.Position;
                    if (targetPosition != body.Position)
                    {
                        if (Vector2.DistanceSquared(targetPosition, body.Position) > SpeedUp * SpeedUp * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
                        {
                            Vector2 direction = Vector2.Normalize(targetPosition - body.Position);
                            body.LinearVelocity = SpeedUp * direction;
                        }
                        else
                        {
                            body.LinearVelocity = Vector2.Zero;
                            body.Position = targetPosition;
                            scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.stomp, body.Position.X, body.Position.Y + height, 0f, false, false, false);
                            isMovingDown = true;
                        }
                    }
                    else
                    {
                        isMovingDown = true;
                        


                    }
                }
            }
            else
                body.LinearVelocity = Vector2.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture2, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(Width * scale)), (int)Math.Round(Conversion.ToDisplay(Height * scale))), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (stomperBase.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (stomperBase.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(Width * scale)), (int)Math.Round(Conversion.ToDisplay(height * scale))), null, Color.White, stomperBase.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position + new Vector2(0, 0);
            body.BodyType = BodyType.Kinematic;
            body.UserData = this;
            body.CollidesWith = ElementCategory.FLOOR | ElementCategory.CHARACTER;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        private void createBaseBody()
        {
            if (stomperBase != null)
                stomperBase.Dispose();
            stomperBase = BodyFactory.CreateRectangle(scene.World, Width, height, 1.0f);
            stomperBase.UserData = this;
            stomperBase.Position = position;
            stomperBase.BodyType = BodyType.Static;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            stomperBase.Dispose();
            base.Dispose(disposing);
        }
    }
}
