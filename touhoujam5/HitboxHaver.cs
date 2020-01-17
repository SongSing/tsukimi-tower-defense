using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class HitboxHaver : Drawable
    {
        private Hitbox _hitbox;
        public Vector2f HitboxPosition => Position + _hitbox.Position;

        public HitboxHaver(string texturePath, int textureIndex, Hitbox hitbox, Vector2f position = new Vector2f())
            : base(texturePath, textureIndex)
        {
            _hitbox = hitbox;
            Position = position;
        }

        public bool Intersects(HitboxHaver other)
        {
            return _hitbox.Intersects(other._hitbox, Position, other.Position);
        }

        public float DistanceTo(HitboxHaver other)
        {
            return _hitbox.DistanceTo(other._hitbox, Position, other.Position);
        }

        public override float DistanceTo(Drawable other)
        {
            Vector2f myPos = Position + _hitbox.Position;
            return (float)Math.Sqrt(Math.Pow(other.Position.X - myPos.X, 2) + Math.Pow(other.Position.Y - myPos.Y, 2));
        }

        public float AngleTo(HitboxHaver other)
        {
            return _hitbox.AngleTo(other._hitbox, Position, other.Position);
        }
    }
}
