using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class Camera : DrawableGameComponent
    {
        public float Margin;
        public float Speed;
        public Element Target;
        public Vector2 Position;
        public Matrix View;
        public Matrix Projection;

        public Camera(Game game)
            : base(game)
        {
            Position = Vector2.Zero;
            Speed = Conversion.ToWorld(10);
            Margin = Conversion.ToWorld(50);
        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null)
            {
                Vector2 centeredPosition = Target.Position - new Vector2(Conversion.ToWorld(GraphicsDevice.PresentationParameters.BackBufferWidth / 2), Conversion.ToWorld(GraphicsDevice.PresentationParameters.BackBufferHeight / 2));
                float distance = Vector2.Distance(centeredPosition, Position);
                if (distance > Margin)
                {
                    Position = centeredPosition + Vector2.Normalize(Position - centeredPosition) * Margin;
                }
                else
                {
                    if (distance < Speed * (float)gameTime.ElapsedGameTime.TotalSeconds)
                    {
                        Position = centeredPosition;
                    }
                    else
                    {
                        Position += Vector2.Normalize(centeredPosition - Position) * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
            }

            View = Matrix.CreateLookAt(new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 1), new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 0), new Vector3(0, 1, 0));
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            //View = new Matrix(1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f);
            //Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, -GraphicsDevice.Viewport.Height, 0, 0, 1);

            base.Update(gameTime);
        }
    }
}
