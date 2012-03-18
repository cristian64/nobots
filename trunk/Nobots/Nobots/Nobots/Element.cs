using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public abstract class Element : DrawableGameComponent
    {
        public abstract Vector2 Position
        {
            get; set;   
        }

        public abstract float Rotation
        {
            get;
            set;
        }

        protected Scene scene;

        public Element(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
        }
    }
}
