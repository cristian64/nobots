using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace Nobots
{
    public class Ladder : Element
    {
        private int stepsNumber;

        Body body;
        Texture2D texture;

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

        public Ladder(Game game, Scene scene, int stepsNumber)
            : base(game, scene)
        {
            this.stepsNumber = stepsNumber;
            texture = Game.Content.Load<Texture2D>("ladder");
            body = BodyFactory.CreateRectangle(scene.World, Conversion.ToWorld(texture.Width) * stepsNumber, Conversion.ToWorld(texture.Height), 20f);
            body.Position = new Vector2(5.812996f, 0.583698f);
            body.BodyType = BodyType.Static;

            body.UserData = this;
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
