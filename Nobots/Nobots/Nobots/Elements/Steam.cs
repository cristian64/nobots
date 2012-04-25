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
using Nobots.ParticleSystem;

namespace Nobots.Elements
{
    public class Steam : Element, IActivable
    {
        public Body body;
        public Vector2 InitialPosition;
        public Vector2 FinalPosition;
        ExplosionSmokeParticleSystem explosionSmokeParticleSystem;
        public float Speed = 1f;
        float delay = 0f;
        Random random = new Random();

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

        public Steam(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 4f;
            this.position = position;
            height = 2.5f;
            width = 0.8f;
            createBody();
            InitialPosition = body.Position;
            FinalPosition = body.Position - new Vector2(0, Height);
        }

        float seconds = 0;
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                seconds +=(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > delay)
                {
                    seconds -= delay;
                    for (int i = 0; i < 8; i++)
                    {
                        scene.smoke.AddParticle(Position + new Vector2(0, height/2), -new Vector2(0,3));
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
        }

        private void createBody()
        {
            if(body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.Friction = 0;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollidesWith = ElementCategory.CHARACTER;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
