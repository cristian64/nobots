using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class Camera : GameComponent
    {
        public Vector2 Position;
        public float Zoom;

        public Camera(Game game)
            : base(game)
        {
            Position = Vector2.Zero;
            Zoom = 50;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
