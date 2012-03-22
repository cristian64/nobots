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
            uint[] data = new uint[texture.Width * texture.Height];
            texture.GetData(data);
            Vertices verts = PolygonTools.CreatePolygon(data, texture.Width, false);
            Vector2 scale = new Vector2(Conversion.WorldUnitsToDisplayUnitsRatio, Conversion.WorldUnitsToDisplayUnitsRatio);

            verts.Scale(ref scale);
            
            List<Fixture> compound = FixtureFactory.AttachCompoundPolygon(BayazitDecomposer.ConvexPartition(verts), 1, body);

            compound[0].Body.BodyType = BodyType.Dynamic;
           // body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height), 150f);
            body.Position = new Vector2(7.812996f, 0.583698f);
            body.BodyType = BodyType.Dynamic;
            body.Friction = 5.0f;
            body.Mass = 10;

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
