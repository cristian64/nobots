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
    public class CameraScale : Element
    {
        public Body body;
        public float ScaleTarget = Camera.DefaultScale;

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

        float rotation;
        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
            }
        }

        public CameraScale(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            height = 1;
            width = 1;
            rotation = MathHelper.PiOver4;
            this.position = position;
            createBody();
        }

        private void createBody()
        {
            if (body != null)
                body.Dispose();
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.Rotation = rotation;
            body.CollidesWith = ElementCategory.ENERGY | ElementCategory.CHARACTER;
            body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            scene.Camera.ScaleTarget = ScaleTarget;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
