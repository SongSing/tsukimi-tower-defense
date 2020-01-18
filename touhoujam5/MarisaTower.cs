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

        private float[] _laserWidths = new float[] { 5, 16, 48, 128 };
        private MarisaLaser _laser;

        public MarisaTower() : base("Content/towers.png", 2, "Marisa Kirisame",
            "Fires a piercing laser at\nthe farthest enemy in range.",
            new float[] { 100, 300, 500, 1000 },
            new float[] { 100, 100, 100, 100 },
            new float[] { 300, 350, 400, 500 },
            new float[] { 1.5f, 5, 10, 20 }
        )
        {
            _laser = new MarisaLaser(this, Strength, _laserWidths[0]);
        }

        public override string GetExtraInfo(int level)
        {
            return "Laser Width: " + _laserWidths[level].ToString();
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
    }
}
