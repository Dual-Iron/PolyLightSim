using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace PolyLightSim.Source
{
    public struct Ray
    {
        private Color clr;
        private Vector2 dir;
        private Vector2 pos;
        private float mag;

        public Ray(Color color, Vector2 position, Vector2 direction, float magnitude)
        {
            clr = color;
            mag = magnitude;
            pos = position;

            dir = direction;
            dir.Normalize();
        }

        public Vector2 GetEndPos(List<Segment> toCheck)
        {
            var colliding = new List<Vector2>();
            foreach (var seg in toCheck)
            {
                Vector2? intersect = IntersectWithSegment(seg);
                if (intersect != null)
                    colliding.Add((Vector2)intersect);
            }

            if (colliding.Count == 0)
            {
                return pos + dir * mag;
            }

            Vector2 thisPos = this.pos;
            var closestsOrdered = colliding.OrderBy(pt => (pt - thisPos).Length());
            return closestsOrdered.First();
        }

        /// <see cref="https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection"/>
        private Vector2? IntersectWithSegment(Segment segment)
        {
            float x1 = segment.A.X,
                    y1 = segment.A.Y,
                    x2 = segment.B.X,
                    y2 = segment.B.Y;

            float x3 = pos.X,
                    y3 = pos.Y,
                    x4 = pos.X + dir.X,
                    y4 = pos.Y + dir.Y;

            float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (den == 0) return null;
            float num = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
            float t = num / den;

            num = (x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3);
            float u = -num / den;

            if (t < 1 && t > 0 && u > 0)
            {
                Vector2 intersect = new Vector2
                {
                    X = x1 + t * (x2 - x1),
                    Y = y1 + t * (y2 - y1)
                };
                return intersect;
            }

            return null;
        }

        public void Draw(SpriteBatch sb, List<Segment> check)
        {
            Vector2 start = pos;
            Vector2 end = GetEndPos(check);

            sb.DrawLine(clr * 0.6f, start, end, 1);
        }
    }
}
