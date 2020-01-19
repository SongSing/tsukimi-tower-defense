using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    class TaroCreep : Creep
    {
        public TaroCreep(Level level, float hp, float worth, float moveSpeed, int startIndex, float cooldown)
            : base("content/creeps.png", 0, level, new Hitbox(new Vector2f(0, 0), 12), hp, worth, moveSpeed, cooldown, startIndex)
        {
        }
    }
}
