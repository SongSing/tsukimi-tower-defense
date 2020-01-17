using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class ReimuBullet : Bullet
    {
        private float _angle;

        public ReimuBullet(Vector2f position, float angle) : base("Content/bullets.png", 0, new Hitbox(new Vector2f(0, 0), 8), position, 1)
        {
            _angle = angle;
        }

        public override void Update(float delta)
        {
            var creeps = Game.PlayArea.GetCreeps();

            if (creeps.Count > 0)
            {
                Creep closest = creeps[0];
                float closestDist = DistanceTo(closest);

                for (int i = 1; i < creeps.Count; i++)
                {
                    var creep = creeps[i];
                    float dist = DistanceTo(creep);

                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = creep;
                    }
                }

                float angle = AngleTo(closest);
                float deltaAngle = angle - _angle;

                if (deltaAngle > (float)Math.PI)
                {
                    deltaAngle -= (float)Math.PI * 2;
                }

                if (deltaAngle < -(float)Math.PI)
                {
                    deltaAngle += (float)Math.PI * 2;
                }

                deltaAngle /= delta * 1000 * (closestDist / 100);
                _angle += deltaAngle;
            }

            Position += new Vector2f((float)Math.Cos(_angle) * delta * 200, (float)Math.Sin(_angle) * delta * 200);
        }
    }
}
