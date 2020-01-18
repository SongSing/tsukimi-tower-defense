using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class FairyCreep2 : Creep
    {
        public override float Worth => 20;
        public FairyCreep2(Level level)
            : base("content/creeps.png", 1, level, new Hitbox(new Vector2f(0, 0), 16), 40, 0.8f, 1)
        {
        }
    }
}
