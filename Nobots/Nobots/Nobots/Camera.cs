using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nobots
{
    public class Camera : DrawableGameComponent
    {
        public float Margin;
        public float Speed;
        public Element Target;
        public Vector2 Position;
        public Matrix View;
        public Matrix ViewNonScaled;
        public Matrix Projection;
        public float Scale = 0.5f;

        public Camera(Game game)
            : base(game)
        {
            Position = Vector2.Zero;
            Speed = Conversion.ToWorld(100);
            Margin = Conversion.ToWorld(50);
            Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null)
            {
                Vector2 centeredPosition = Target.Position - new Vector2(Conversion.ToWorld(GraphicsDevice.Viewport.Width / 2 / Scale), Conversion.ToWorld(GraphicsDevice.Viewport.Height / 1.5f / Scale));
                float distance = Vector2.Distance(centeredPosition, Position);
                float Speed = Scale * this.Speed;
                if (distance > Margin * Scale)
                {
                    Speed *= 3;
                    //Position = centeredPosition + Vector2.Normalize(Position - centeredPosition) * Margin;
                }
                /*else
                {*/
                    if (distance < Speed * (float)gameTime.ElapsedGameTime.TotalSeconds)
                    {
                        Position = centeredPosition;
                    }
                    else
                    {
                        Position += Vector2.Normalize(centeredPosition - Position) * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                //}
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    Position = Position + Conversion.ToWorld(-Vector2.UnitX * 15);
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                    Position = Position + Conversion.ToWorld(-Vector2.UnitY * 15);
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                    Position = Position + Conversion.ToWorld(Vector2.UnitX * 15);
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    Position = Position + Conversion.ToWorld(Vector2.UnitY * 15);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                Target = null;

            ViewNonScaled = Matrix.CreateLookAt(new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 1), new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 0), new Vector3(0, 1, 0));
            View = Matrix.CreateScale(Conversion.DisplayUnitsToWorldUnitsRatio) * ViewNonScaled;
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width / Scale, GraphicsDevice.Viewport.Height / Scale, 0, 0, 1);

            base.Update(gameTime);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Vector3 position = Game.GraphicsDevice.Viewport.Project(new Vector3(worldPosition.X, worldPosition.Y, 0), Projection, View, Matrix.Identity);
            return new Vector2(position.X, position.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Conversion.ToWorld(screenPosition) / Scale + Position;
        }

        public Vector2 ScreenToWorld(int x, int y)
        {
            return ScreenToWorld(new Vector2(x, y));
        }

        public Vector2 ScreenToWorld(MouseState mouseState)
        {
            return ScreenToWorld(mouseState.X, mouseState.Y);
        }
    }
}
