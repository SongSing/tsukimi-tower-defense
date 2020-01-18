using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class Utils
    {
        public static Vector2f Vi2f(Vector2i v)
        {
            return new Vector2f(v.X, v.Y);
        }
        public static Vector2u Vi2u(Vector2i v)
        {
            return new Vector2u((uint)v.X, (uint)v.Y);
        }

        public static Vector2f Vu2f(Vector2u v)
        {
            return new Vector2f(v.X, v.Y);
        }

        public static Vector2i Vf2i(Vector2f v)
        {
            return new Vector2i((int)v.X, (int)v.Y);
        }

        public static Vector2f RoundV(Vector2f v)
        {
            return new Vector2f((float)Math.Round(v.X), (float)Math.Round(v.Y));
        }

        public static Vector2f Grid2f(Vector2i gridPos)
        {
            return Vi2f(gridPos) * Game.TileSize + new Vector2f(Game.TileSize, Game.TileSize) / 2;
        }

        public static Vector2f Snap2Grid(Vector2i pos)
        {
            return Grid2f(new Vector2i(pos.X / Game.TileSize, pos.Y / Game.TileSize));
        }

        public static Vector2i Vf2Grid(Vector2f v)
        {
            return new Vector2i((int)v.X / Game.TileSize, (int)v.Y / Game.TileSize);
        }

        public static Vector2f[] Laser2Points(Vector2f startPos, Vector2f endPos, float width, float angle)
        {
            float rightAngle = angle - (float)Math.PI / 2;
            float leftAngle = angle + (float)Math.PI / 2;

            Vector2f bottomLeft = startPos + new Vector2f((float)Math.Cos(leftAngle), (float)Math.Sin(leftAngle)) * width / 2;
            Vector2f bottomRight = startPos + new Vector2f((float)Math.Cos(rightAngle), (float)Math.Sin(rightAngle)) * width / 2;
            Vector2f topLeft = endPos + new Vector2f((float)Math.Cos(leftAngle), (float)Math.Sin(leftAngle)) * width / 2;
            Vector2f topRight = endPos + new Vector2f((float)Math.Cos(rightAngle), (float)Math.Sin(rightAngle)) * width / 2;

            return new Vector2f[] { topRight, topLeft, bottomLeft, bottomRight };
        }

        public static float DistanceToLine(Vector2f pt, Vector2f lineStart, Vector2f lineEnd)
        {
            float num = (float)Math.Abs((lineEnd.Y - lineStart.Y) * pt.X - (lineEnd.X - lineStart.X) * pt.Y + lineEnd.X * lineStart.Y - lineEnd.Y * lineStart.X);
            float den = (float)Math.Sqrt(Math.Pow(lineEnd.Y - lineStart.Y, 2) + Math.Pow(lineEnd.X - lineStart.X, 2));

            return num / den;
        }

        public static bool LaserContainsHitbox(Vector2f startPos, Vector2f endPos, float width, float angle, Vector2f hitboxPos, float hitboxRadius)
        {
            var points = Laser2Points(startPos, endPos, width, angle);

            if (PointInRectangle(hitboxPos, points))
            {
                Console.WriteLine("point in rect");
                return true;
            }
            else if (DistanceToLine(hitboxPos, points[0], points[1]) < hitboxRadius)
            {
                Console.WriteLine("dist1");
                return true;
            }
            else if (DistanceToLine(hitboxPos, points[1], points[2]) < hitboxRadius)
            {
                Console.WriteLine("dist2");
                return true;
            }
            else if (DistanceToLine(hitboxPos, points[2], points[3]) < hitboxRadius)
            {
                Console.WriteLine("dist3");
                return true;
            }
            else if (DistanceToLine(hitboxPos, points[3], points[0]) < hitboxRadius)
            {
                Console.WriteLine("dist4");
                return true;
            }

            return PointInRectangle(hitboxPos, points) ||
            DistanceToLine(hitboxPos, points[0], points[1]) < hitboxRadius ||
            DistanceToLine(hitboxPos, points[1], points[2]) < hitboxRadius ||
            DistanceToLine(hitboxPos, points[2], points[3]) < hitboxRadius ||
            DistanceToLine(hitboxPos, points[3], points[0]) < hitboxRadius;
        }

        // https://stackoverflow.com/questions/2752725/finding-whether-a-point-lies-inside-a-rectangle-or-not/37865332#37865332
        public static bool PointInRectangle(Vector2f point, Vector2f[] rect)
        {
            var AB = VectorDiff(rect[0], rect[1]);
            var AM = VectorDiff(rect[0], point);
            var BC = VectorDiff(rect[1], rect[2]);
            var BM = VectorDiff(rect[1], point);
            var dotABAM = Dot(AB, AM);
            var dotABAB = Dot(AB, AB);
            var dotBCBM = Dot(BC, BM);
            var dotBCBC = Dot(BC, BC);
            return 0 <= dotABAM && dotABAM <= dotABAB && 0 <= dotBCBM && dotBCBM <= dotBCBC;
        }

        private static Vector2f VectorDiff(Vector2f p1, Vector2f p2)
        {
            return p2 - p1;
        }

        public static float Dot(Vector2f u, Vector2f v)
        {
            return u.X * v.X + u.Y * v.Y;
        }

        public static bool PointIsInRect(Vector2i point, RectangleShape rect)
        {
            return point.X >= rect.Position.X && point.X <= rect.Position.X + rect.Size.X &&
                point.Y >= rect.Position.Y && point.Y <= rect.Position.Y + rect.Size.Y;
        }
}
}
