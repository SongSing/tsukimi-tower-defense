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
        public ReimuTower(Vector2i pos) : base(pos, "Content/towers.png", 0, new float[] { 100, 100, 200, 300 }, 1, new float[] { 200, 300, 400, 500 })
        {
        }

        protected override void Shoot(List<Creep> creepsInRange)
        {
            int toShoot = Level + 4;
            for (int i = 0; i < toShoot; i++)
            {
                Game.PlayArea.Bullets.Add(new ReimuBullet(Position, ((float)Math.PI * 2) / toShoot * i));
            }
        }
    }
}
