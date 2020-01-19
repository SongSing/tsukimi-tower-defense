using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class YoumuTower : Tower
    {
        private bool _directionSwitch = false;
        private float[] _slashWidths = new float[] { 1.5f, 2f, 3f, 4f };

        public YoumuTower()
            : base("Content/towers.png", 1, "Youmu Konpaku",
                  "Slashes at adjacent enemies\nwith a sword.",
                  costs: new float[] { 200, 400, 1500, 2500 },
                  shotRates: new float[] { 2, 2.5f, 3, 4 },
                  ranges: new float[] { 44, 44, 44, 44 },
                  strengths: new float[] { 2, 5, 20, 50 })
        { }

        public override string GetExtraInfo(int level)
        {
            return "Slash Width: " + _slashWidths[level].ToString();
        }

        public override Tower CloneLevel0()
        {
            return new YoumuTower();
        }

        protected override void Shoot(List<Creep> creepsInRange)
        {
            Creep closest = Closest(creepsInRange, out float distance);

            float xDist = closest.HitboxPosition.X - Position.X;
            float yDist = closest.HitboxPosition.Y - Position.Y;
            Vector2f posToTarget, direction;

            if (Math.Abs(xDist) > Math.Abs(yDist))
            {
                if (xDist < 0)
                {
                    // left //
                    posToTarget = Position - new Vector2f(Game.TileSize, 0);
                    direction = new Vector2f(0, 1); // slash downwards
                }
                else
                {
                    // right //
                    posToTarget = Position + new Vector2f(Game.TileSize, 0);
                    direction = new Vector2f(0, 1); // slash downwards
                }
            }
            else
            {
                if (yDist < 0)
                {
                    // up //
                    posToTarget = Position - new Vector2f(0, Game.TileSize);
                    direction = new Vector2f(1, 0); // slash sidewards
                }
                else
                {
                    // down //
                    posToTarget = Position + new Vector2f(0, Game.TileSize);
                    direction = new Vector2f(1, 0); // slash sidewards
                }
            }

            var bullet = new YoumuBullet(posToTarget, Strength, _slashWidths[Level], direction, _directionSwitch = !_directionSwitch);
            Game.PlayArea.Bullets.Add(bullet);
        }
    }
}
