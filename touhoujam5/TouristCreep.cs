using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class TouristCreep : Creep
    {
        private bool _enraged;
        public override float Worth => _enraged ? 2 : 1;
        public TouristCreep(Level level, bool enraged)
            : base("content/creeps.png", enraged ? 3 : 2, level, new Hitbox(new Vector2f(0, 0), 8), 5, enraged ? 2.5f : 1.5f, 0.1f)
        {
            _enraged = enraged;
        }
    }
}
