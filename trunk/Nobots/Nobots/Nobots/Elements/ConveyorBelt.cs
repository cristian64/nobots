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

namespace Nobots
{
    public class ConveyorBelt : Element
    {
        Texture2D texture;
        Body rotor1;
        Body rotor2;
        Body rotor3;

        private float radius = 0.5f;
        private float width = 5.0f;
        private float rotation = 0.0f;
        private int linksNumber = 25;
        private float linkWidth = 0.05f;
        private float linkHeight = 0.25f;
        private float angularSpeed = 1;

        List<Body> chainLinks;
        List<RevoluteJoint> joints;

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
                rotor1.AngularVelocity = rotor2.AngularVelocity = rotor3.AngularVelocity = angularSpeed;
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
                return rotor2.Position;
            }
            set
            {
                Vector2 difference = rotor2.Position;
                rotor2.Position = value;
                difference -= rotor2.Position;
                rotor1.Position -= difference;
                rotor3.Position -= difference;
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
            texture = Game.Content.Load<Texture2D>("closet");

            createBody(position);
        }

        public override void Draw(GameTime gameTime)
        {
            /*scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);*/
        }

        private void createBody(Vector2 position)
        {
            if (rotor1 != null)
                scene.World.RemoveBody(rotor1);
            if (rotor2 != null)
                scene.World.RemoveBody(rotor2);
            if (rotor3 != null)
                scene.World.RemoveBody(rotor3);
            if (chainLinks != null)
                foreach (Body i in chainLinks)
                    scene.World.RemoveBody(i);
            if (joints != null)
                foreach (Joint i in joints)
                    scene.World.RemoveJoint(i);
            rotor1 = rotor2 = rotor3 = null;
            chainLinks = null;
            joints = null;

            rotor1 = BodyFactory.CreateCircle(scene.World, radius, float.MaxValue);
            rotor2 = BodyFactory.CreateCircle(scene.World, radius, float.MaxValue);
            rotor3 = BodyFactory.CreateCircle(scene.World, radius, float.MaxValue);

            rotor1.Position = position + new Vector2(-width / 2, 0);
            rotor2.Position = position;
            rotor3.Position = position + new Vector2(width / 2, 0);

            rotor1.BodyType = rotor2.BodyType = rotor3.BodyType = BodyType.Kinematic;
            rotor1.Friction = rotor2.Friction = rotor3.Friction = float.MaxValue;
            rotor1.AngularVelocity = rotor2.AngularVelocity = rotor3.AngularVelocity = AngularSpeed;

            createChain(scene.World, rotor1.Position + new Vector2(-1, -1f), rotor3.Position + new Vector2(1, -1f), position, rotation, linkWidth, linkHeight, linksNumber, 5000.0f);

            rotor1.Position = RotateAboutOrigin(rotor1.Position, position, rotation);
            rotor3.Position = RotateAboutOrigin(rotor3.Position, position, rotation); 
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
    }
}
