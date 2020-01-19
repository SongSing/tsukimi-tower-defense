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
        public bool IsPiercing;
        public bool ShouldBeCulled = false;
        public virtual bool IsCollidable => true;
        public virtual bool IsMultiHit => false;

        public Bullet(int textureIndex, Hitbox hitbox, Vector2f position, float strength, bool isPiercing) : base("Content/bullets.png", textureIndex, hitbox, position)
        {
            Strength = strength;
            IsPiercing = isPiercing;
        }

        /// <summary>
        /// Handle collision with a creep. Returns whether or not testing should continue for this bullet.
        /// </summary>
        /// <param name="creep"></param>
        /// <returns></returns>
        public virtual bool OnCollide(Creep creep)
        {
            creep.Damage(this);

            if (!this.IsPiercing)
            {
                ShouldBeCulled = true;
            }

            return this.IsPiercing;
        }
    }
}
