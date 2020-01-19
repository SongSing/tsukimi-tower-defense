using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class AliceDoll : Bullet
    {
        public float DamageRate;
        public float ChaseDuration = 0.5f;
        public override bool IsCollidable => false;
        public override bool IsMultiHit => true;

        public Creep TargetCreep;
        private bool _isAttached = false;
        private float _damageCounter = 0;
        private float _chaseCounter = 0;
        private AliceTower _parentTower;
        private Vector2f _chaseStartingPosition;

        public float SlowModifier => _parentTower.SlowModifier;

        public AliceDoll(AliceTower parentTower, float strength, float damageRate)
            : base(3, new Hitbox(new Vector2f(0, 0), 8), parentTower.Position, strength, true)
        {
            DamageRate = damageRate;
            _parentTower = parentTower;
        }

        public void Kill()
        {
            ShouldBeCulled = true;
            _isAttached = false;
            if (TargetCreep != null && TargetCreep.AttachedDolls.Contains(this))
            {
                TargetCreep.AttachedDolls.Remove(this);
            }
        }

        public void FindNewTargetIfNeeded(List<Creep> creepsInRange, List<Creep> ignore)
        {
            if (!ShouldBeCulled && (TargetCreep != null && !creepsInRange.Contains(TargetCreep)) ||
                TargetCreep == null || (TargetCreep != null && TargetCreep.HasDied))
            {
                FindNewTarget(creepsInRange, ignore);
            }
        }

        private void FindNewTarget(List<Creep> candidates, List<Creep> ignore)
        {
            if (TargetCreep != null && TargetCreep.AttachedDolls.Contains(this))
            {
                TargetCreep.AttachedDolls.Remove(this);
            }

            _isAttached = false;
            var creeps = new List<Creep>(candidates);
            creeps.RemoveAll(creep => ignore.Contains(creep));

            if (creeps.Count > 0)
            {
                Creep fastest = creeps[0];
                float fastestSpeed = fastest.PathProgress;

                for (int i = 1; i < creeps.Count; i++)
                {
                    var creep = creeps[i];
                    float speed = creep.PathProgress;

                    if (speed > fastestSpeed)
                    {
                        fastestSpeed = speed;
                        fastest = creep;
                    }
                }

                TargetCreep = fastest;
                TargetCreep.AttachedDolls.Add(this);
                _chaseStartingPosition = Position;
                _chaseCounter = 0;
            }
            else
            {
                TargetCreep = null;
            }
        }

        public override void Update(float delta)
        {
            if (_isAttached)
            {
                Position = TargetCreep.Position;
                _damageCounter += delta;

                if (_damageCounter >= 1 / DamageRate)
                {
                    TargetCreep.Damage(this);
                    _damageCounter %= 1 / DamageRate;
                }

                if (TargetCreep.HasDied)
                {
                    _isAttached = false;
                    TargetCreep = null;
                }
            }
            else if (TargetCreep != null)
            {
                float angle = Utils.AngleBetween(_chaseStartingPosition, TargetCreep.Position);
                float dist = Utils.DistanceBetween(_chaseStartingPosition, TargetCreep.Position);
                Position = _chaseStartingPosition + Utils.VectorFromAngle(angle) * (_chaseCounter / ChaseDuration) * dist;

                _chaseCounter += delta;
                if (_chaseCounter >= ChaseDuration)
                {
                    _isAttached = true;
                    _damageCounter = 0;
                }
            }
            else if (TargetCreep == null)
            {
                float angle = Utils.AngleBetween(Position, _parentTower.Position);
                float dist = Utils.DistanceBetween(Position, _parentTower.Position);
                if (dist < 50)
                {
                    angle += (float)Math.PI / 2;
                }
                Position += Utils.VectorFromAngle(angle) * delta * 100;
            }
        }

        public override void Draw(RenderTarget target)
        {
            _sprite.Color = new Color(255, 255, 255, (byte)(_isAttached ? 128 : 255));
            base.Draw(target);
        }
    }
}
