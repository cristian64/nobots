using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class Camera : DrawableGameComponent
    {
        public Element Target;
        public Vector2 Position;
        public float Zoom;

        public Camera(Game game)
            : base(game)
        {
            Position = Vector2.Zero;
            Zoom = 100;
        }

        public override void Update(GameTime gameTime)
        {
            if (Target != null)
            {
                Position = Target.Position;
                Position -= new Vector2(Conversion.ToWorld(GraphicsDevice.PresentationParameters.BackBufferWidth / 2), Conversion.ToWorld(GraphicsDevice.PresentationParameters.BackBufferHeight / 2));
            }
            base.Update(gameTime);
        }
    }
}
