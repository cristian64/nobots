using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots.Elements
{
    public abstract class Element : DrawableGameComponent, IComparable<Element>
    {
        public float ZBuffer;

        public int CompareTo(Element element)
        {
            return (int)(ZBuffer - element.ZBuffer);
        }

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
        public String Id;

        public Element(Game game, Scene scene)
            : base(game)
        {
            this.scene = scene;
            Initialize();
        }
    }
}
