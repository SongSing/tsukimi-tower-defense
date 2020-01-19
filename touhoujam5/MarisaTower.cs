using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class MarisaTower : Tower
    {

        private float[] _laserWidths = new float[] { 16, 48, 96, 192 };
        private MarisaLaser _laser;

        public MarisaTower() : base("Content/towers.png", 2, "Marisa Kirisame",
            "Fires a piercing laser at\nthe farthest enemy in range.",
            costs: new float[] { 500, 1000, 1750, 2200 },
            shotRates: new float[] { 100, 100, 100, 100 },
            ranges: new float[] { 300, 400, 500, 600 },
            strengths: new float[] { 1.5f, 5, 15, 30 }
        )
        {
            _laser = new MarisaLaser(this, Strength, _laserWidths[0]);
        }

        public override string GetExtraInfo(int level)
        {
            return "Laser Width: " + _laserWidths[level].ToString();
        }

        public override Tower CloneLevel0()
        {
            return new MarisaTower();
        }

        protected override void Shoot(List<Creep> creepsInRange)
        {
            Creep farthest = Farthest(creepsInRange, out float distance);

            _laser.TargetCreep = farthest;
            if (!Game.PlayArea.Bullets.Contains(_laser))
            {
                _laser.ShouldBeCulled = false;
                Game.PlayArea.Bullets.Add(_laser);
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            _laser.Width = _laserWidths[Level];
            _laser.Strength = Strength;
        }

        public override void BeforeRemove()
        {
            base.BeforeRemove();

            _laser.ShouldBeCulled = true;
        }
    }
}
