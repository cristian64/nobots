﻿using System;
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
        public Matrix ViewNonScaled;
        public Matrix Projection;


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
                Vector2 centeredPosition = Target.Position - new Vector2(Conversion.ToWorld(GraphicsDevice.Viewport.Width / 2), Conversion.ToWorld(GraphicsDevice.Viewport.Height / 1.5f));
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

            ViewNonScaled = Matrix.CreateLookAt(new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 1), new Vector3(Conversion.ToDisplay(Position.X), Conversion.ToDisplay(Position.Y), 0), new Vector3(0, 1, 0));
            View = Matrix.CreateScale(Conversion.DisplayUnitsToWorldUnitsRatio) * ViewNonScaled;
            Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

            base.Update(gameTime);
        }
    }
}