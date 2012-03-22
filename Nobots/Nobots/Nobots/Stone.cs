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

namespace Nobots
{
    class Stone : Element
    {
        Body body;
        Texture2D texture;

        public override int Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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

        public Stone(Game game, Scene scene)
            : base(game, scene)
        {
            texture = Game.Content.Load<Texture2D>("stone");

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

            body.Position = new Vector2(7.812996f, 0f);
            body.Friction = 1000.0f;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
