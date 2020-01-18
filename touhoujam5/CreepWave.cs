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
        public List<Creep> Creeps = new List<Creep>();
        public float Cooldown;
        public float FrameDamage = 0;
        public bool IsFinishedSpawning = false;

        private float _spawnCooldown = 0;
        private float _spawnCounter = 0;
        private int _currentWave = 0;

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
            FrameDamage = 0;
            _spawnCounter += delta;

            // spawn creeps //
            if (_toSpawn.Count > 0)
            {
                if (_spawnCounter >= _spawnCooldown)
                {
                    _spawnCooldown = _toSpawn[0].Cooldown;
                    Creeps.Add(_toSpawn[0]);
                    _toSpawn.RemoveAt(0);
                    _spawnCounter %= _spawnCooldown;
                }
            }
            
            if (_toSpawn.Count == 0)
            {
                if (_spawnCounter >= Cooldown)
                {
                    IsFinishedSpawning = true;
                }
            }

            // update creeps //
            List<Creep> toRemove = new List<Creep>();

            foreach (Creep creep in Creeps)
            {
                creep.Update(delta);

                if (creep.HasReachedEnd)
                {
                    toRemove.Add(creep);
                    FrameDamage += creep.Hp;
                }
            }

            foreach (Creep creep in toRemove)
            {
                Creeps.Remove(creep);
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
            bool toReturn = false;

            foreach (Creep creep in Creeps)
            {
                if (bullet.Intersects(creep))
                {
                    toReturn = true;
                    creep.Damage(bullet);
                    if (!bullet.IsPiercing)
                    {
                        return true;
                    }
                }
            }

            return toReturn;
        }

        public void CullDeadCreeps()
        {
            List<Creep> toRemove = Creeps.FindAll(creep => creep.HasDied);

            foreach (Creep creep in toRemove)
            {
                Game.Money += creep.Worth;
                Creeps.Remove(creep);
            }
        }
    }
}
