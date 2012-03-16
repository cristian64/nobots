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
            base.Update(gameTime);
        }
    }
}
