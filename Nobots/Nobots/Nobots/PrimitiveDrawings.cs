using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nobots
{
    public class PrimitiveDrawings
    {
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D blank, Vector2 point1, Vector2 point2, Color color, float thickness = 1)
        {
           float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
           float length = Vector2.Distance(point1, point2);
 
           spriteBatch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        public static void DrawBoundingBox(SpriteBatch spriteBatch, Texture2D blank, Vector2 center, float width, float height, float rotation, Color color, float thickness = 1)
        {
            Vector2 vertex1 = RotateAboutOrigin(center + new Vector2(-width / 2, -height / 2), center, rotation);
            Vector2 vertex2 = RotateAboutOrigin(center + new Vector2(width / 2, -height / 2), center, rotation);
            Vector2 vertex3 = RotateAboutOrigin(center + new Vector2(width / 2, height / 2), center, rotation);
            Vector2 vertex4 = RotateAboutOrigin(center + new Vector2(-width / 2, height / 2), center, rotation);

            DrawLine(spriteBatch, blank, vertex1, vertex2, color, thickness);
            DrawLine(spriteBatch, blank, vertex2, vertex3, color, thickness);
            DrawLine(spriteBatch, blank, vertex3, vertex4, color, thickness);
            DrawLine(spriteBatch, blank, vertex4, vertex1, color, thickness);
        }

        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }
    }
}
