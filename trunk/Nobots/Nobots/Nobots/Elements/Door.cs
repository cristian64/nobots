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
                FinalPosition = body.Position - new Vector2(0, Height);
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
                InitialPosition = value;
                FinalPosition = value - new Vector2(0, Height);
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

        public Door(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 4f;
            this.position = position;
            texture = Game.Content.Load<Texture2D>("door");
            width = Conversion.ToWorld(texture.Width);
            height = Conversion.ToWorld(texture.Height/2);
            createBody();
            InitialPosition = body.Position;
            FinalPosition = body.Position - new Vector2(0, Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (body.Position.Y > FinalPosition.Y)
                    body.LinearVelocity = Speed * (-Vector2.UnitY);
                else
                    body.LinearVelocity = Vector2.Zero;
            } 
            else
            {
                if (body.Position.Y < InitialPosition.Y)
                    body.LinearVelocity = 10 * Speed * Vector2.UnitY;
                else
                    body.LinearVelocity = Vector2.Zero;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            scene.SpriteBatch.Draw(texture, new Rectangle((int)Conversion.ToDisplay(scale * (body.Position.X - scene.Camera.Position.X)), (int)Conversion.ToDisplay(scale * (body.Position.Y - scene.Camera.Position.Y)),
                (int)Conversion.ToDisplay(Width * scale), (int)Conversion.ToDisplay(Height * scale)), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.Friction = 0;
            body.BodyType = BodyType.Kinematic;
            body.CollisionCategories = ElementCategory.FLOOR;
            body.CollidesWith = ElementCategory.CHARACTER;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
