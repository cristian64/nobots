using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics.Contacts;

namespace Nobots
{
    class Energy : Character
    {

        public Energy(Game game, Scene scene)
            : base(game, scene)
        {
        }

        protected void UpActionStart()
        {
        }

        protected void UpAction()
        {
        }

        protected void UpActionStop()
        {
        }

        protected void BActionStart()
        {
            State.BActionStart();
        }

        protected void BAction()
        {
            State.BAction();
        }

        protected void BActionStop()
        {
            State.BActionStop();
        }
    }
}
