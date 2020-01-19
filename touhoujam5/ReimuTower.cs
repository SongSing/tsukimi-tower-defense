using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class ReimuTower : Tower
    {
        private int[] _numBullets = new int[] { 3, 4, 5, 6 };
        public ReimuTower()
            : base("Content/towers.png", 0, "Reimu Hakurei",
                  "Fires homing bullets that\nseek the closest enemy.",
                  costs: new float[] { 200, 400, 1000, 5000 },
                  shotRates: new float[] { 1.5f, 2, 3, 4 },
                  ranges: new float[] { 100, 150, 250, 400 },
                  strengths: new float[] { 1, 2, 5, 10 })
        {

        }

        public override string GetExtraInfo(int level)
        {
            return "Num Bullets: " + _numBullets[level].ToString();
        }

        public override Tower CloneLevel0()
        {
            return new ReimuTower();
        }

        protected override void Shoot(List<Creep> creepsInRange)
        {
            int toShoot = _numBullets[Level];
            for (int i = 0; i < toShoot; i++)
            {
                Game.PlayArea.Bullets.Add(new ReimuBullet(Position, ((float)Math.PI * 2) / toShoot * i, Strength));
            }
        }
    }
}
