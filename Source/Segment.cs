using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PolyLightSim.Source
{
    public struct Segment
    {
        public Vector2 A;
        public Vector2 B;

        public Segment(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public void Draw(SpriteBatch sb) => sb.DrawLine(Color.Gray, A, B, 2);
    }
}
