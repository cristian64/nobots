using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Input;

namespace Nobots.Elements
{
    public class MovingPlatform : Element, IActivable
    {
        public Body body;
        Texture2D texture;
        Texture2D texture2;
        bool isStartPosition = true;

        private bool isActive = false;
        public bool Active
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                if (!isActive)
                    body.LinearVelocity = Vector2.Zero;
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
                height = value;
                createBody();
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
                width = value;
                createBody();
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
                body.Rotation = value;
            }
        }

        public MovingPlatform(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = -1;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("movingplatform");
            texture2 = Game.Content.Load<Texture2D>("movingplatform_line");
            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height);
            createBody();
            initialPosition = body.Position;
            finalPosition = body.Position - Vector2.UnitY * 3 + Vector2.UnitX * 5;
            createLine();
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
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture2, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (linePosition.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (linePosition.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(lineWidth * scale)), (int)Math.Round(Conversion.ToDisplay(lineHeight * scale))), null, Color.White, lineRotation, new Vector2(texture2.Width / 2.0f, texture2.Height / 2.0f), SpriteEffects.None, 0);
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(Width * scale)), (int)Math.Round(Conversion.ToDisplay(Height * scale))), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Kinematic;
            body.CollisionCategories = ElementCategory.FLOOR;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
