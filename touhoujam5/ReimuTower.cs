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
        private int[] _numBullets = new int[] { 4, 4, 5, 6 };
        public ReimuTower()
            : base("Content/towers.png", 0, "Reimu Hakurei",
                  "Fires homing bullets that\nseek the closest enemy.",
                  new float[] { 100, 200, 500, 1500 },
                  new float[] { 1, 1, 2, 3 },
                  new float[] { 150, 200, 280, 400 },
                  new float[] { 1, 3, 5, 10 })
        {

        }

        public override string GetExtraInfo(int level)
        {
            return "Num Bullets: " + _numBullets[level].ToString();
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
