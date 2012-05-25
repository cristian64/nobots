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
    public class TrainTrack : Element
    {
        public Body body;
        Texture2D texture;

        private int stepsNumber;
        public int StepsNumber
        {
            get
            {
                return stepsNumber;
            }
            set
            {
                stepsNumber = value;
                createBody();
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
                return width * stepsNumber;
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
                createBody();
            }
        }

        public TrainTrack(Game game, Scene scene, Vector2 position, int stepsNumber)
            : base(game, scene)
        {
            ZBuffer = 5f;
            this.position = position;
            this.stepsNumber = stepsNumber;
            texture = Game.Content.Load<Texture2D>("traintrack");
            height = Conversion.ToWorld(texture.Height);
            width = Conversion.ToWorld(texture.Width);
            createBody();
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = scene.Camera.Scale;
            Vector2 auxiliarPosition = body.Position - drawingShift;
            for (int i = 0; i < stepsNumber; i++)
            {
                scene.SpriteBatch.Draw(texture, scale * Conversion.ToDisplay(auxiliarPosition - scene.Camera.Position), null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
                auxiliarPosition += drawingIncrement;
            }
        }

        private Vector2 direction;
        private Vector2 drawingIncrement;
        private Vector2 drawingShift;

        private void createBody()
        {
            float previousRotation = 0;
            if (body != null)
            {
                previousRotation = body.Rotation;
                body.Dispose();
            }
            body = BodyFactory.CreateRectangle(scene.World, Width, Height, 1.0f);
            body.Position = position;
            body.BodyType = BodyType.Static;
            body.Rotation = previousRotation;
            body.Friction = 0.3f;
            body.CollisionCategories = ElementCategory.FLOOR;
            body.UserData = this;

            direction = new Vector2((float)Math.Cos(body.Rotation), (float)Math.Sin(body.Rotation));
            drawingIncrement = direction * Conversion.ToWorld(texture.Width);
            drawingShift = direction * (Width / 2) - drawingIncrement / 2.0f;
        }

        protected override void Dispose(bool disposing)
        {
            body.Dispose();
            base.Dispose(disposing);
        }
    }
}
