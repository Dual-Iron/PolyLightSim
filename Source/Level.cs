using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PolyLightSim.Source
{
    public class Level
    {
        public List<Shape> Bounds = new List<Shape>();

        public Level(List<Shape> bounds)
        {
            Bounds = bounds;
        }

        public Level(Player player, int polygonCount, int pointsPerPoly)
        {
            for (int ct = 0; ct < polygonCount; ct++)
            {
                List<Vector2> points = new List<Vector2>();

                int x = PolyLightSim.random.Next((int)player.ScreenSize.X);
                int yMin = (int)MathHelper.Lerp(0, player.ScreenSize.Y, ct / (float)polygonCount);
                int yMax = yMin + (int)(player.ScreenSize.Y / polygonCount);

                for (int i = 0; i < pointsPerPoly; i++)
                {
                    points.Add(new Vector2(
                        PolyLightSim.random.Next(x - 50, x + 50),
                        PolyLightSim.random.Next(yMin, yMax)
                        ));
                }

                List<Vector2> corners = new List<Vector2>()
                {
                    new Vector2(0, 0),
                    new Vector2(player.ScreenSize.X, 0),
                    player.ScreenSize,
                    new Vector2(0, player.ScreenSize.Y),
                };

                Bounds.Add(new Shape(points));
                Bounds.Add(new Shape(corners));
            }
        }

        public List<Segment> GetSegments()
        {
            var segments = new List<Segment>();
            foreach (var poly in Bounds)
            {
                segments.AddRange(poly.GetSegments());
            }
            return segments;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var poly in Bounds)
            {
                poly.Draw(sb);
            }
        }
    }
}
