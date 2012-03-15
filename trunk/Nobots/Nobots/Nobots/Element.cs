using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public interface Element : DrawableGameComponent
    {
        public virtual Vector2 Position;
    }
}
