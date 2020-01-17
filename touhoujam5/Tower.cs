using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class Tower : Drawable
    {
        public bool IsPlaced { get; set; }
        public float[] Cost;
        public float ShotRate;
        public float Range => _ranges[Level];
        public int Level;
        public int NumLevels => Cost.Length;

        private float _shotCounter = 0;
        private float[] _ranges;
        private CircleShape _rangeShape;


        protected Tower(Vector2i pos, string texturePath, int textureIndex, float[] cost, float shotRate, float[] ranges)
            : base(texturePath, textureIndex)
        {
            Cost = cost;
            ShotRate = shotRate;
            Position = Utils.Grid2f(pos);
            _ranges = ranges;
            Level = 0;
            IsPlaced = false;

            _rangeShape = new CircleShape(Range)
            {
                FillColor = new Color(255, 255, 255, 128),
                OutlineColor = new Color(255, 255, 255, 255),
                OutlineThickness = 2,
                Origin = new Vector2f(Range, Range)
            };
        }

        /// <summary>
        /// Should add bullets to Bullets
        /// </summary>
        protected abstract void Shoot(List<Creep> creepsInRange);

        public override void Update(float delta)
        {
            _shotCounter += delta;
            float goal = 1 / ShotRate;

            if (_shotCounter >= goal)
            {
                _shotCounter %= goal;
                var creepsInRange = GetCreepsInRange();

                if (creepsInRange.Count > 0)
                {
                    Shoot(creepsInRange);
                }
            }
        }

        private List<Creep> GetCreepsInRange()
        {
            return Game.PlayArea.GetCreeps().FindAll(creep => creep.DistanceTo(this) < Range);
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            if (Game.PlayArea.InfoTower == this)
            {
                _rangeShape.Position = Position;
                target.Draw(_rangeShape);
            }
        }
    }
}
