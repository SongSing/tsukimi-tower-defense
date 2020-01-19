﻿using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class AliceTower : Tower
    {
        private float[] _slowAmounts = new float[] { 2, 2.5f, 3, 4 };
        private float[] _damageRates = new float[] { 1, 1.5f, 2, 2.5f };
        private bool _hasShot = false;
        private List<AliceDoll> _dolls = new List<AliceDoll>();

        public AliceTower() : base("Content/towers.png", 3, "Alice Margatroid",
            "Manipulates cute dolls\nto slow down enemies.",
            costs: new float[] { 600, 500, 2000, 8000 },
            shotRates: new float[] { 100, 100, 100, 100 },
            ranges: new float[] { 100, 150, 200, 300 },
            strengths: new float[] { 5, 10, 20, 50 }
        )
        {
            _dolls.Add(new AliceDoll(this, Strength, _damageRates[Level]));
            _dolls.Add(new AliceDoll(this, Strength, _damageRates[Level]));
        }

        public override string GetExtraInfo(int level)
        {
            return "Num Dolls: " + (level + 1).ToString() + "\n" +
                "Slow Amount: " + _slowAmounts[level].ToString() + "x";
        }

        public override Tower CloneLevel0()
        {
            return new AliceTower();
        }

        public override void LevelUp()
        {
            base.LevelUp();

            foreach (var doll in _dolls)
            {
                doll.Strength = Strength;
                doll.DamageRate = _damageRates[Level];
            }

            var newDoll = new AliceDoll(this, Strength, _damageRates[Level]);
            _dolls.Add(newDoll);

            if (_hasShot)
            {
                newDoll.Position = Position + Utils.VectorFromAngle((float)Math.PI * 2) * 50;
            }
            else
            {
                for (int i = 0; i < _dolls.Count; i++)
                {
                    var doll = _dolls[i];
                    doll.Position = Position + Utils.VectorFromAngle(((float)Math.PI * 2) * ((float)i / _dolls.Count)) * 50;
                }
            }

            Game.PlayArea.Bullets.Add(newDoll);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public override void OnPlace()
        {
            base.OnPlace();
            Game.PlayArea.Bullets.AddRange(_dolls);

            for (int i = 0; i < _dolls.Count; i++)
            {
                var doll = _dolls[i];
                doll.Position = Position + Utils.VectorFromAngle(((float)Math.PI * 2) * ((float)i / _dolls.Count)) * 50;
            }
        }

        protected override void Shoot(List<Creep> creepsInRange)
        {
            _hasShot = true;
            List<Creep> targets = new List<Creep>();

            foreach (AliceDoll doll in _dolls)
            {
                if (doll.TargetCreep != null)
                {
                    targets.Add(doll.TargetCreep);
                }
            }

            foreach (AliceDoll doll in _dolls)
            {
                doll.FindNewTargetIfNeeded(creepsInRange, targets);
                if (doll.TargetCreep != null)
                {
                    targets.Add(doll.TargetCreep);
                }
            }
        }
    }
}
