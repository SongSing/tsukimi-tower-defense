using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class CreepWave : Entity
    {
        private List<Creep> _toSpawn;
        private float _spawnCooldown = 0;
        private float _spawnCounter = 0;
        public List<Creep> Creeps = new List<Creep>();
        public float Cooldown;

        public CreepWave(float cooldown, List<Creep> creeps)
        {
            _toSpawn = creeps;
            Cooldown = cooldown;

            _spawnCooldown = _toSpawn[0].Cooldown;
            Creeps.Add(_toSpawn[0]);
            _toSpawn.RemoveAt(0);
        }

        public void Update(float delta)
        {
            if (_toSpawn.Count > 0)
            {
                _spawnCounter += delta;
                if (_spawnCounter >= _spawnCooldown)
                {
                    _spawnCooldown = _toSpawn[0].Cooldown;
                    Creeps.Add(_toSpawn[0]);
                    _toSpawn.RemoveAt(0);
                    _spawnCounter = 0;
                }
            }

            foreach (Creep creep in Creeps)
            {
                creep.Update(delta);
            }
        }

        public void Draw(RenderTarget target)
        {
            foreach (Creep creep in Creeps)
            {
                creep.Draw(target);
            }
        }

        /// <summary>
        /// Test for collision. Will handle damaging and removing the creep if necessary, but will not destroy the bullet.
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns>Whether or not a collision occured.</returns>
        public bool TryBullet(Bullet bullet)
        {
            List<Creep> toRemove = new List<Creep>();
            bool toReturn = false;

            foreach (Creep creep in Creeps)
            {
                if (bullet.Intersects(creep))
                {
                    toReturn = true;
                    creep.Damage(bullet.Strength);
                    if (creep.Hp <= 0)
                    {
                        toRemove.Add(creep);
                    }
                }
            }

            foreach (Creep creep in toRemove)
            {
                Creeps.Remove(creep);
            }

            return toReturn;
        }
    }
}
