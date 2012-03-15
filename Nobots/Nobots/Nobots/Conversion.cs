using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nobots
{
    public class Conversion
    {
        public static float DisplayUnitsToWorldUnitsRatio = 100.0f;
        public static float WorldUnitsToDisplayUnitsRatio = 1 / DisplayUnitsToWorldUnitsRatio;

        public static float ToDisplay(float u)
        {
            return u * DisplayUnitsToWorldUnitsRatio;
        }

        public static float ToWorld(float u)
        {
            return u * WorldUnitsToDisplayUnitsRatio;
        }

        public static Vector2 ToDisplay(Vector2 u)
        {
            return u * DisplayUnitsToWorldUnitsRatio;
        }

        public static Vector2 ToWorld(Vector2 u)
        {
            return u * WorldUnitsToDisplayUnitsRatio;
        }
    }
}
