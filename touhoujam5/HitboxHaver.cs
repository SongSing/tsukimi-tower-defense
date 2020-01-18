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
        public Hitbox Hitbox;
        public Vector2f HitboxPosition => Position + Hitbox.Position;
        public float HitboxRadius => Hitbox.Radius;
        private CircleShape _circleShape;

        public HitboxHaver(string texturePath, int textureIndex, Hitbox hitbox, Vector2f position = new Vector2f())
            : base(texturePath, textureIndex)
        {
            Hitbox = hitbox;
            Position = position;
            _circleShape = new CircleShape(Hitbox.Radius);
            _circleShape.FillColor = new Color(255, 0, 0, 128);
            _circleShape.Origin = new Vector2f(Hitbox.Radius, Hitbox.Radius);
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);
            _circleShape.Position = HitboxPosition;
            //target.Draw(_circleShape);
        }

        public bool Intersects(HitboxHaver other)
        {
            return Hitbox.Intersects(other.Hitbox, Position, other.Position);
        }

        public float DistanceTo(HitboxHaver other)
        {
            return Hitbox.DistanceTo(other.Hitbox, Position, other.Position);
        }

        public override float DistanceTo(Drawable other)
        {
            Vector2f myPos = Position + Hitbox.Position;
            return (float)Math.Sqrt(Math.Pow(other.Position.X - myPos.X, 2) + Math.Pow(other.Position.Y - myPos.Y, 2));
        }

        public float AngleTo(HitboxHaver other)
        {
            return Hitbox.AngleTo(other.Hitbox, Position, other.Position);
        }

        public float AngleTo(Drawable other)
        {
            var delta = (other.Position) - (Position + Hitbox.Position);
            return (float)Math.Atan2(delta.Y, delta.X);
        }
    }
}
