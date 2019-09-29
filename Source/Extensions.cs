using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PolyLightSim.Source
{
    public static class Extensions
    {
        public static Vector2 SafeNormalize(this Vector2 v)
        {
            float length = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (length == 0)
                return Vector2.Zero;
            return new Vector2(v.X / length, v.Y / length);
        }
        public static Vector2 RotatedBy(this Vector2 v, float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);

            return v;
        }
        public static float ToRotation(this Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
        public static Vector2 ToVector2(this float f)
        {
            return new Vector2((float)Math.Cos(f), (float)Math.Sin(f));
        }
        public static void DrawLine(this SpriteBatch sb, Color color, Vector2 start, Vector2 end, int thickness = 1)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line

            sb.Draw(PolyLightSim.Pixel,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X - thickness / 2,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    thickness), //width of line, change this to make thicker line
                null,
                color, //colour of line
                edge.ToRotation(),     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
