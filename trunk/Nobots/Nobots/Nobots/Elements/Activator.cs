﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Nobots.Elements
{
    public class Activator : Element
    {
        protected IActivable activableElement;
        public IActivable ActivableElement
        {
            get 
            {
                if (activableElement == null)
                    foreach (Element e in scene.Elements)
                    {
                        if (e as IActivable != null && e.Id == activableElementId)
                        {
                            activableElement = (IActivable)e;
                            break;
                        }
                    }
                Debug.Assert(activableElement == null, "The element " + Id + " looked for " + activableElementId + " but that ID wasn't in the list.");
                return activableElement; 
            }
        }

        protected String activableElementId;
        public String ActivableElementId
        {
            get { return activableElementId; }
            set
            {
                activableElementId = value;
            }
        }

        public Activator(Game game, Scene scene) : base(game, scene)
        {
        }

        public override float Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Vector2 Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Width
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Rotation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
