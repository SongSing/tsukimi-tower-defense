using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class Hitbox
    {
        public Vector2f Position;
        public float Radius;

        public Hitbox(Vector2f position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Intersects(Hitbox other, Vector2f myPos, Vector2f otherPos)
        {
            float dist = DistanceTo(other, myPos, otherPos);
            return dist < Radius + other.Radius;
        }

        public float DistanceTo(Hitbox other, Vector2f myPos, Vector2f otherPos)
        {
            float term1 = (float)Math.Pow((other.Position.X + otherPos.X) - (Position.X + myPos.X), 2);
            float term2 = (float)Math.Pow((other.Position.Y + otherPos.Y) - (Position.Y + myPos.Y), 2);
            float dist = (float)Math.Sqrt(term1 + term2);
            return dist;
        }

        public float AngleTo(Hitbox other, Vector2f myPos, Vector2f otherPos)
        {
            var delta = (otherPos + other.Position) - (myPos + Position);
            return (float)Math.Atan2(delta.Y, delta.X);
        }
    }
}
