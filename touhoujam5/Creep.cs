using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class Creep : HitboxHaver
    {
        private float _baseMoveSpeed;
        public float Hp { get; protected set; }
        public float MoveSpeed => _baseMoveSpeed * MoveSpeedModifier;
        public float MoveSpeedModifier { get; set; }
        public float Cooldown { get; protected set; }
        public float Worth { get; protected set; }
        public bool HasReachedEnd = false;
        public bool HasDied = false;
        public float PathProgress => (float)_pathIndex / _path.Length + _moveProgress * (1 / _path.Length);

        private float _maxHp;
        private float _moveProgress = 0;
        private Vector2i[] _path;
        private int _pathIndex;
        private List<Bullet> _beenHitBy = new List<Bullet>();

        private RectangleShape _hpBackground, _hpForeground;

        protected Creep(string texturePath, int textureIndex, Level level, Hitbox hitbox, float hp, float worth, float moveSpeed, float cooldown, int startIndex)
            : base(texturePath, textureIndex, hitbox)
        {
            _maxHp = Hp = hp;
            _baseMoveSpeed = moveSpeed;
            MoveSpeedModifier = 1;
            Cooldown = cooldown;
            Worth = worth;

            _path = PathFinder.FindPath(level, startIndex);

            var pos = _path[0];
            Position = Utils.Grid2f(pos);

            _hpBackground = new RectangleShape(new Vector2f(Game.TileSize, 8));
            _hpForeground = new RectangleShape(new Vector2f(Game.TileSize, 8));

            _hpBackground.FillColor = Color.Red;
            _hpForeground.FillColor = Color.Green;

            _hpBackground.OutlineColor = _hpForeground.OutlineColor = Color.Black;
            _hpBackground.OutlineThickness = _hpForeground.OutlineThickness = 1;

            _hpBackground.Origin = _hpForeground.Origin = _hpBackground.Size / 2;
        }

        public override void Update(float delta)
        {
            if (_pathIndex == _path.Length)
            {
                return;
            }

            _moveProgress += delta * MoveSpeed;
            if (_moveProgress >= 1)
            {
                _moveProgress %= 1;
                _pathIndex++;

                if (_pathIndex == _path.Length - 1)
                {
                    HasReachedEnd = true;
                    return;
                }
            }

            Vector2f pos = Utils.Grid2f(_path[_pathIndex]);
            Vector2f movingToward = Utils.Grid2f(_path[_pathIndex + 1]);
            Vector2f vDelta = movingToward - pos;

            Vector2f actualPos = pos + vDelta * (_moveProgress);
            Position = actualPos;
        }

        public void Damage(Bullet offendingBullet)
        {
            if (!_beenHitBy.Contains(offendingBullet))
            {
                Hp -= CalculateDamage(offendingBullet);

                if (Hp <= 0)
                {
                    Hp = 0;
                    HasDied = true;
                }

                _hpForeground.Size = new Vector2f(Game.TileSize * (Hp / _maxHp), 8);

                if (!offendingBullet.IsMultiHit)
                {
                    _beenHitBy.Add(offendingBullet);
                }
            }
        }

        protected virtual float CalculateDamage(Bullet offendingBullet)
        {
            return offendingBullet.Strength;
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            var pos = Position - new Vector2f(0, Game.TileSize / 2 + 8);

            _hpBackground.Position = _hpForeground.Position = pos;

            target.Draw(_hpBackground);
            target.Draw(_hpForeground);
        }
    }
}
