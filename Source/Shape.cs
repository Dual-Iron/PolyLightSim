using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PolyLightSim.Source
{
    public class Shape
    {
        public List<Vector2> vertices = new List<Vector2>();

        public Shape(List<Vector2> vertices)
        {
            this.vertices = vertices;
        }

        public IEnumerable<Segment> GetSegments()
        {
            for (int current = 0; current < vertices.Count; current++)
            {
                int next = (current + 1) % vertices.Count;
                yield return new Segment(vertices[current], vertices[next]);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var seg in GetSegments())
            {
                seg.Draw(sb);
            }
        }
    }
}
