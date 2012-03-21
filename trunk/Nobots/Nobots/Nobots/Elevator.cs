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

namespace Nobots
{
    public class Elevator : Element
    {
        public bool Active = false;

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public Vector2 InitialPosition;
        public Vector2 FinalPosition;
        public float Speed = 0.5f;

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

        public Elevator(Game game, Scene scene)
            : base(game, scene)
        {
            Active = true;
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("elevator");
            body = BodyFactory.CreateEdge(scene.World, Vector2.Zero, new Vector2(Conversion.ToWorld(texture.Width), 0));
            EdgeShape ceiling = new EdgeShape(new Vector2(0, Conversion.ToWorld(texture.Height)), new Vector2(Conversion.ToWorld(texture.Width), Conversion.ToWorld(texture.Height)));
            body.CreateFixture(ceiling);

            body.Position = new Vector2(-2.5f, 1.583698f);
            body.BodyType = BodyType.Kinematic;

            InitialPosition = body.Position;
            FinalPosition = body.Position - Vector2.UnitY * 3;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 targetPosition = Active ? FinalPosition : InitialPosition;
            if (Vector2.DistanceSquared(targetPosition, Position) > Speed * Speed * gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds)
            {
                Vector2 direction = Vector2.Normalize(targetPosition - Position);
                body.LinearVelocity = Speed * direction;
            }
            else
            {
                Position = targetPosition;
            }
            if (targetPosition == Position)
                Active = !Active;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            scene.SpriteBatch.Begin();
            scene.SpriteBatch.Draw(texture, Conversion.ToDisplay(body.Position - scene.Camera.Position), null, Color.White, body.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            scene.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
