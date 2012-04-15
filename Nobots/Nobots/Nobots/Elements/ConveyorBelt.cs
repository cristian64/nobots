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
        public float Speed = 1;

        public override float Width
        {
            get
            {
                return Vector2.Distance(rotor1.Position, rotor2.Position) + rotor1.FixtureList[0].Shape.Radius * 2;
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return rotor2.FixtureList[0].Shape.Radius * 2;
            }
            set
            {
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
                return 0;
            }
            set
            {
            }
        }

        public ConveyorBelt(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0f;
            texture = Game.Content.Load<Texture2D>("closet");

            rotor1 = BodyFactory.CreateCircle(scene.World, 0.5f, float.MaxValue);
            rotor2 = BodyFactory.CreateCircle(scene.World, 0.5f, float.MaxValue);
            rotor3 = BodyFactory.CreateCircle(scene.World, 0.5f, float.MaxValue);

            rotor1.Position = position + new Vector2(-2.5f, 0);
            rotor2.Position = position;
            rotor3.Position = position + new Vector2(2.5f, 0);

            rotor1.BodyType = BodyType.Kinematic;
            rotor2.BodyType = BodyType.Kinematic;
            rotor3.BodyType = BodyType.Kinematic;

            rotor1.Friction = float.MaxValue;
            rotor2.Friction = float.MaxValue;
            rotor3.Friction = float.MaxValue;

            rotor1.AngularVelocity = Speed;
            rotor2.AngularVelocity = Speed;
            rotor3.AngularVelocity = Speed;

            createChain(scene.World, rotor1.Position + new Vector2(-2, -1.5f), rotor3.Position + new Vector2(2, -1.5f), 0.05f, 0.25f, 25, 5000.0f);
        }

        public override void Draw(GameTime gameTime)
        {
            /*scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), scene.Camera.Scale, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);*/
        }

        Path path;
        List<Body> chainLinks;
        List<RevoluteJoint> joints;
        private void createChain(World world, Vector2 start, Vector2 end, float linkWidth, float linkHeight, int numberOfLinks, float linkDensity)
        {
            //Chain start / end
            path = new Path();
            path.Add(start);
            path.Add(end);
            path.Add(new Vector2(end.X, end.Y + 2.0f));
            path.Add(new Vector2(start.X, start.Y + 2.0f));

            //A single chainlink
            PolygonShape shape = new PolygonShape(PolygonTools.CreateRectangle(linkWidth, linkHeight), linkDensity);

            //Use PathManager to create all the chainlinks based on the chainlink created before.
            chainLinks = PathManager.EvenlyDistributeShapesAlongPath(world, path, shape, BodyType.Dynamic, numberOfLinks);

            //Attach all the chainlinks together with a revolute joint
            joints = PathManager.AttachBodiesWithRevoluteJoint(world, chainLinks, new Vector2(0, -linkHeight), new Vector2(0, linkHeight), true, false);
        }
    }
}
