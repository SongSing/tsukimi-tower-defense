using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class FairyCreep1 : Creep
    {
        public FairyCreep1(Level level)
            : base("content/creeps.png", 0, level, new Hitbox(new Vector2f(0, 0), 16), 20, 1, 2)
        {
        }
    }
}
