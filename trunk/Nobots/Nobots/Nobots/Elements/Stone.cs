using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;

namespace Nobots.Elements
{
    public class Stone : Element, IPushable, IPullable
    {
        Body body;
        Texture2D texture;

        public override float Width
        {
            get
            {
                return Conversion.ToWorld(texture.Width);
            }
            set
            {
            }
        }

        public override float Height
        {
            get
            {
                return Conversion.ToWorld(texture.Height);
            }
            set
            {
            }
        }

        public override Vector2 Position
        {
            get
            {
                return body.Position + body.LocalCenter;
            }
            set
            {
                body.Position = value - body.LocalCenter;
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

        static Random random = new Random();

        public Stone(Game game, Scene scene, Vector2 position)
            : base(game, scene)
        {
            ZBuffer = 0.5f;
            switch (random.Next(9))
            {
                case 0: texture = Game.Content.Load<Texture2D>("stone"); break;
                case 1: texture = Game.Content.Load<Texture2D>("stone2"); break;
                case 2: texture = Game.Content.Load<Texture2D>("stone3"); break;
                case 3: texture = Game.Content.Load<Texture2D>("stone4"); break;
                case 4: texture = Game.Content.Load<Texture2D>("stone5"); break;
                case 5: texture = Game.Content.Load<Texture2D>("stone6"); break;
                case 6: texture = Game.Content.Load<Texture2D>("stone7"); break;
                case 7: texture = Game.Content.Load<Texture2D>("stone8"); break;
                case 8: texture = Game.Content.Load<Texture2D>("stone9"); break;
            }

            //Create an array to hold the data from the texture
            uint[] data = new uint[texture.Width * texture.Height];

            //Transfer the texture data to the array
            texture.GetData(data);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices textureVertices = PolygonTools.CreatePolygon(data, texture.Width, false);
            textureVertices.Translate(new Vector2(-texture.Width / 2, -texture.Height / 2));

            //We simplify the vertices found in the texture.
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            List<Vertices> list = BayazitDecomposer.ConvexPartition(textureVertices);

            //scale the vertices from graphics space to sim space
            Vector2 vertScale = new Vector2(Conversion.WorldUnitsToDisplayUnitsRatio);
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            //Create a single body with multiple fixtures
            body = BodyFactory.CreateCompoundPolygon(scene.World, list, 500f, BodyType.Dynamic);
            body.BodyType = BodyType.Dynamic;

            body.Position = position;
            body.Friction = 1000.0f;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Draw(texture, scene.Camera.Scale * Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scene.Camera.Scale * 1.05f, SpriteEffects.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
