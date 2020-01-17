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
    }
}
