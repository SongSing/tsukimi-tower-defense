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
        private bool _isPlaced = false;
        public bool IsPlaced
        {
            get => _isPlaced;
            set
            {
                if (value)
                {
                    HasBeenPlaced = true;
                    OnPlace();
                }
                _isPlaced = value;
            }
        }
        public bool HasBeenPlaced { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Cost => !IsPlaced ? (HasBeenPlaced ? 0 : _costs[0]) : UpgradeCost;
        public float UpgradeCost => Level + 1 == NumLevels ? -1 : _costs[Level + 1];
        public float Worth => _costs[Level] / 2;
        public float ShotRate => _shotRates[Level];
        public float NextShotRate => _shotRates[Level + 1];
        public float Strength => _strengths[Level];
        public float NextStrength => _strengths[Level + 1];
        public float Range => _ranges[Level];
        public float NextRange => _ranges[Level + 1];
        public int Level { get; protected set; }
        public int NumLevels => _costs.Length;

        private float _shotCounter = 0;
        private float[] _ranges, _shotRates, _costs, _strengths;
        private CircleShape _rangeShape;


        protected Tower(string texturePath, int textureIndex, string name, string description, float[] costs, float[] shotRates, float[] ranges, float[] strengths)
            : base(texturePath, textureIndex)
        {
            Name = name;
            Description = description;
            _costs = costs;
            _shotRates = shotRates;
            _ranges = ranges;
            _strengths = strengths;
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

        public virtual void OnPlace()
        {

        }

        public abstract Tower CloneLevel0();

        public abstract string GetExtraInfo(int level);

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
            Draw(target, false);
        }

        public void Draw(RenderTarget target, bool shouldBeRed)
        {
            if (shouldBeRed)
            {
                _sprite.Color = new Color(255, 0, 0);
                _rangeShape.FillColor = new Color(255, 0, 0, 128);
                _rangeShape.OutlineColor = new Color(255, 0, 0);
            }
            else
            {
                _sprite.Color = Color.White;
                _rangeShape.FillColor = new Color(255, 255, 255, 128);
                _rangeShape.OutlineColor = new Color(255, 255, 255, 255);
            }

            if (Game.PlayArea.InfoTower == this)
            {
                _rangeShape.Position = Position;
                target.Draw(_rangeShape);
            }

            base.Draw(target);
        }

        public virtual void LevelUp()
        {
            if (Level != NumLevels - 1)
            {
                Level++;
                _rangeShape.Radius = Range;
                _rangeShape.Origin = new Vector2f(Range, Range);
            }
        }
    }
}
