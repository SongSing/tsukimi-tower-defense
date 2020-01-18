using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class Drawable : Entity
    {
        private Texture _texture;
        protected Sprite _sprite;
        public Vector2f Position
        {
            get => _sprite.Position;
            set => _sprite.Position = value;
        }

        public Drawable(string texturePath, int textureIndex)
        {
            _texture = Resources.Texture(texturePath);
            _sprite = new Sprite(_texture, new IntRect(textureIndex * Game.TileSize, 0, Game.TileSize, (int)_texture.Size.Y));
            _sprite.Origin = new Vector2f(Game.TileSize, Game.TileSize) / 2;
        }

        public abstract void Update(float delta);

        public virtual void Draw(RenderTarget target)
        {
            target.Draw(_sprite);
        }

        public virtual float DistanceTo(Drawable other)
        {
            return (float)Math.Sqrt(Math.Pow(other.Position.X - Position.X, 2) + Math.Pow(other.Position.Y - Position.Y, 2));
        }

        public virtual T Closest<T>(List<T> candidates, out float distance) where T : HitboxHaver
        {
            if (candidates.Count == 0)
            {
                throw new Exception("empty list not valid for closest");
            }

            T closest = candidates[0];
            float closestDist = closest.DistanceTo(this);

            for (int i = 1; i < candidates.Count; i++)
            {
                float dist = candidates[i].DistanceTo(this);
                if (dist < closestDist)
                {
                    closest = candidates[i];
                    closestDist = dist;
                }
            }

            distance = closestDist;
            return closest;
        }

        public virtual T Farthest<T>(List<T> candidates, out float distance) where T : HitboxHaver
        {
            if (candidates.Count == 0)
            {
                throw new Exception("empty list not valid for closest");
            }

            T farthest = candidates[0];
            float farthestDist = farthest.DistanceTo(this);

            for (int i = 1; i < candidates.Count; i++)
            {
                float dist = candidates[i].DistanceTo(this);
                if (dist > farthestDist)
                {
                    farthest = candidates[i];
                    farthestDist = dist;
                }
            }

            distance = farthestDist;
            return farthest;
        }
    }
}
