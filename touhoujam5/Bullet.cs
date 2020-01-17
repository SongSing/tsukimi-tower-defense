using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace touhoujam5
{
    abstract class Bullet : HitboxHaver
    {
        public float Strength;
        public Bullet(string textureString, int textureIndex, Hitbox hitbox, Vector2f position, float strength) : base(textureString, textureIndex, hitbox, position)
        {
            Strength = strength;
        }
    }
}
