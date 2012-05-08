using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Joints;
using System.Collections;

namespace Nobots.Elements
{
    public class ConveyorBelt : Element, IActivable
    {
        Texture2D circleTexture;
        Texture2D chainTexture;
        List<Body> rotors;
        List<Body> chainLinks;
        List<RevoluteJoint> joints;

        private int rotorsNumber = 3;
        private float radius = 0.5f;
        private float width = 5.0f;
        private float rotation = 0.0f;
        private int linksNumber = 25;
        private float linkWidth = 0.15f;
        private float linkHeight = 0.25f;
        private float angularSpeed = 1;

        public float LinkWidth
        {
            get { return linkWidth; }
            set
            {
                linkWidth = value;
                createBody(Position);
            }
        }

        public float LinkHeight
        {
            get { return linkHeight; }
            set
            {
                linkHeight = value;
                createBody(Position);
            }
        }

        public float AngularSpeed
        {
            get { return angularSpeed; }
            set
            {
                angularSpeed = value;
                if (isActive)
                    foreach (Body i in rotors)
                        i.AngularVelocity = angularSpeed;
            }
        }

        public int LinksNumber
        {
            get { return linksNumber; }
            set
            {
                linksNumber = value;
                createBody(Position);
            }
        }

        public int RotorsNumber
        {
            get { return rotorsNumber; }
            set
            {
                rotorsNumber = value;
                createBody(Position);
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

                if (value)
                    scene.SoundManager.ISoundEngine.Play3D(scene.SoundManager.powerUp[2], Position.X, Position.Y, 0f, false, false, false);
                foreach (Body i in rotors)
                    i.AngularVelocity = isActive ? angularSpeed : 0;
            }
        }

        public override float Width
        {
            get
            {
                return width + radius * 2;
            }
            set
            {
                width = value - radius * 2;
                createBody(Position);
            }
        }

        public override float Height
        {
            get
            {
                return radius * 2;
            }
            set
            {
                radius = value / 2;
                createBody(Position);
            }
        }

        public override Vector2 Position
        {
            get
            {
                return (rotors[0].Position + rotors[rotors.Count - 1].Position) / 2;
            }
            set
            {
                Vector2 difference = (rotors[0].Position + rotors[rotors.Count() - 1].Position) / 2 - value;
                foreach (Body i in rotors)
                    i.Position -= difference;
                foreach (Body i in chainLinks)
                    i.Position -= difference;
            }
        }

        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                createBody(Position);
            }
        }

        public ConveyorBelt(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            circleTexture = Game.Content.Load<Texture2D>("circle");
            chainTexture = Game.Content.Load<Texture2D>("chain");

            createBody(position);
        }

        public ConveyorBelt(Game game, Scene scene, Vector2 position, float? angularSpeed, int? linksNumber, int? rotorsNumber, float? linkWidth, float? linkHeight)
            : base(game, scene)
        {
            ZBuffer = 0f;
            circleTexture = Game.Content.Load<Texture2D>("circle");
            chainTexture = Game.Content.Load<Texture2D>("chain");

            if (angularSpeed != null)
                this.angularSpeed = (float)angularSpeed;
            if (linksNumber != null)
                this.linksNumber = (int)linksNumber;
            if (rotorsNumber != null)
                this.rotorsNumber = (int)rotorsNumber;
            if (linkWidth != null)
                this.linkWidth = (float)linkWidth;
            if (linkHeight != null)
                this.linkHeight = (float)linkHeight;

            createBody(position);
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            foreach (Body b in rotors)
                scene.SpriteBatch.Draw(circleTexture, scale * Conversion.ToDisplay(b.Position - scene.Camera.Position), null, Color.White, b.Rotation, new Vector2(circleTexture.Width / 2.0f, circleTexture.Height / 2.0f), scale * radius * 2, SpriteEffects.None, 0);
            foreach (Body b in chainLinks)
                scene.SpriteBatch.Draw(chainTexture, scale * Conversion.ToDisplay(b.Position - scene.Camera.Position), null, Color.White, b.Rotation, new Vector2(chainTexture.Width / 2.0f, chainTexture.Height / 2.0f), scale * (Vector2.UnitX * linkWidth + Vector2.UnitY * linkHeight) * 2, SpriteEffects.None, 0);
        }

        private void createBody(Vector2 position)
        {
            if (rotors != null)
                foreach (Body i in rotors)
                    scene.World.RemoveBody(i);
            if (chainLinks != null)
                foreach (Body i in chainLinks)
                    scene.World.RemoveBody(i);
            if (joints != null)
                foreach (Joint i in joints)
                    scene.World.RemoveJoint(i);
            rotors = null;
            chainLinks = null;
            joints = null;

            rotors = new List<Body>(rotorsNumber);
            for (int i = 0; i < rotorsNumber; i++)
            {
                Body rotor = BodyFactory.CreateCircle(scene.World, radius, float.MaxValue);
                rotor.BodyType = BodyType.Kinematic;
                rotor.Friction = float.MaxValue;
                if (isActive)
                    rotor.AngularVelocity = AngularSpeed;
                rotor.Position = position + new Vector2(-width / 2 + i * (width / (rotorsNumber - 1)), 0);
                rotors.Add(rotor);
            }

            createChain(scene.World, rotors[0].Position + new Vector2(-1, -1f), rotors.Last<Body>().Position + new Vector2(1, -1f), position, rotation, linkWidth, linkHeight, linksNumber, 1000.0f);

            foreach (Body i in chainLinks)
                i.CollisionCategories = ElementCategory.FLOOR;

            foreach (Body i in rotors)
                i.Position = RotateAboutOrigin(i.Position, position, rotation);
        }

        private void createChain(World world, Vector2 start, Vector2 end, Vector2 origin, float rotation, float linkWidth, float linkHeight, int numberOfLinks, float linkDensity)
        {
            //Chain start / end
            Path path = new Path();
            path.Add(RotateAboutOrigin(start, origin, rotation));
            path.Add(RotateAboutOrigin(end, origin, rotation));
            path.Add(RotateAboutOrigin(new Vector2(end.X, end.Y + 2.0f), origin, rotation));
            path.Add(RotateAboutOrigin(new Vector2(start.X, start.Y + 2.0f), origin, rotation));
            path.Add(RotateAboutOrigin(start, origin, rotation));

            //A single chainlink
            PolygonShape shape = new PolygonShape(PolygonTools.CreateRectangle(linkWidth, linkHeight), linkDensity);

            //Use PathManager to create all the chainlinks based on the chainlink created before.
            chainLinks = PathManager.EvenlyDistributeShapesAlongPath(world, path, shape, BodyType.Dynamic, numberOfLinks);

            //Attach all the chainlinks together with a revolute joint
            joints = PathManager.AttachBodiesWithRevoluteJoint(world, chainLinks, new Vector2(0, -linkHeight), new Vector2(0, linkHeight), true, false);
        }

        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        protected override void Dispose(bool disposing)
        {
            foreach (Body i in rotors)
                i.Dispose();
            foreach (Body i in chainLinks)
                i.Dispose();
            foreach (Joint i in joints)
                scene.World.RemoveJoint(i);
            base.Dispose(disposing);
        }
    }
}
