using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public abstract class Element : DrawableGameComponent
    {
        public float ZBuffer;

        public abstract Vector2 Position
        {
            get; set;   
        }

        public abstract float Rotation
        {
            get;
            set;
        }

        public abstract float Width
        {
            get;
            set;
        }

        public abstract float Height
        {
            get;
            set;
        }

        protected Scene scene;

        public Element(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            Initialize();
        }
    }
}
