using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class YoumuBullet : Bullet
    {
        private Vector2f _direction;
        private Vector2f _initialPosition;
        private float _slashDuration = 0.25f;
        private float _slashCounter = 0;
        private float _slashWidth;

        public YoumuBullet(Vector2f pos, float strength, float slashWidth, Vector2f direction, bool directionSwitch)
            : base(1, new Hitbox(new Vector2f(0, 0), 8), pos, strength, true)
        {
            if (directionSwitch)
            {
                direction *= -1;
            }

            _direction = direction;
            Position = _initialPosition = Position - (direction * slashWidth * Game.TileSize) / 2;

            if (!directionSwitch)
            {
                _sprite.Scale = new Vector2f(-1, 1);
            }

            if (direction.X == 1)
            {
                _sprite.Rotation = 0;
            }
            else if (direction.X == -1)
            {
                _sprite.Rotation = 0;
            }
            else if (direction.Y == 1)
            {
                _sprite.Rotation = 90;
            }
            else if (direction.Y == -1)
            {
                _sprite.Rotation = 90;
            }

            _sprite.Color = new Color(255, 255, 255, 150);

            _slashWidth = slashWidth;
        }

        public override void Update(float delta)
        {
            float progress = _slashCounter / _slashDuration;
            float distanceToCover = Game.TileSize * _slashWidth;

            Position = _initialPosition + (_direction * distanceToCover) * progress;

            if (progress >= 1)
            {
                ShouldBeCulled = true;
            }

            _slashCounter += delta;
        }
    }
}
