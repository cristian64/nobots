using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace Nobots.Elements
{
    public class Door : Element, IActivable
    {
        public Body body;
        Texture2D texture;
        public Vector2 InitialPosition;
        public Vector2 FinalPosition;
        public float Speed = 1f;

        bool playSound = false;

        private bool isActive = false;
        public bool Active
        {
            get
            {
                return isActive;
            }
            set
            {
                if (isActive != value)
                    playSound = true;
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
                UpdatePositions();
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
                UpdatePositions();
            }
        }

        float rotation = 0;
        public override float Rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = rotation = value;
            }
        }

        public Door(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 4f;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("door");
            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height/2);
            createBody();
            Position = position;
            UpdatePositions();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (body.Position.Y > FinalPosition.Y)
                    body.LinearVelocity = Speed * (-Vector2.UnitY);
                else
                {
                    body.LinearVelocity = Vector2.Zero;
                    body.Position = FinalPosition;
                }
            } 
            else
            {
                if (body.Position.Y < InitialPosition.Y)
                    body.LinearVelocity = 10 * Speed * Vector2.UnitY;
                else
                {
                    if (playSound)
                    {
                        scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.Door, body.Position.X, body.Position.Y, 0.0f, false, false, false);
                        playSound = false;
                    }
                    body.LinearVelocity = Vector2.Zero;
                    body.Position = InitialPosition;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Math.Round(Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X))), (int)Math.Round(Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y))),
                (int)Math.Round(Conversion.ToDisplay(Width * scale)), (int)Math.Round(Conversion.ToDisplay(Height * scale))), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0);
        }

        public void UpdatePositions()
        {
            if (!isActive)
            {
                InitialPosition = body.Position;
                FinalPosition = body.Position - new Vector2(0, height);
            }
            else
            {
                FinalPosition = body.Position;
                InitialPosition = body.Position + new Vector2(0, height);
            }
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.Rotation = rotation;
            body.Friction = 0;
            body.BodyType = BodyType.Kinematic;
            body.CollisionCategories = ElementCategory.FLOOR;
            body.CollidesWith = Category.All & ~ElementCategory.FLOOR;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
