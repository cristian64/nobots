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
        float stomperBaseHeight = 3;
        bool isMovingDown = true;
        public Body body;
        public float Speed = 0.8f;
        Texture2D texture;

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
                body.Position = stomperBase.Position + new Vector2(0, -stomperBaseHeight/2 + height / 2);
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
                body.Rotation = value;
            }
        }

        public Stomper(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 5f;
            this.position = position;
            height = stomperBaseHeight;
            texture = Game.Content.Load<Texture2D>("platform");
            createBody();
            createBaseBody();
        }


        protected bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            isMovingDown = false;
            return true;
        }

        protected void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (isMovingDown)
                    body.LinearVelocity += Speed * new Vector2(0, 1);
                else
                {
                    Vector2 targetPosition = stomperBase.Position + new Vector2(0, -stomperBaseHeight / 2 + height / 2);
                    if (targetPosition != body.Position)
                    {
                        if (Vector2.DistanceSquared(targetPosition, body.Position) > Speed * Speed * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
                        {
                            Vector2 direction = Vector2.Normalize(targetPosition - body.Position);
                            body.LinearVelocity = Speed * direction;
                        }
                        else
                        {
                            body.LinearVelocity = Vector2.Zero;
                            body.Position = targetPosition;
                            isMovingDown = true;
                        }
                    }
                    else
                        isMovingDown = true;
                }
            }
            else
                body.Position = stomperBase.Position + new Vector2(0, -stomperBaseHeight / 2 + height / 2);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)), (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)),
                (int)Conversion.ToDisplay(Width * scale), (int)Conversion.ToDisplay(Height * scale)), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (stomperBase.Position.X - scene.Camera.Position.X)), (int)Conversion.ToDisplay(scale * (stomperBase.Position.Y - scene.Camera.Position.Y)),
                (int)Conversion.ToDisplay(Width * scale), (int)Conversion.ToDisplay(stomperBaseHeight * scale)), null, Color.White, stomperBase.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);

        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            // body.Position = new Vector2(1.812996f, 3.583698f);
            body.Position = position + new Vector2(0,-stomperBaseHeight/2 + height/2);
            body.BodyType = BodyType.Kinematic;
            body.CollidesWith = ElementCategory.FLOOR | ElementCategory.CHARACTER;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            body.OnSeparation += new OnSeparationEventHandler(body_OnSeparation);
        }

        private void createBaseBody()
        {
            if (stomperBase != null)
                stomperBase.Dispose();
            stomperBase = BodyFactory.CreateRectangle(scene.World, Width, stomperBaseHeight, 1.0f);
            // body.Position = new Vector2(1.812996f, 3.583698f);
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
